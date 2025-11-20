using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using NouAssist.StartupEntrepreneurship.FundraisingPipeline.Aggregates;
using NouAssist.StartupEntrepreneurship.FundraisingPipeline.Entities;
using NouAssist.StartupEntrepreneurship.FundraisingPipeline.Enums;
using NouAssist.StartupEntrepreneurship.FundraisingPipeline.Permissions;
using NouAssist.StartupEntrepreneurship.FundraisingPipeline.Repositories;
using NouAssist.StartupEntrepreneurship.FundraisingPipeline.Services; // For TermSheetComparisonResult, TermSheetRankingResult
using NouAssist.StartupEntrepreneurship.FundraisingPipeline.Services.Interfaces;
using NouAssist.StartupEntrepreneurship.FundraisingPipeline.TermSheet.Dtos;
using NouAssist.StartupEntrepreneurship.FundraisingPipeline.ValueObjects;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;
using Volo.Abp.ObjectMapping;

namespace NouAssist.StartupEntrepreneurship.FundraisingPipeline.TermSheet
{
    [Authorize(FundraisingPipelinePermissions.TermSheet.Default)]
    public class TermSheetAppService : ApplicationService, ITermSheetAppService
    {
        private readonly ITermSheetOfferRepository _termSheetRepository;
        private readonly IProspectRepository _prospectRepository;
        private readonly IFundraisingRoundRepository _fundraisingRoundRepository;
        private readonly ITermSheetClauseRepository _termSheetClauseRepository; // Assuming this exists for clause names
        private readonly ITermSheetComparisonService _termSheetComparisonService;
        private readonly IGuidGenerator _guidGenerator;

        public TermSheetAppService(
            ITermSheetOfferRepository termSheetRepository,
            IProspectRepository prospectRepository,
            IFundraisingRoundRepository fundraisingRoundRepository,
            ITermSheetClauseRepository termSheetClauseRepository, // Inject if needed for clause names
            ITermSheetComparisonService termSheetComparisonService,
            IGuidGenerator guidGenerator)
        {
            _termSheetRepository = termSheetRepository;
            _prospectRepository = prospectRepository;
            _fundraisingRoundRepository = fundraisingRoundRepository;
            _termSheetClauseRepository = termSheetClauseRepository;
            _termSheetComparisonService = termSheetComparisonService;
            _guidGenerator = guidGenerator;
        }

        [Authorize(FundraisingPipelinePermissions.TermSheet.View)]
        public async Task<TermSheetOfferDto> GetAsync(Guid id)
        {
            try
            {
                var termSheet = await _termSheetRepository.GetWithDetailsAsync(id);
                if (termSheet == null)
                {
                    throw new EntityNotFoundException(typeof(TermSheetOffer), id);
                }
                
                var dto = ObjectMapper.Map<TermSheetOffer, TermSheetOfferDto>(termSheet);
                await PopulateExtraDetailsForTermSheetDto(dto, termSheet);
                return dto;
            }
            catch (EntityNotFoundException)
            {
                throw new UserFriendlyException(L["FundraisingPipeline:ErrorMessage:TermSheetNotFound", id]);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error retrieving term sheet with ID {TermSheetId}", id);
                throw new UserFriendlyException(L["FundraisingPipeline:ErrorMessage:GenericError"]);
            }
        }

        [Authorize(FundraisingPipelinePermissions.TermSheet.View)]
        public async Task<PagedResultDto<TermSheetOfferDto>> GetListAsync(GetTermSheetListDto input)
        {
            try
            {
                var query = await _termSheetRepository.WithDetailsAsync();

                if (!string.IsNullOrWhiteSpace(input.Filter))
                {
                    var filterLower = input.Filter.ToLower();
                    query = query.Where(ts => ts.Title.ToLower().Contains(filterLower));
                }
                if (input.ProspectId.HasValue)
                {
                    query = query.Where(ts => ts.ProspectId == input.ProspectId.Value);
                }
                if (input.FundraisingRoundId.HasValue)
                {
                    query = query.Where(ts => ts.FundraisingRoundId == input.FundraisingRoundId.Value);
                }
                if (input.Status.HasValue)
                {
                    query = query.Where(ts => ts.Status == input.Status.Value);
                }
                if (input.UrgentOnly == true)
                {
                    var urgentSpec = new Specifications.UrgentTermSheetSpecification();
                    query = query.Where(urgentSpec.ToExpression());
                }
                if (input.FavorableOnly == true)
                {
                    var favorableSpec = new Specifications.FavorableTermSheetSpecification();
                    query = query.Where(favorableSpec.ToExpression());
                }

                query = ApplySorting(query, input);
                
                var totalCount = await AsyncExecuter.CountAsync(query);
                query = query.Skip(input.SkipCount).Take(input.MaxResultCount);
                
                var termSheets = await AsyncExecuter.ToListAsync(query);
                var dtos = new List<TermSheetOfferDto>();
                foreach(var ts in termSheets)
                {
                    var dto = ObjectMapper.Map<TermSheetOffer, TermSheetOfferDto>(ts);
                    await PopulateExtraDetailsForTermSheetDto(dto, ts);
                    dtos.Add(dto);
                }
                
                return new PagedResultDto<TermSheetOfferDto>(totalCount, dtos);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error retrieving term sheet list");
                throw new UserFriendlyException(L["FundraisingPipeline:ErrorMessage:GenericError"]);
            }
        }


        [Authorize(FundraisingPipelinePermissions.TermSheet.Create)]
        public async Task<TermSheetOfferDto> CreateAsync(CreateTermSheetOfferDto input)
        {
            try
            {
                var prospect = await _prospectRepository.FindAsync(input.ProspectId);
                if (prospect == null)
                {
                    throw new UserFriendlyException(L["FundraisingPipeline:ErrorMessage:ProspectNotFound", input.ProspectId]);
                }
                if (input.FundraisingRoundId.HasValue)
                {
                    var round = await _fundraisingRoundRepository.FindAsync(input.FundraisingRoundId.Value);
                    if (round == null)
                    {
                         throw new UserFriendlyException(L["FundraisingPipeline:ErrorMessage:FundraisingRoundNotFound", input.FundraisingRoundId.Value]);
                    }
                }

                var termData = ObjectMapper.Map<CreateTermComparisonDataDto, TermComparisonData>(input.TermData);
                var termSheet = TermSheetOffer.Create(
                    _guidGenerator.Create(),
                    input.Title,
                    input.ProspectId,
                    input.ReceivedDate,
                    termData,
                    input.ExpirationDate,
                    input.FundraisingRoundId,
                    CurrentTenant.Id
                );

                await _termSheetRepository.InsertAsync(termSheet, autoSave: true);
                
                // Update prospect status if applicable (e.g., to TermSheetReceived)
                if (prospect.PipelineStatus.Status < ProspectStatus.TermSheetReceived)
                {
                    prospect.UpdateStatus(ProspectStatus.TermSheetReceived, DateTime.Now); // Use Clock
                    await _prospectRepository.UpdateAsync(prospect, autoSave: true);
                    await ServiceProvider.GetRequiredService<IProspectScoringService>().UpdateAllScoresAsync(prospect.Id);
                }


                var dto = ObjectMapper.Map<TermSheetOffer, TermSheetOfferDto>(termSheet);
                await PopulateExtraDetailsForTermSheetDto(dto, termSheet);
                return dto;
            }
            catch (UserFriendlyException)
            {
                throw;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error creating term sheet");
                throw new UserFriendlyException(L["FundraisingPipeline:ErrorMessage:GenericError"]);
            }
        }

        [Authorize(FundraisingPipelinePermissions.TermSheet.Update)]
        public async Task<TermSheetOfferDto> UpdateAsync(Guid id, UpdateTermSheetOfferDto input)
        {
             try
            {
                var termSheet = await _termSheetRepository.GetAsync(id);
                 if (termSheet == null)
                {
                    throw new EntityNotFoundException(typeof(TermSheetOffer), id);
                }

                if (input.FundraisingRoundId.HasValue)
                {
                    var round = await _fundraisingRoundRepository.FindAsync(input.FundraisingRoundId.Value);
                    if (round == null)
                    {
                         throw new UserFriendlyException(L["FundraisingPipeline:ErrorMessage:FundraisingRoundNotFound", input.FundraisingRoundId.Value]);
                    }
                }

                var termData = ObjectMapper.Map<UpdateTermComparisonDataDto, TermComparisonData>(input.TermData);
                termSheet.UpdateBasicInfo(
                    input.Title,
                    input.ReceivedDate,
                    termData,
                    input.ExpirationDate,
                    input.FundraisingRoundId
                );

                await _termSheetRepository.UpdateAsync(termSheet, autoSave: true);
                var updatedTermSheet = await _termSheetRepository.GetWithDetailsAsync(id);
                var dto = ObjectMapper.Map<TermSheetOffer, TermSheetOfferDto>(updatedTermSheet);
                await PopulateExtraDetailsForTermSheetDto(dto, updatedTermSheet);
                return dto;
            }
            catch (EntityNotFoundException)
            {
                throw new UserFriendlyException(L["FundraisingPipeline:ErrorMessage:TermSheetNotFound", id]);
            }
            catch (UserFriendlyException)
            {
                throw;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error updating term sheet {TermSheetId}", id);
                throw new UserFriendlyException(L["FundraisingPipeline:ErrorMessage:GenericError"]);
            }
        }

        [Authorize(FundraisingPipelinePermissions.TermSheet.Delete)]
        public async Task DeleteAsync(Guid id)
        {
            try
            {
                await _termSheetRepository.DeleteAsync(id, autoSave: true);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error deleting term sheet {TermSheetId}", id);
                var ts = await _termSheetRepository.FindAsync(id);
                if(ts == null) 
                {
                    Logger.LogWarning("Attempted to delete a term sheet that was not found: {TermSheetId}", id);
                    return;
                }
                throw new UserFriendlyException(L["FundraisingPipeline:ErrorMessage:GenericError"]);
            }
        }

        [Authorize(FundraisingPipelinePermissions.TermSheet.Update)]
        public async Task<TermSheetOfferDto> UpdateStatusAsync(Guid id, UpdateTermSheetStatusDto input)
        {
            try
            {
                var termSheet = await _termSheetRepository.GetAsync(id);
                if (termSheet == null)
                {
                    throw new EntityNotFoundException(typeof(TermSheetOffer), id);
                }

                termSheet.UpdateStatus(input.Status);
                await _termSheetRepository.UpdateAsync(termSheet, autoSave: true);

                // Update prospect status if term sheet is accepted/rejected
                var prospect = await _prospectRepository.FindAsync(termSheet.ProspectId);
                if (prospect != null)
                {
                    bool prospectUpdated = false;
                    if (input.Status == TermSheetStatus.Accepted && prospect.PipelineStatus.Status < ProspectStatus.Committed)
                    {
                        prospect.UpdateStatus(ProspectStatus.Committed, DateTime.Now); // Use Clock
                        prospectUpdated = true;
                    }
                    else if (input.Status == TermSheetStatus.Rejected && prospect.PipelineStatus.Status < ProspectStatus.Declined)
                    {
                         // Only mark as declined if not already closed/committed via another path
                        if(prospect.PipelineStatus.Status != ProspectStatus.Closed && prospect.PipelineStatus.Status != ProspectStatus.Committed)
                        {
                            prospect.UpdateStatus(ProspectStatus.Declined, DateTime.Now); // Use Clock
                            prospectUpdated = true;
                        }
                    }
                    if(prospectUpdated)
                    {
                        await _prospectRepository.UpdateAsync(prospect, autoSave: true);
                        await ServiceProvider.GetRequiredService<IProspectScoringService>().UpdateAllScoresAsync(prospect.Id);
                    }
                }


                var updatedTermSheet = await _termSheetRepository.GetWithDetailsAsync(id);
                var dto = ObjectMapper.Map<TermSheetOffer, TermSheetOfferDto>(updatedTermSheet);
                await PopulateExtraDetailsForTermSheetDto(dto, updatedTermSheet);
                return dto;
            }
            catch (EntityNotFoundException)
            {
                throw new UserFriendlyException(L["FundraisingPipeline:ErrorMessage:TermSheetNotFound", id]);
            }
            catch (BusinessException ex) // e.g. InvalidTermSheetStatusTransition
            {
                throw new UserFriendlyException(ex.Message);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error updating status for term sheet {TermSheetId}", id);
                throw new UserFriendlyException(L["FundraisingPipeline:ErrorMessage:GenericError"]);
            }
        }


        [Authorize(FundraisingPipelinePermissions.TermSheet.Update)]
        public async Task<TermSheetClauseDto> AddClauseAsync(Guid id, CreateTermSheetClauseDto input)
        {
            try
            {
                var termSheet = await _termSheetRepository.GetWithDetailsAsync(id);
                if (termSheet == null)
                {
                    throw new EntityNotFoundException(typeof(TermSheetOffer), id);
                }

                var text = ObjectMapper.Map<CreateClauseTextDto, ClauseText>(input.Text);
                var clause = termSheet.AddClause(input.Name, text, input.Category, input.Order, input.Notes);
                
                await _termSheetRepository.UpdateAsync(termSheet, autoSave: true);
                return ObjectMapper.Map<TermSheetClause, TermSheetClauseDto>(clause);
            }
            catch (EntityNotFoundException)
            {
                throw new UserFriendlyException(L["FundraisingPipeline:ErrorMessage:TermSheetNotFound", id]);
            }
            catch (BusinessException ex) // e.g. DuplicateClauseName
            {
                throw new UserFriendlyException(ex.Message);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error adding clause to term sheet {TermSheetId}", id);
                throw new UserFriendlyException(L["FundraisingPipeline:ErrorMessage:GenericError"]);
            }
        }

        [Authorize(FundraisingPipelinePermissions.TermSheet.Update)]
        public async Task<TermSheetClauseDto> UpdateClauseAsync(Guid id, Guid clauseId, UpdateTermSheetClauseDto input)
        {
            try
            {
                var termSheet = await _termSheetRepository.GetWithDetailsAsync(id);
                if (termSheet == null)
                {
                    throw new EntityNotFoundException(typeof(TermSheetOffer), id);
                }
                
                var text = ObjectMapper.Map<UpdateClauseTextDto, ClauseText>(input.Text);
                termSheet.UpdateClause(clauseId, input.Name, text, input.Category, input.Order, input.Notes);

                await _termSheetRepository.UpdateAsync(termSheet, autoSave: true);
                var updatedClause = termSheet.Clauses.First(c => c.Id == clauseId);
                return ObjectMapper.Map<TermSheetClause, TermSheetClauseDto>(updatedClause);
            }
            catch (EntityNotFoundException)
            {
                throw new UserFriendlyException(L["FundraisingPipeline:ErrorMessage:TermSheetNotFound", id]);
            }
            catch (BusinessException ex) // e.g. ClauseNotFound, DuplicateClauseName
            {
                throw new UserFriendlyException(ex.Message);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error updating clause {ClauseId} for term sheet {TermSheetId}", clauseId, id);
                throw new UserFriendlyException(L["FundraisingPipeline:ErrorMessage:GenericError"]);
            }
        }

        [Authorize(FundraisingPipelinePermissions.TermSheet.Update)]
        public async Task DeleteClauseAsync(Guid id, Guid clauseId)
        {
            try
            {
                var termSheet = await _termSheetRepository.GetWithDetailsAsync(id);
                if (termSheet == null)
                {
                    throw new EntityNotFoundException(typeof(TermSheetOffer), id);
                }

                termSheet.RemoveClause(clauseId);
                await _termSheetRepository.UpdateAsync(termSheet, autoSave: true);
            }
            catch (EntityNotFoundException)
            {
                throw new UserFriendlyException(L["FundraisingPipeline:ErrorMessage:TermSheetNotFound", id]);
            }
            catch (BusinessException ex) // e.g. ClauseNotFound
            {
                Logger.LogWarning("Clause {ClauseId} not found for term sheet {TermSheetId} during delete. Message: {ErrorMessage}", clauseId, id, ex.Message);
                return; // Idempotent
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error deleting clause {ClauseId} from term sheet {TermSheetId}", clauseId, id);
                throw new UserFriendlyException(L["FundraisingPipeline:ErrorMessage:GenericError"]);
            }
        }

        [Authorize(FundraisingPipelinePermissions.TermSheet.View)]
        public async Task<List<TermSheetClauseDto>> GetClausesAsync(Guid id)
        {
            try
            {
                var termSheet = await _termSheetRepository.GetWithDetailsAsync(id);
                if (termSheet == null)
                {
                    throw new EntityNotFoundException(typeof(TermSheetOffer), id);
                }
                return ObjectMapper.Map<List<TermSheetClause>, List<TermSheetClauseDto>>(termSheet.Clauses.ToList());
            }
            catch (EntityNotFoundException)
            {
                throw new UserFriendlyException(L["FundraisingPipeline:ErrorMessage:TermSheetNotFound", id]);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error retrieving clauses for term sheet {TermSheetId}", id);
                throw new UserFriendlyException(L["FundraisingPipeline:ErrorMessage:GenericError"]);
            }
        }


        [Authorize(FundraisingPipelinePermissions.TermSheet.Update)]
        public async Task<NegotiationNoteDto> AddNegotiationNoteAsync(Guid id, CreateNegotiationNoteDto input)
        {
            try
            {
                var termSheet = await _termSheetRepository.GetWithDetailsAsync(id);
                if (termSheet == null)
                {
                    throw new EntityNotFoundException(typeof(TermSheetOffer), id);
                }

                var note = termSheet.AddNegotiationNote(input.Title, input.Content, input.RelatedClauseId);
                await _termSheetRepository.UpdateAsync(termSheet, autoSave: true);
                
                var dto = ObjectMapper.Map<NegotiationNote, NegotiationNoteDto>(note);
                if (note.RelatedClauseId.HasValue)
                {
                    var clause = termSheet.Clauses.FirstOrDefault(c => c.Id == note.RelatedClauseId.Value);
                    dto.RelatedClauseName = clause?.Name;
                }
                return dto;
            }
            catch (EntityNotFoundException)
            {
                throw new UserFriendlyException(L["FundraisingPipeline:ErrorMessage:TermSheetNotFound", id]);
            }
            catch (BusinessException ex) // e.g. ClauseNotFound
            {
                throw new UserFriendlyException(ex.Message);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error adding negotiation note to term sheet {TermSheetId}", id);
                throw new UserFriendlyException(L["FundraisingPipeline:ErrorMessage:GenericError"]);
            }
        }

        [Authorize(FundraisingPipelinePermissions.TermSheet.Update)]
        public async Task<NegotiationNoteDto> UpdateNegotiationNoteAsync(Guid id, Guid noteId, UpdateNegotiationNoteDto input)
        {
            try
            {
                var termSheet = await _termSheetRepository.GetWithDetailsAsync(id);
                if (termSheet == null)
                {
                    throw new EntityNotFoundException(typeof(TermSheetOffer), id);
                }

                termSheet.UpdateNegotiationNote(noteId, input.Title, input.Content, input.RelatedClauseId);
                await _termSheetRepository.UpdateAsync(termSheet, autoSave: true);
                
                var updatedNote = termSheet.NegotiationNotes.First(n => n.Id == noteId);
                var dto = ObjectMapper.Map<NegotiationNote, NegotiationNoteDto>(updatedNote);
                if (updatedNote.RelatedClauseId.HasValue)
                {
                    var clause = termSheet.Clauses.FirstOrDefault(c => c.Id == updatedNote.RelatedClauseId.Value);
                    dto.RelatedClauseName = clause?.Name;
                }
                return dto;
            }
            catch (EntityNotFoundException)
            {
                throw new UserFriendlyException(L["FundraisingPipeline:ErrorMessage:TermSheetNotFound", id]);
            }
            catch (BusinessException ex) // e.g. NegotiationNoteNotFound, ClauseNotFound
            {
                throw new UserFriendlyException(ex.Message);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error updating negotiation note {NoteId} for term sheet {TermSheetId}", noteId, id);
                throw new UserFriendlyException(L["FundraisingPipeline:ErrorMessage:GenericError"]);
            }
        }

        [Authorize(FundraisingPipelinePermissions.TermSheet.Update)]
        public async Task DeleteNegotiationNoteAsync(Guid id, Guid noteId)
        {
            try
            {
                var termSheet = await _termSheetRepository.GetWithDetailsAsync(id);
                if (termSheet == null)
                {
                    throw new EntityNotFoundException(typeof(TermSheetOffer), id);
                }
                
                termSheet.RemoveNegotiationNote(noteId);
                await _termSheetRepository.UpdateAsync(termSheet, autoSave: true);
            }
            catch (EntityNotFoundException)
            {
                throw new UserFriendlyException(L["FundraisingPipeline:ErrorMessage:TermSheetNotFound", id]);
            }
            catch (BusinessException ex) // e.g. NegotiationNoteNotFound
            {
                 Logger.LogWarning("Negotiation note {NoteId} not found for term sheet {TermSheetId} during delete. Message: {ErrorMessage}", noteId, id, ex.Message);
                return; // Idempotent
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error deleting negotiation note {NoteId} from term sheet {TermSheetId}", noteId, id);
                throw new UserFriendlyException(L["FundraisingPipeline:ErrorMessage:GenericError"]);
            }
        }

        [Authorize(FundraisingPipelinePermissions.TermSheet.View)]
        public async Task<List<NegotiationNoteDto>> GetNegotiationNotesAsync(Guid id)
        {
            try
            {
                var termSheet = await _termSheetRepository.GetWithDetailsAsync(id);
                if (termSheet == null)
                {
                    throw new EntityNotFoundException(typeof(TermSheetOffer), id);
                }
                var dtos = ObjectMapper.Map<List<NegotiationNote>, List<NegotiationNoteDto>>(termSheet.NegotiationNotes.ToList());
                
                var clauseIds = dtos.Where(n => n.RelatedClauseId.HasValue).Select(n => n.RelatedClauseId.Value).Distinct().ToList();
                Dictionary<Guid, string> clauseNames = new Dictionary<Guid, string>();
                if(clauseIds.Any())
                {
                    // This assumes you have a way to get clauses by IDs, or iterate through termSheet.Clauses
                     clauseNames = termSheet.Clauses
                        .Where(c => clauseIds.Contains(c.Id))
                        .ToDictionary(c => c.Id, c => c.Name);
                }

                foreach(var dto in dtos)
                {
                    if(dto.RelatedClauseId.HasValue && clauseNames.TryGetValue(dto.RelatedClauseId.Value, out var name))
                    {
                        dto.RelatedClauseName = name;
                    }
                }
                return dtos;
            }
            catch (EntityNotFoundException)
            {
                throw new UserFriendlyException(L["FundraisingPipeline:ErrorMessage:TermSheetNotFound", id]);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error retrieving negotiation notes for term sheet {TermSheetId}", id);
                throw new UserFriendlyException(L["FundraisingPipeline:ErrorMessage:GenericError"]);
            }
        }


        [Authorize(FundraisingPipelinePermissions.TermSheet.View)] // Or a specific analysis permission
        public async Task<TermSheetComparisonResultDto> CompareTermSheetsAsync(List<Guid> termSheetIds)
        {
            try
            {
                var result = await _termSheetComparisonService.CompareTermSheetsAsync(termSheetIds);
                var dto = ObjectMapper.Map<TermSheetComparisonResult, TermSheetComparisonResultDto>(result);

                // Populate prospect names in TermSheetSummaryDto
                foreach(var summaryDto in dto.TermSheetDetails.Values)
                {
                    var prospect = await _prospectRepository.FindAsync(summaryDto.ProspectId);
                    summaryDto.ProspectName = prospect?.Name;
                }
                return dto;
            }
            catch (ArgumentException ex) // From domain service if < 2 term sheets
            {
                throw new UserFriendlyException(ex.Message);
            }
            catch (BusinessException ex) // e.g. TermSheetNotFound from domain service
            {
                throw new UserFriendlyException(ex.Message);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error comparing term sheets");
                throw new UserFriendlyException(L["FundraisingPipeline:ErrorMessage:GenericError"]);
            }
        }

        [Authorize(FundraisingPipelinePermissions.TermSheet.View)] // Or a specific analysis permission
        public async Task<TermSheetRankingResultDto> RankTermSheetsAsync(List<Guid> termSheetIds)
        {
            try
            {
                var result = await _termSheetComparisonService.RankTermSheetsAsync(termSheetIds);
                var dto = ObjectMapper.Map<TermSheetRankingResult, TermSheetRankingResultDto>(result);

                // Populate prospect names in TermSheetSummaryDto within the comparison result
                 foreach(var summaryDto in dto.ComparisonResult.TermSheetDetails.Values)
                {
                    var prospect = await _prospectRepository.FindAsync(summaryDto.ProspectId);
                    summaryDto.ProspectName = prospect?.Name;
                }
                return dto;
            }
            catch (ArgumentException ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
            catch (BusinessException ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error ranking term sheets");
                throw new UserFriendlyException(L["FundraisingPipeline:ErrorMessage:GenericError"]);
            }
        }

        [Authorize(FundraisingPipelinePermissions.TermSheet.View)]
        public async Task<List<TermSheetOfferDto>> GetUrgentTermSheetsAsync()
        {
            try
            {
                var urgentSpec = new Specifications.UrgentTermSheetSpecification(); // Use default or configured values
                var termSheets = await _termSheetRepository.GetBySpecificationAsync(urgentSpec);
                
                var dtos = new List<TermSheetOfferDto>();
                foreach(var ts in termSheets)
                {
                    var dto = ObjectMapper.Map<TermSheetOffer, TermSheetOfferDto>(ts);
                    await PopulateExtraDetailsForTermSheetDto(dto, ts);
                    dtos.Add(dto);
                }
                return dtos;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error retrieving urgent term sheets");
                throw new UserFriendlyException(L["FundraisingPipeline:ErrorMessage:GenericError"]);
            }
        }

        [Authorize(FundraisingPipelinePermissions.TermSheet.View)]
        public async Task<List<TermSheetOfferDto>> GetFavorableTermSheetsAsync()
        {
            try
            {
                var favorableSpec = new Specifications.FavorableTermSheetSpecification(); // Use default or configured values
                var termSheets = await _termSheetRepository.GetBySpecificationAsync(favorableSpec);
                
                var dtos = new List<TermSheetOfferDto>();
                foreach(var ts in termSheets)
                {
                    var dto = ObjectMapper.Map<TermSheetOffer, TermSheetOfferDto>(ts);
                    await PopulateExtraDetailsForTermSheetDto(dto, ts);
                    dtos.Add(dto);
                }
                return dtos;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error retrieving favorable term sheets");
                throw new UserFriendlyException(L["FundraisingPipeline:ErrorMessage:GenericError"]);
            }
        }
        
        private async Task PopulateExtraDetailsForTermSheetDto(TermSheetOfferDto dto, TermSheetOffer entity)
        {
            var prospect = await _prospectRepository.FindAsync(entity.ProspectId);
            dto.ProspectName = prospect?.Name;
            dto.ProspectCompany = prospect?.Company;

            if (entity.FundraisingRoundId.HasValue)
            {
                var round = await _fundraisingRoundRepository.FindAsync(entity.FundraisingRoundId.Value);
                dto.FundraisingRoundName = round?.Name;
            }

            foreach(var noteDto in dto.NegotiationNotes)
            {
                if(noteDto.RelatedClauseId.HasValue)
                {
                    var clause = entity.Clauses.FirstOrDefault(c => c.Id == noteDto.RelatedClauseId.Value);
                    noteDto.RelatedClauseName = clause?.Name;
                }
            }
        }
        
        private IQueryable<TermSheetOffer> ApplySorting(IQueryable<TermSheetOffer> query, PagedAndSortedResultRequestDto input)
        {
            if (string.IsNullOrWhiteSpace(input.Sorting))
            {
                return query.OrderByDescending(ts => ts.ReceivedDate);
            }
            try
            {
                // Map DTO sort fields to entity sort fields
                var sortField = input.Sorting.Split(' ')[0].ToLowerInvariant();
                var sortDirection = input.Sorting.Split(' ').Length > 1 ? input.Sorting.Split(' ')[1] : "asc";

                string entitySortField = sortField switch
                {
                    "title" => "Title",
                    "status" => "Status",
                    "receiveddate" => "ReceivedDate",
                    "expirationdate" => "ExpirationDate",
                    "premoneyvaluation" => "TermData.PreMoneyValuation", // May need EF.Property for owned types
                    "investmentamount" => "TermData.InvestmentAmount", // May need EF.Property for owned types
                    _ => "ReceivedDate" // Default
                };
                
                // Special handling for owned type properties if DynamicLinq struggles
                if (sortField == "premoneyvaluation")
                {
                    query = sortDirection.Equals("asc", StringComparison.OrdinalIgnoreCase)
                        ? query.OrderBy(p => EF.Property<decimal>(p.TermData, "PreMoneyValuation"))
                        : query.OrderByDescending(p => EF.Property<decimal>(p.TermData, "PreMoneyValuation"));
                }
                else if (sortField == "investmentamount")
                {
                     query = sortDirection.Equals("asc", StringComparison.OrdinalIgnoreCase)
                        ? query.OrderBy(p => EF.Property<decimal>(p.TermData, "InvestmentAmount"))
                        : query.OrderByDescending(p => EF.Property<decimal>(p.TermData, "InvestmentAmount"));
                }
                else
                {
                    query = query.OrderBy($"{entitySortField} {sortDirection}");
                }
            }
            catch (Exception ex)
            {
                Logger.LogWarning(ex, "Could not apply sorting '{Sorting}', using default.", input.Sorting);
                query = query.OrderByDescending(ts => ts.ReceivedDate);
            }
            return query;
        }
    }
}
