using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using NouAssist.StartupEntrepreneurship.FundraisingPipeline.Aggregates;
using NouAssist.StartupEntrepreneurship.FundraisingPipeline.Entities;
using NouAssist.StartupEntrepreneurship.FundraisingPipeline.PitchDeck.Dtos;
using NouAssist.StartupEntrepreneurship.FundraisingPipeline.Permissions;
using NouAssist.StartupEntrepreneurship.FundraisingPipeline.Repositories;
using NouAssist.StartupEntrepreneurship.FundraisingPipeline.Services; // For PitchEffectivenessResult
using NouAssist.StartupEntrepreneurship.FundraisingPipeline.Services.Interfaces;
using NouAssist.StartupEntrepreneurship.FundraisingPipeline.ValueObjects;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;
using Volo.Abp.ObjectMapping;

namespace NouAssist.StartupEntrepreneurship.FundraisingPipeline.PitchDeck
{
    [Authorize(FundraisingPipelinePermissions.PitchDeck.Default)]
    public class PitchDeckAppService : ApplicationService, IPitchDeckAppService
    {
        private readonly IPitchDeckRepository _pitchDeckRepository;
        private readonly IFundraisingRoundRepository _fundraisingRoundRepository;
        private readonly IProspectRepository _prospectRepository; // For prospect names
        private readonly IPitchEffectivenessAnalyzer _pitchEffectivenessAnalyzer;
        private readonly IGuidGenerator _guidGenerator;

        public PitchDeckAppService(
            IPitchDeckRepository pitchDeckRepository,
            IFundraisingRoundRepository fundraisingRoundRepository,
            IProspectRepository prospectRepository,
            IPitchEffectivenessAnalyzer pitchEffectivenessAnalyzer,
            IGuidGenerator guidGenerator)
        {
            _pitchDeckRepository = pitchDeckRepository;
            _fundraisingRoundRepository = fundraisingRoundRepository;
            _prospectRepository = prospectRepository;
            _pitchEffectivenessAnalyzer = pitchEffectivenessAnalyzer;
            _guidGenerator = guidGenerator;
        }

        [Authorize(FundraisingPipelinePermissions.PitchDeck.View)]
        public async Task<PitchDeckDto> GetAsync(Guid id)
        {
            try
            {
                var pitchDeck = await _pitchDeckRepository.GetWithDetailsAsync(id);
                if (pitchDeck == null)
                {
                    throw new EntityNotFoundException(typeof(Aggregates.PitchDeck), id);
                }
                
                var dto = ObjectMapper.Map<Aggregates.PitchDeck, PitchDeckDto>(pitchDeck);
                
                if (pitchDeck.FundraisingRoundId.HasValue)
                {
                    var round = await _fundraisingRoundRepository.FindAsync(pitchDeck.FundraisingRoundId.Value);
                    dto.FundraisingRoundName = round?.Name;
                }

                if (pitchDeck.Feedbacks.Any())
                {
                    dto.AverageRating = pitchDeck.Feedbacks.Average(f => f.Feedback.Rating);
                }

                return dto;
            }
            catch (EntityNotFoundException)
            {
                throw new UserFriendlyException(L["FundraisingPipeline:ErrorMessage:PitchDeckNotFound", id]);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error retrieving pitch deck with ID {PitchDeckId}", id);
                throw new UserFriendlyException(L["FundraisingPipeline:ErrorMessage:GenericError"]);
            }
        }

        [Authorize(FundraisingPipelinePermissions.PitchDeck.View)]
        public async Task<PagedResultDto<PitchDeckDto>> GetListAsync(GetPitchDeckListDto input)
        {
            try
            {
                var query = await _pitchDeckRepository.WithDetailsAsync();

                if (!string.IsNullOrWhiteSpace(input.Filter))
                {
                    var filterLower = input.Filter.ToLower();
                    query = query.Where(pd => 
                        pd.Title.ToLower().Contains(filterLower) || 
                        (pd.Description != null && pd.Description.ToLower().Contains(filterLower)));
                }

                if (input.FundraisingRoundId.HasValue)
                {
                    query = query.Where(pd => pd.FundraisingRoundId == input.FundraisingRoundId.Value);
                }
                
                query = ApplySorting(query, input);

                var totalCount = await AsyncExecuter.CountAsync(query);
                query = query.Skip(input.SkipCount).Take(input.MaxResultCount);

                var pitchDecks = await AsyncExecuter.ToListAsync(query);
                var dtos = ObjectMapper.Map<List<Aggregates.PitchDeck>, List<PitchDeckDto>>(pitchDecks);

                await PopulateExtraDetailsForPitchDeckList(dtos, pitchDecks);

                return new PagedResultDto<PitchDeckDto>(totalCount, dtos);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error retrieving pitch deck list");
                throw new UserFriendlyException(L["FundraisingPipeline:ErrorMessage:GenericError"]);
            }
        }
        
        private async Task PopulateExtraDetailsForPitchDeckList(List<PitchDeckDto> dtos, List<Aggregates.PitchDeck> entities)
        {
            var roundIds = entities
                .Where(pd => pd.FundraisingRoundId.HasValue)
                .Select(pd => pd.FundraisingRoundId.Value)
                .Distinct().ToList();

            Dictionary<Guid, string> roundNames = new Dictionary<Guid, string>();
            if (roundIds.Any())
            {
                var rounds = await _fundraisingRoundRepository.GetListAsync(r => roundIds.Contains(r.Id));
                roundNames = rounds.ToDictionary(r => r.Id, r => r.Name);
            }

            for(int i=0; i < dtos.Count; i++)
            {
                var dto = dtos[i];
                var entity = entities[i]; // Assuming dtos and entities are in the same order

                if (dto.FundraisingRoundId.HasValue && roundNames.TryGetValue(dto.FundraisingRoundId.Value, out var roundName))
                {
                    dto.FundraisingRoundName = roundName;
                }
                if (entity.Feedbacks.Any())
                {
                    dto.AverageRating = entity.Feedbacks.Average(f => f.Feedback.Rating);
                }
            }
        }


        [Authorize(FundraisingPipelinePermissions.PitchDeck.Create)]
        public async Task<PitchDeckDto> CreateAsync(CreatePitchDeckDto input)
        {
            try
            {
                if (input.FundraisingRoundId.HasValue)
                {
                    var roundExists = await _fundraisingRoundRepository.FindAsync(input.FundraisingRoundId.Value);
                    if (roundExists == null)
                    {
                         throw new UserFriendlyException(L["FundraisingPipeline:ErrorMessage:FundraisingRoundNotFound", input.FundraisingRoundId.Value]);
                    }
                }

                var pitchDeck = Aggregates.PitchDeck.Create(
                    _guidGenerator.Create(),
                    input.Title,
                    input.Description,
                    input.FundraisingRoundId,
                    CurrentTenant.Id
                );

                await _pitchDeckRepository.InsertAsync(pitchDeck, autoSave: true);
                var dto = ObjectMapper.Map<Aggregates.PitchDeck, PitchDeckDto>(pitchDeck);
                 if (dto.FundraisingRoundId.HasValue)
                {
                    var round = await _fundraisingRoundRepository.FindAsync(dto.FundraisingRoundId.Value);
                    dto.FundraisingRoundName = round?.Name;
                }
                return dto;
            }
            catch (UserFriendlyException)
            {
                throw;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error creating pitch deck");
                throw new UserFriendlyException(L["FundraisingPipeline:ErrorMessage:GenericError"]);
            }
        }

        [Authorize(FundraisingPipelinePermissions.PitchDeck.Update)]
        public async Task<PitchDeckDto> UpdateAsync(Guid id, UpdatePitchDeckDto input)
        {
            try
            {
                var pitchDeck = await _pitchDeckRepository.GetAsync(id);
                 if (pitchDeck == null)
                {
                    throw new EntityNotFoundException(typeof(Aggregates.PitchDeck), id);
                }

                if (input.FundraisingRoundId.HasValue)
                {
                     var roundExists = await _fundraisingRoundRepository.FindAsync(input.FundraisingRoundId.Value);
                    if (roundExists == null)
                    {
                         throw new UserFriendlyException(L["FundraisingPipeline:ErrorMessage:FundraisingRoundNotFound", input.FundraisingRoundId.Value]);
                    }
                }
                
                pitchDeck.UpdateBasicInfo(input.Title, input.Description, input.FundraisingRoundId);
                await _pitchDeckRepository.UpdateAsync(pitchDeck, autoSave: true);
                
                var updatedPitchDeck = await _pitchDeckRepository.GetWithDetailsAsync(id); // Re-fetch to get all details
                var dto = ObjectMapper.Map<Aggregates.PitchDeck, PitchDeckDto>(updatedPitchDeck);
                if (dto.FundraisingRoundId.HasValue)
                {
                    var round = await _fundraisingRoundRepository.FindAsync(dto.FundraisingRoundId.Value);
                    dto.FundraisingRoundName = round?.Name;
                }
                 if (updatedPitchDeck.Feedbacks.Any())
                {
                    dto.AverageRating = updatedPitchDeck.Feedbacks.Average(f => f.Feedback.Rating);
                }
                return dto;
            }
            catch (EntityNotFoundException)
            {
                throw new UserFriendlyException(L["FundraisingPipeline:ErrorMessage:PitchDeckNotFound", id]);
            }
            catch (UserFriendlyException)
            {
                throw;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error updating pitch deck {PitchDeckId}", id);
                throw new UserFriendlyException(L["FundraisingPipeline:ErrorMessage:GenericError"]);
            }
        }

        [Authorize(FundraisingPipelinePermissions.PitchDeck.Delete)]
        public async Task DeleteAsync(Guid id)
        {
            try
            {
                await _pitchDeckRepository.DeleteAsync(id, autoSave: true);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error deleting pitch deck {PitchDeckId}", id);
                var pitchDeck = await _pitchDeckRepository.FindAsync(id);
                if (pitchDeck == null)
                {
                    Logger.LogWarning("Attempted to delete a pitch deck that was not found: {PitchDeckId}", id);
                    return;
                }
                throw new UserFriendlyException(L["FundraisingPipeline:ErrorMessage:GenericError"]);
            }
        }

        [Authorize(FundraisingPipelinePermissions.PitchDeck.Update)]
        public async Task<PitchVersionDto> AddVersionAsync(Guid id, CreatePitchVersionDto input)
        {
            try
            {
                var pitchDeck = await _pitchDeckRepository.GetWithDetailsAsync(id);
                if (pitchDeck == null)
                {
                    throw new EntityNotFoundException(typeof(Aggregates.PitchDeck), id);
                }

                var content = ObjectMapper.Map<CreateDeckContentDto, DeckContent>(input.Content);
                var version = pitchDeck.AddVersion(input.VersionName, content, input.ChangeNotes);
                
                await _pitchDeckRepository.UpdateAsync(pitchDeck, autoSave: true);
                return ObjectMapper.Map<PitchVersion, PitchVersionDto>(version);
            }
            catch (EntityNotFoundException)
            {
                throw new UserFriendlyException(L["FundraisingPipeline:ErrorMessage:PitchDeckNotFound", id]);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error adding version to pitch deck {PitchDeckId}", id);
                throw new UserFriendlyException(L["FundraisingPipeline:ErrorMessage:GenericError"]);
            }
        }

        [Authorize(FundraisingPipelinePermissions.PitchDeck.Update)]
        public async Task<PitchVersionDto> UpdateVersionAsync(Guid id, Guid versionId, UpdatePitchVersionDto input)
        {
            try
            {
                var pitchDeck = await _pitchDeckRepository.GetWithDetailsAsync(id);
                 if (pitchDeck == null)
                {
                    throw new EntityNotFoundException(typeof(Aggregates.PitchDeck), id);
                }

                var content = ObjectMapper.Map<UpdateDeckContentDto, DeckContent>(input.Content);
                pitchDeck.UpdateVersion(versionId, input.VersionName, content, input.ChangeNotes);

                await _pitchDeckRepository.UpdateAsync(pitchDeck, autoSave: true);
                var updatedVersion = pitchDeck.Versions.First(v => v.Id == versionId);
                return ObjectMapper.Map<PitchVersion, PitchVersionDto>(updatedVersion);
            }
            catch (EntityNotFoundException)
            {
                throw new UserFriendlyException(L["FundraisingPipeline:ErrorMessage:PitchDeckNotFound", id]);
            }
            catch (BusinessException ex) // e.g. PitchVersionNotFound
            {
                throw new UserFriendlyException(ex.Message);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error updating version {VersionId} for pitch deck {PitchDeckId}", versionId, id);
                throw new UserFriendlyException(L["FundraisingPipeline:ErrorMessage:GenericError"]);
            }
        }
        
        [Authorize(FundraisingPipelinePermissions.PitchDeck.View)]
        public async Task<List<PitchVersionDto>> GetVersionsAsync(Guid id)
        {
            try
            {
                var pitchDeck = await _pitchDeckRepository.GetWithDetailsAsync(id);
                 if (pitchDeck == null)
                {
                    throw new EntityNotFoundException(typeof(Aggregates.PitchDeck), id);
                }
                return ObjectMapper.Map<List<PitchVersion>, List<PitchVersionDto>>(pitchDeck.Versions.ToList());
            }
            catch (EntityNotFoundException)
            {
                throw new UserFriendlyException(L["FundraisingPipeline:ErrorMessage:PitchDeckNotFound", id]);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error retrieving versions for pitch deck {PitchDeckId}", id);
                throw new UserFriendlyException(L["FundraisingPipeline:ErrorMessage:GenericError"]);
            }
        }


        [Authorize(FundraisingPipelinePermissions.PitchDeck.Update)]
        public async Task<PitchPresentationDto> AddPresentationAsync(Guid id, CreatePresentationDto input)
        {
            try
            {
                var pitchDeck = await _pitchDeckRepository.GetWithDetailsAsync(id);
                if (pitchDeck == null)
                {
                    throw new EntityNotFoundException(typeof(Aggregates.PitchDeck), id);
                }
                
                // Validate Prospect
                var prospect = await _prospectRepository.FindAsync(input.ProspectId);
                if (prospect == null)
                {
                    throw new UserFriendlyException(L["FundraisingPipeline:ErrorMessage:ProspectNotFound", input.ProspectId]);
                }

                var details = ObjectMapper.Map<CreatePresentationDetailsDto, PresentationDetails>(input.Details);
                var presentation = pitchDeck.AddPresentation(input.VersionId, input.ProspectId, details);
                
                await _pitchDeckRepository.UpdateAsync(pitchDeck, autoSave: true);
                
                var presentationDto = ObjectMapper.Map<PitchPresentation, PitchPresentationDto>(presentation);
                var version = pitchDeck.Versions.FirstOrDefault(v => v.Id == input.VersionId);
                presentationDto.VersionName = version?.VersionName;
                presentationDto.ProspectName = prospect.Name;
                presentationDto.ProspectCompany = prospect.Company;

                return presentationDto;
            }
            catch (EntityNotFoundException)
            {
                throw new UserFriendlyException(L["FundraisingPipeline:ErrorMessage:PitchDeckNotFound", id]);
            }
            catch (BusinessException ex) // e.g. PitchVersionNotFound
            {
                throw new UserFriendlyException(ex.Message);
            }
            catch (UserFriendlyException)
            {
                throw;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error adding presentation to pitch deck {PitchDeckId}", id);
                throw new UserFriendlyException(L["FundraisingPipeline:ErrorMessage:GenericError"]);
            }
        }

        [Authorize(FundraisingPipelinePermissions.PitchDeck.Update)]
        public async Task<PitchPresentationDto> UpdatePresentationAsync(Guid id, Guid presentationId, UpdatePresentationDto input)
        {
            try
            {
                var pitchDeck = await _pitchDeckRepository.GetWithDetailsAsync(id);
                if (pitchDeck == null)
                {
                    throw new EntityNotFoundException(typeof(Aggregates.PitchDeck), id);
                }
                
                // Validate Prospect
                var prospect = await _prospectRepository.FindAsync(input.ProspectId);
                if (prospect == null)
                {
                    throw new UserFriendlyException(L["FundraisingPipeline:ErrorMessage:ProspectNotFound", input.ProspectId]);
                }

                var details = ObjectMapper.Map<UpdatePresentationDetailsDto, PresentationDetails>(input.Details);
                pitchDeck.UpdatePresentation(presentationId, input.VersionId, input.ProspectId, details);

                await _pitchDeckRepository.UpdateAsync(pitchDeck, autoSave: true);
                
                var updatedPresentation = pitchDeck.Presentations.First(p => p.Id == presentationId);
                var presentationDto = ObjectMapper.Map<PitchPresentation, PitchPresentationDto>(updatedPresentation);

                var version = pitchDeck.Versions.FirstOrDefault(v => v.Id == input.VersionId);
                presentationDto.VersionName = version?.VersionName;
                presentationDto.ProspectName = prospect.Name;
                presentationDto.ProspectCompany = prospect.Company;
                
                return presentationDto;
            }
            catch (EntityNotFoundException)
            {
                throw new UserFriendlyException(L["FundraisingPipeline:ErrorMessage:PitchDeckNotFound", id]);
            }
            catch (BusinessException ex) // e.g. PresentationNotFound, PitchVersionNotFound
            {
                throw new UserFriendlyException(ex.Message);
            }
            catch (UserFriendlyException)
            {
                throw;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error updating presentation {PresentationId} for pitch deck {PitchDeckId}", presentationId, id);
                throw new UserFriendlyException(L["FundraisingPipeline:ErrorMessage:GenericError"]);
            }
        }

        [Authorize(FundraisingPipelinePermissions.PitchDeck.Update)]
        public async Task DeletePresentationAsync(Guid id, Guid presentationId)
        {
            try
            {
                var pitchDeck = await _pitchDeckRepository.GetWithDetailsAsync(id);
                if (pitchDeck == null)
                {
                    throw new EntityNotFoundException(typeof(Aggregates.PitchDeck), id);
                }

                // Domain entity needs a RemovePresentation method or direct manipulation
                var presentation = pitchDeck.Presentations.FirstOrDefault(p => p.Id == presentationId);
                if (presentation != null)
                {
                     var presentationsList = pitchDeck.Presentations as List<PitchPresentation>;
                     if(presentationsList != null) presentationsList.Remove(presentation);
                     else throw new AbpException("Internal error: Presentations collection is not a list.");
                    // Also remove associated feedbacks
                    var feedbacksToRemove = pitchDeck.Feedbacks.Where(f => f.PresentationId == presentationId).ToList();
                    var feedbacksList = pitchDeck.Feedbacks as List<FeedbackRecord>;
                    if(feedbacksList != null) feedbacksToRemove.ForEach(f => feedbacksList.Remove(f));
                     else throw new AbpException("Internal error: Feedbacks collection is not a list.");


                    await _pitchDeckRepository.UpdateAsync(pitchDeck, autoSave: true);
                }
                else
                {
                    Logger.LogWarning("Presentation {PresentationId} not found for pitch deck {PitchDeckId} during delete.", presentationId, id);
                }
            }
            catch (EntityNotFoundException)
            {
                throw new UserFriendlyException(L["FundraisingPipeline:ErrorMessage:PitchDeckNotFound", id]);
            }
            catch (AbpException ex)
            {
                Logger.LogError(ex, "Internal error removing presentation {PresentationId} for pitch deck {PitchDeckId}", presentationId, id);
                throw new UserFriendlyException(L["FundraisingPipeline:ErrorMessage:GenericError"]);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error deleting presentation {PresentationId} from pitch deck {PitchDeckId}", presentationId, id);
                throw new UserFriendlyException(L["FundraisingPipeline:ErrorMessage:GenericError"]);
            }
        }

        [Authorize(FundraisingPipelinePermissions.PitchDeck.View)]
        public async Task<List<PitchPresentationDto>> GetPresentationsAsync(Guid id)
        {
            try
            {
                var pitchDeck = await _pitchDeckRepository.GetWithDetailsAsync(id);
                 if (pitchDeck == null)
                {
                    throw new EntityNotFoundException(typeof(Aggregates.PitchDeck), id);
                }
                var presentationDtos = ObjectMapper.Map<List<PitchPresentation>, List<PitchPresentationDto>>(pitchDeck.Presentations.ToList());
                
                // Populate prospect and version names
                var prospectIds = presentationDtos.Select(p => p.ProspectId).Distinct().ToList();
                var versionIds = presentationDtos.Select(p => p.VersionId).Distinct().ToList();

                Dictionary<Guid, Aggregates.Prospect> prospectsMap = new Dictionary<Guid, Aggregates.Prospect>();
                if(prospectIds.Any())
                {
                    var prospects = await _prospectRepository.GetListAsync(p => prospectIds.Contains(p.Id));
                    prospectsMap = prospects.ToDictionary(p => p.Id);
                }
                
                var versionsMap = pitchDeck.Versions.Where(v => versionIds.Contains(v.Id)).ToDictionary(v => v.Id);

                foreach(var dto in presentationDtos)
                {
                    if(prospectsMap.TryGetValue(dto.ProspectId, out var prospect))
                    {
                        dto.ProspectName = prospect.Name;
                        dto.ProspectCompany = prospect.Company;
                    }
                    if(versionsMap.TryGetValue(dto.VersionId, out var version))
                    {
                        dto.VersionName = version.VersionName;
                    }
                }
                return presentationDtos;
            }
            catch (EntityNotFoundException)
            {
                throw new UserFriendlyException(L["FundraisingPipeline:ErrorMessage:PitchDeckNotFound", id]);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error retrieving presentations for pitch deck {PitchDeckId}", id);
                throw new UserFriendlyException(L["FundraisingPipeline:ErrorMessage:GenericError"]);
            }
        }


        [Authorize(FundraisingPipelinePermissions.PitchDeck.Update)]
        public async Task<FeedbackRecordDto> AddFeedbackAsync(Guid id, CreateFeedbackDto input)
        {
            try
            {
                var pitchDeck = await _pitchDeckRepository.GetWithDetailsAsync(id);
                if (pitchDeck == null)
                {
                    throw new EntityNotFoundException(typeof(Aggregates.PitchDeck), id);
                }

                var feedbackData = ObjectMapper.Map<CreateFeedbackDataDto, FeedbackData>(input.Feedback);
                var feedbackRecord = pitchDeck.AddFeedback(input.PresentationId, feedbackData, input.Notes);
                
                await _pitchDeckRepository.UpdateAsync(pitchDeck, autoSave: true);
                
                var feedbackDto = ObjectMapper.Map<FeedbackRecord, FeedbackRecordDto>(feedbackRecord);
                var presentation = pitchDeck.Presentations.FirstOrDefault(p => p.Id == input.PresentationId);
                if(presentation != null)
                {
                    var prospect = await _prospectRepository.FindAsync(presentation.ProspectId);
                    feedbackDto.ProspectName = prospect?.Name;
                }
                return feedbackDto;
            }
            catch (EntityNotFoundException)
            {
                throw new UserFriendlyException(L["FundraisingPipeline:ErrorMessage:PitchDeckNotFound", id]);
            }
            catch (BusinessException ex) // e.g. PresentationNotFound
            {
                throw new UserFriendlyException(ex.Message);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error adding feedback to pitch deck {PitchDeckId}", id);
                throw new UserFriendlyException(L["FundraisingPipeline:ErrorMessage:GenericError"]);
            }
        }

        [Authorize(FundraisingPipelinePermissions.PitchDeck.Update)]
        public async Task<FeedbackRecordDto> UpdateFeedbackAsync(Guid id, Guid feedbackId, UpdateFeedbackDto input)
        {
            try
            {
                var pitchDeck = await _pitchDeckRepository.GetWithDetailsAsync(id);
                if (pitchDeck == null)
                {
                    throw new EntityNotFoundException(typeof(Aggregates.PitchDeck), id);
                }

                var feedbackData = ObjectMapper.Map<UpdateFeedbackDataDto, FeedbackData>(input.Feedback);
                pitchDeck.UpdateFeedback(feedbackId, feedbackData, input.Notes);

                await _pitchDeckRepository.UpdateAsync(pitchDeck, autoSave: true);
                var updatedFeedback = pitchDeck.Feedbacks.First(f => f.Id == feedbackId);

                var feedbackDto = ObjectMapper.Map<FeedbackRecord, FeedbackRecordDto>(updatedFeedback);
                var presentation = pitchDeck.Presentations.FirstOrDefault(p => p.Id == updatedFeedback.PresentationId);
                 if(presentation != null)
                {
                    var prospect = await _prospectRepository.FindAsync(presentation.ProspectId);
                    feedbackDto.ProspectName = prospect?.Name;
                }
                return feedbackDto;
            }
            catch (EntityNotFoundException)
            {
                throw new UserFriendlyException(L["FundraisingPipeline:ErrorMessage:PitchDeckNotFound", id]);
            }
            catch (BusinessException ex) // e.g. FeedbackNotFound
            {
                throw new UserFriendlyException(ex.Message);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error updating feedback {FeedbackId} for pitch deck {PitchDeckId}", feedbackId, id);
                throw new UserFriendlyException(L["FundraisingPipeline:ErrorMessage:GenericError"]);
            }
        }

        [Authorize(FundraisingPipelinePermissions.PitchDeck.Update)]
        public async Task DeleteFeedbackAsync(Guid id, Guid feedbackId)
        {
            try
            {
                var pitchDeck = await _pitchDeckRepository.GetWithDetailsAsync(id);
                if (pitchDeck == null)
                {
                    throw new EntityNotFoundException(typeof(Aggregates.PitchDeck), id);
                }

                // Domain entity needs a RemoveFeedback method or direct manipulation
                var feedback = pitchDeck.Feedbacks.FirstOrDefault(f => f.Id == feedbackId);
                if (feedback != null)
                {
                    var feedbacksList = pitchDeck.Feedbacks as List<FeedbackRecord>;
                    if(feedbacksList != null) feedbacksList.Remove(feedback);
                    else throw new AbpException("Internal error: Feedbacks collection is not a list.");
                    await _pitchDeckRepository.UpdateAsync(pitchDeck, autoSave: true);
                }
                 else
                {
                    Logger.LogWarning("Feedback {FeedbackId} not found for pitch deck {PitchDeckId} during delete.", feedbackId, id);
                }
            }
            catch (EntityNotFoundException)
            {
                throw new UserFriendlyException(L["FundraisingPipeline:ErrorMessage:PitchDeckNotFound", id]);
            }
            catch (AbpException ex)
            {
                Logger.LogError(ex, "Internal error removing feedback {FeedbackId} for pitch deck {PitchDeckId}", feedbackId, id);
                throw new UserFriendlyException(L["FundraisingPipeline:ErrorMessage:GenericError"]);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error deleting feedback {FeedbackId} from pitch deck {PitchDeckId}", feedbackId, id);
                throw new UserFriendlyException(L["FundraisingPipeline:ErrorMessage:GenericError"]);
            }
        }

        [Authorize(FundraisingPipelinePermissions.PitchDeck.View)]
        public async Task<List<FeedbackRecordDto>> GetFeedbacksAsync(Guid id)
        {
            try
            {
                var pitchDeck = await _pitchDeckRepository.GetWithDetailsAsync(id);
                if (pitchDeck == null)
                {
                    throw new EntityNotFoundException(typeof(Aggregates.PitchDeck), id);
                }
                var feedbackDtos = ObjectMapper.Map<List<FeedbackRecord>, List<FeedbackRecordDto>>(pitchDeck.Feedbacks.ToList());

                var presentationIds = feedbackDtos.Select(f => f.PresentationId).Distinct().ToList();
                Dictionary<Guid, PitchPresentation> presentationsMap = new Dictionary<Guid, PitchPresentation>();
                if(presentationIds.Any())
                {
                    presentationsMap = pitchDeck.Presentations.Where(p => presentationIds.Contains(p.Id)).ToDictionary(p => p.Id);
                }

                var prospectIds = presentationsMap.Values.Select(p => p.ProspectId).Distinct().ToList();
                Dictionary<Guid, Aggregates.Prospect> prospectsMap = new Dictionary<Guid, Aggregates.Prospect>();
                 if(prospectIds.Any())
                {
                    var prospects = await _prospectRepository.GetListAsync(p => prospectIds.Contains(p.Id));
                    prospectsMap = prospects.ToDictionary(p => p.Id);
                }

                foreach(var dto in feedbackDtos)
                {
                    if(presentationsMap.TryGetValue(dto.PresentationId, out var presentation))
                    {
                        if(prospectsMap.TryGetValue(presentation.ProspectId, out var prospect))
                        {
                             dto.ProspectName = prospect.Name;
                        }
                    }
                }
                return feedbackDtos;
            }
            catch (EntityNotFoundException)
            {
                throw new UserFriendlyException(L["FundraisingPipeline:ErrorMessage:PitchDeckNotFound", id]);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error retrieving feedbacks for pitch deck {PitchDeckId}", id);
                throw new UserFriendlyException(L["FundraisingPipeline:ErrorMessage:GenericError"]);
            }
        }

        [Authorize(FundraisingPipelinePermissions.PitchDeck.View)] // Or a specific analysis permission
        public async Task<PitchEffectivenessDto> AnalyzePitchEffectivenessAsync(Guid id)
        {
            try
            {
                var analysisResult = await _pitchEffectivenessAnalyzer.AnalyzePitchEffectivenessAsync(id);
                return ObjectMapper.Map<PitchEffectivenessResult, PitchEffectivenessDto>(analysisResult);
            }
            catch (BusinessException ex) // e.g. PitchDeckNotFound from domain service
            {
                throw new UserFriendlyException(ex.Message);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error analyzing pitch effectiveness for pitch deck {PitchDeckId}", id);
                throw new UserFriendlyException(L["FundraisingPipeline:ErrorMessage:GenericError"]);
            }
        }
        
        private IQueryable<Aggregates.PitchDeck> ApplySorting(IQueryable<Aggregates.PitchDeck> query, PagedAndSortedResultRequestDto input)
        {
            if (string.IsNullOrWhiteSpace(input.Sorting))
            {
                return query.OrderByDescending(pd => pd.CreationTime);
            }
            try
            {
                query = query.OrderBy(input.Sorting);
            }
            catch (Exception ex)
            {
                Logger.LogWarning(ex, "Could not apply sorting '{Sorting}', using default.", input.Sorting);
                query = query.OrderByDescending(pd => pd.CreationTime);
            }
            return query;
        }
    }
}
