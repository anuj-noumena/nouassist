using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NouAssist.StartupEntrepreneurship.FundraisingPipeline.Aggregates;
using NouAssist.StartupEntrepreneurship.FundraisingPipeline.Entities;
using NouAssist.StartupEntrepreneurship.FundraisingPipeline.Enums;
using NouAssist.StartupEntrepreneurship.FundraisingPipeline.Exceptions;
using NouAssist.StartupEntrepreneurship.FundraisingPipeline.FundraisingRound.Dtos;
using NouAssist.StartupEntrepreneurship.FundraisingPipeline.Permissions;
using NouAssist.StartupEntrepreneurship.FundraisingPipeline.Prospect.Dtos; // For ProspectDto
using NouAssist.StartupEntrepreneurship.FundraisingPipeline.Repositories;
using NouAssist.StartupEntrepreneurship.FundraisingPipeline.Specifications;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;
using Volo.Abp.ObjectMapping;
using Volo.Abp.Timing;

namespace NouAssist.StartupEntrepreneurship.FundraisingPipeline.FundraisingRound
{
    [Authorize(FundraisingPipelinePermissions.FundraisingRound.Default)]
    public class FundraisingRoundAppService : ApplicationService, IFundraisingRoundAppService
    {
        private readonly IFundraisingRoundRepository _fundraisingRoundRepository;
        private readonly IProspectRepository _prospectRepository;
        private readonly IGuidGenerator _guidGenerator;
        private readonly IClock _clock;

        public FundraisingRoundAppService(
            IFundraisingRoundRepository fundraisingRoundRepository,
            IProspectRepository prospectRepository,
            IGuidGenerator guidGenerator,
            IClock clock)
        {
            _fundraisingRoundRepository = fundraisingRoundRepository;
            _prospectRepository = prospectRepository;
            _guidGenerator = guidGenerator;
            _clock = clock;
        }

        [Authorize(FundraisingPipelinePermissions.FundraisingRound.View)]
        public async Task<FundraisingRoundDto> GetAsync(Guid id)
        {
            try
            {
                var round = await _fundraisingRoundRepository.GetWithDetailsAsync(id);
                if (round == null)
                {
                    throw new EntityNotFoundException(typeof(Aggregates.FundraisingRound), id);
                }
                var dto = ObjectMapper.Map<Aggregates.FundraisingRound, FundraisingRoundDto>(round);
                dto.ProspectCount = await _prospectRepository.GetCountAsync(p => p.FundraisingRoundId == id);
                
                // Populate prospect count for each stage
                foreach(var stageDto in dto.PipelineStages)
                {
                    // This is a simplification. In a real scenario, prospect status would map to pipeline stages.
                    // For now, we'll assume a direct count or leave it, as prospect status isn't directly linked to these custom stages.
                    // stageDto.ProspectCount = await _prospectRepository.GetCountAsync(p => p.FundraisingRoundId == id && p.PipelineStatus.Status == ...);
                    stageDto.ProspectCount = 0; // Placeholder
                }
                return dto;
            }
            catch (EntityNotFoundException)
            {
                throw new UserFriendlyException(L["FundraisingPipeline:ErrorMessage:FundraisingRoundNotFound", id]);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error retrieving fundraising round with ID {RoundId}", id);
                throw new UserFriendlyException(L["FundraisingPipeline:ErrorMessage:GenericError"]);
            }
        }

        [Authorize(FundraisingPipelinePermissions.FundraisingRound.View)]
        public async Task<PagedResultDto<FundraisingRoundDto>> GetListAsync(GetFundraisingRoundListDto input)
        {
            try
            {
                var query = await _fundraisingRoundRepository.WithDetailsAsync();

                if (!string.IsNullOrWhiteSpace(input.Filter))
                {
                    var filterLower = input.Filter.ToLower();
                    query = query.Where(r => 
                        r.Name.ToLower().Contains(filterLower) || 
                        (r.Description != null && r.Description.ToLower().Contains(filterLower)));
                }

                if (input.Type.HasValue)
                {
                    query = query.Where(r => r.Type == input.Type.Value);
                }

                if (input.Stage.HasValue)
                {
                    query = query.Where(r => r.Stage == input.Stage.Value);
                }
                
                if (input.ActiveOnly == true)
                {
                    query = query.Where(r => r.Stage == RoundStage.Active);
                }

                if (input.AtRiskOnly == true)
                {
                    var atRiskSpec = new AtRiskFundraisingRoundSpecification();
                    query = query.Where(atRiskSpec.ToExpression());
                }

                query = ApplySorting(query, input);
                
                var totalCount = await AsyncExecuter.CountAsync(query);
                query = query.Skip(input.SkipCount).Take(input.MaxResultCount);
                
                var rounds = await AsyncExecuter.ToListAsync(query);
                var dtos = ObjectMapper.Map<List<Aggregates.FundraisingRound>, List<FundraisingRoundDto>>(rounds);

                foreach (var dto in dtos)
                {
                    dto.ProspectCount = await _prospectRepository.GetCountAsync(p => p.FundraisingRoundId == dto.Id);
                }
                
                return new PagedResultDto<FundraisingRoundDto>(totalCount, dtos);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error retrieving fundraising round list");
                throw new UserFriendlyException(L["FundraisingPipeline:ErrorMessage:GenericError"]);
            }
        }

        [Authorize(FundraisingPipelinePermissions.FundraisingRound.Create)]
        public async Task<FundraisingRoundDto> CreateAsync(CreateFundraisingRoundDto input)
        {
            try
            {
                var round = Aggregates.FundraisingRound.Create(
                    _guidGenerator.Create(),
                    input.Name,
                    input.Type,
                    input.TargetAmount,
                    input.StartDate,
                    input.TargetCloseDate,
                    input.Description,
                    CurrentTenant.Id
                );

                await _fundraisingRoundRepository.InsertAsync(round, autoSave: true);
                return ObjectMapper.Map<Aggregates.FundraisingRound, FundraisingRoundDto>(round);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error creating fundraising round");
                throw new UserFriendlyException(L["FundraisingPipeline:ErrorMessage:GenericError"]);
            }
        }

        [Authorize(FundraisingPipelinePermissions.FundraisingRound.Update)]
        public async Task<FundraisingRoundDto> UpdateAsync(Guid id, UpdateFundraisingRoundDto input)
        {
            try
            {
                var round = await _fundraisingRoundRepository.GetAsync(id);
                if (round == null)
                {
                    throw new EntityNotFoundException(typeof(Aggregates.FundraisingRound), id);
                }

                round.UpdateBasicInfo(
                    input.Name,
                    input.Type,
                    input.TargetAmount,
                    input.StartDate,
                    input.TargetCloseDate,
                    input.Description
                );

                await _fundraisingRoundRepository.UpdateAsync(round, autoSave: true);
                var dto = ObjectMapper.Map<Aggregates.FundraisingRound, FundraisingRoundDto>(round);
                dto.ProspectCount = await _prospectRepository.GetCountAsync(p => p.FundraisingRoundId == id);
                return dto;
            }
            catch (EntityNotFoundException)
            {
                throw new UserFriendlyException(L["FundraisingPipeline:ErrorMessage:FundraisingRoundNotFound", id]);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error updating fundraising round {RoundId}", id);
                throw new UserFriendlyException(L["FundraisingPipeline:ErrorMessage:GenericError"]);
            }
        }

        [Authorize(FundraisingPipelinePermissions.FundraisingRound.Delete)]
        public async Task DeleteAsync(Guid id)
        {
            try
            {
                // Check if any prospects are assigned to this round
                var prospectCount = await _prospectRepository.GetCountAsync(p => p.FundraisingRoundId == id);
                if (prospectCount > 0)
                {
                    throw new UserFriendlyException(L["FundraisingPipeline:ErrorMessage:CannotDeleteRoundWithProspects"]);
                }

                await _fundraisingRoundRepository.DeleteAsync(id, autoSave: true);
            }
            catch (UserFriendlyException)
            {
                throw;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error deleting fundraising round {RoundId}", id);
                // It's possible the round was already deleted or an FK constraint failed if not checked above
                var round = await _fundraisingRoundRepository.FindAsync(id);
                if (round == null) // Already deleted
                {
                     Logger.LogWarning("Attempted to delete a fundraising round that was not found: {RoundId}", id);
                     return;
                }
                throw new UserFriendlyException(L["FundraisingPipeline:ErrorMessage:GenericError"]);
            }
        }

        [Authorize(FundraisingPipelinePermissions.FundraisingRound.Update)]
        public async Task<FundraisingRoundDto> UpdateStageAsync(Guid id, UpdateRoundStageDto input)
        {
            try
            {
                var round = await _fundraisingRoundRepository.GetAsync(id);
                 if (round == null)
                {
                    throw new EntityNotFoundException(typeof(Aggregates.FundraisingRound), id);
                }

                round.ChangeStage(input.Stage);
                await _fundraisingRoundRepository.UpdateAsync(round, autoSave: true);
                
                var dto = ObjectMapper.Map<Aggregates.FundraisingRound, FundraisingRoundDto>(round);
                dto.ProspectCount = await _prospectRepository.GetCountAsync(p => p.FundraisingRoundId == id);
                return dto;
            }
            catch (EntityNotFoundException)
            {
                throw new UserFriendlyException(L["FundraisingPipeline:ErrorMessage:FundraisingRoundNotFound", id]);
            }
            catch (InvalidRoundStageTransitionException ex)
            {
                throw new UserFriendlyException(L["FundraisingPipeline:ErrorMessage:InvalidRoundStageTransition", ex.Data["currentStage"], ex.Data["newStage"]]);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error updating stage for fundraising round {RoundId}", id);
                throw new UserFriendlyException(L["FundraisingPipeline:ErrorMessage:GenericError"]);
            }
        }

        [Authorize(FundraisingPipelinePermissions.FundraisingRound.Update)]
        public async Task<FundraisingRoundDto> UpdateCommittedAmountAsync(Guid id, UpdateCommittedAmountDto input)
        {
            try
            {
                var round = await _fundraisingRoundRepository.GetAsync(id);
                 if (round == null)
                {
                    throw new EntityNotFoundException(typeof(Aggregates.FundraisingRound), id);
                }

                round.UpdateCommittedAmount(input.CommittedAmount);
                await _fundraisingRoundRepository.UpdateAsync(round, autoSave: true);
                
                var dto = ObjectMapper.Map<Aggregates.FundraisingRound, FundraisingRoundDto>(round);
                dto.ProspectCount = await _prospectRepository.GetCountAsync(p => p.FundraisingRoundId == id);
                return dto;
            }
            catch (EntityNotFoundException)
            {
                throw new UserFriendlyException(L["FundraisingPipeline:ErrorMessage:FundraisingRoundNotFound", id]);
            }
            catch (BusinessException ex) // Domain validation for amount
            {
                 throw new UserFriendlyException(ex.Message);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error updating committed amount for fundraising round {RoundId}", id);
                throw new UserFriendlyException(L["FundraisingPipeline:ErrorMessage:GenericError"]);
            }
        }

        [Authorize(FundraisingPipelinePermissions.FundraisingRound.Update)]
        public async Task<PipelineStageDto> AddPipelineStageAsync(Guid id, CreatePipelineStageDto input)
        {
            try
            {
                var round = await _fundraisingRoundRepository.GetWithDetailsAsync(id);
                 if (round == null)
                {
                    throw new EntityNotFoundException(typeof(Aggregates.FundraisingRound), id);
                }
                
                var stage = round.AddPipelineStage(input.Name, input.Order, input.Description);
                await _fundraisingRoundRepository.UpdateAsync(round, autoSave: true);
                return ObjectMapper.Map<PipelineStage, PipelineStageDto>(stage);
            }
            catch (EntityNotFoundException)
            {
                throw new UserFriendlyException(L["FundraisingPipeline:ErrorMessage:FundraisingRoundNotFound", id]);
            }
            catch (BusinessException ex) // e.g. DuplicatePipelineStageName
            {
                throw new UserFriendlyException(ex.Message);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error adding pipeline stage to fundraising round {RoundId}", id);
                throw new UserFriendlyException(L["FundraisingPipeline:ErrorMessage:GenericError"]);
            }
        }

        [Authorize(FundraisingPipelinePermissions.FundraisingRound.Update)]
        public async Task<PipelineStageDto> UpdatePipelineStageAsync(Guid id, Guid stageId, UpdatePipelineStageDto input)
        {
             try
            {
                var round = await _fundraisingRoundRepository.GetWithDetailsAsync(id);
                 if (round == null)
                {
                    throw new EntityNotFoundException(typeof(Aggregates.FundraisingRound), id);
                }

                round.UpdatePipelineStage(stageId, input.Name, input.Order, input.Description);
                await _fundraisingRoundRepository.UpdateAsync(round, autoSave: true);
                
                var updatedStage = round.PipelineStages.First(s => s.Id == stageId);
                return ObjectMapper.Map<PipelineStage, PipelineStageDto>(updatedStage);
            }
            catch (EntityNotFoundException)
            {
                throw new UserFriendlyException(L["FundraisingPipeline:ErrorMessage:FundraisingRoundNotFound", id]);
            }
            catch (BusinessException ex) // e.g. PipelineStageNotFound, DuplicatePipelineStageName
            {
                throw new UserFriendlyException(ex.Message);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error updating pipeline stage {StageId} for fundraising round {RoundId}", stageId, id);
                throw new UserFriendlyException(L["FundraisingPipeline:ErrorMessage:GenericError"]);
            }
        }

        [Authorize(FundraisingPipelinePermissions.FundraisingRound.Update)]
        public async Task DeletePipelineStageAsync(Guid id, Guid stageId)
        {
            try
            {
                var round = await _fundraisingRoundRepository.GetWithDetailsAsync(id);
                 if (round == null)
                {
                    throw new EntityNotFoundException(typeof(Aggregates.FundraisingRound), id);
                }

                round.RemovePipelineStage(stageId);
                await _fundraisingRoundRepository.UpdateAsync(round, autoSave: true);
            }
            catch (EntityNotFoundException)
            {
                throw new UserFriendlyException(L["FundraisingPipeline:ErrorMessage:FundraisingRoundNotFound", id]);
            }
            catch (BusinessException ex) // e.g. PipelineStageNotFound
            {
                // If stage not found, it's already "deleted" in a sense.
                Logger.LogWarning("Pipeline stage {StageId} not found for round {RoundId} during delete. Message: {ErrorMessage}", stageId, id, ex.Message);
                return;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error deleting pipeline stage {StageId} from fundraising round {RoundId}", stageId, id);
                throw new UserFriendlyException(L["FundraisingPipeline:ErrorMessage:GenericError"]);
            }
        }

        [Authorize(FundraisingPipelinePermissions.FundraisingRound.View)]
        public async Task<List<PipelineStageDto>> GetPipelineStagesAsync(Guid id)
        {
            try
            {
                var round = await _fundraisingRoundRepository.GetWithDetailsAsync(id);
                if (round == null)
                {
                    throw new EntityNotFoundException(typeof(Aggregates.FundraisingRound), id);
                }
                return ObjectMapper.Map<List<PipelineStage>, List<PipelineStageDto>>(round.PipelineStages.ToList());
            }
            catch (EntityNotFoundException)
            {
                throw new UserFriendlyException(L["FundraisingPipeline:ErrorMessage:FundraisingRoundNotFound", id]);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error retrieving pipeline stages for fundraising round {RoundId}", id);
                throw new UserFriendlyException(L["FundraisingPipeline:ErrorMessage:GenericError"]);
            }
        }

        [Authorize(FundraisingPipelinePermissions.FundraisingRound.View)] // Or Prospect.View
        public async Task<List<ProspectDto>> GetProspectsAsync(Guid id)
        {
            try
            {
                var roundExists = await _fundraisingRoundRepository.FindAsync(id);
                 if (roundExists == null)
                {
                    throw new EntityNotFoundException(typeof(Aggregates.FundraisingRound), id);
                }
                
                var prospects = await _prospectRepository.GetByFundraisingRoundAsync(id);
                var dtos = ObjectMapper.Map<List<Aggregates.Prospect>, List<ProspectDto>>(prospects);
                // No need to populate round names here as they all belong to the same round.
                // But if we wanted to be super explicit:
                foreach(var dto in dtos) { dto.FundraisingRoundName = roundExists.Name; }
                return dtos;
            }
            catch (EntityNotFoundException)
            {
                throw new UserFriendlyException(L["FundraisingPipeline:ErrorMessage:FundraisingRoundNotFound", id]);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error retrieving prospects for fundraising round {RoundId}", id);
                throw new UserFriendlyException(L["FundraisingPipeline:ErrorMessage:GenericError"]);
            }
        }

        [Authorize(FundraisingPipelinePermissions.FundraisingRound.View)]
        public async Task<List<FundraisingRoundDto>> GetActiveRoundsAsync()
        {
            try
            {
                var rounds = await _fundraisingRoundRepository.GetActiveRoundsAsync();
                var dtos = ObjectMapper.Map<List<Aggregates.FundraisingRound>, List<FundraisingRoundDto>>(rounds);
                foreach (var dto in dtos)
                {
                    dto.ProspectCount = await _prospectRepository.GetCountAsync(p => p.FundraisingRoundId == dto.Id);
                }
                return dtos;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error retrieving active fundraising rounds");
                throw new UserFriendlyException(L["FundraisingPipeline:ErrorMessage:GenericError"]);
            }
        }

        [Authorize(FundraisingPipelinePermissions.FundraisingRound.View)]
        public async Task<List<FundraisingRoundDto>> GetAtRiskRoundsAsync()
        {
            try
            {
                var atRiskSpec = new AtRiskFundraisingRoundSpecification(); // Use default or configured values
                var rounds = await _fundraisingRoundRepository.GetBySpecificationAsync(atRiskSpec);
                var dtos = ObjectMapper.Map<List<Aggregates.FundraisingRound>, List<FundraisingRoundDto>>(rounds);
                 foreach (var dto in dtos)
                {
                    dto.ProspectCount = await _prospectRepository.GetCountAsync(p => p.FundraisingRoundId == dto.Id);
                }
                return dtos;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error retrieving at-risk fundraising rounds");
                throw new UserFriendlyException(L["FundraisingPipeline:ErrorMessage:GenericError"]);
            }
        }
        
        private IQueryable<Aggregates.FundraisingRound> ApplySorting(IQueryable<Aggregates.FundraisingRound> query, PagedAndSortedResultRequestDto input)
        {
            if (string.IsNullOrWhiteSpace(input.Sorting))
            {
                return query.OrderByDescending(r => r.StartDate);
            }
            try
            {
                 // Map DTO sort fields to entity sort fields
                var sortField = input.Sorting.Split(' ')[0].ToLowerInvariant();
                var sortDirection = input.Sorting.Split(' ').Length > 1 ? input.Sorting.Split(' ')[1] : "asc";

                string entitySortField = sortField switch
                {
                    "name" => "Name",
                    "type" => "Type",
                    "stage" => "Stage",
                    "targetamount" => "TargetAmount",
                    "committedamount" => "CommittedAmount",
                    "startdate" => "StartDate",
                    "targetclosedate" => "TargetCloseDate",
                    _ => "StartDate" // Default
                };
                query = query.OrderBy($"{entitySortField} {sortDirection}");
            }
            catch (Exception ex)
            {
                Logger.LogWarning(ex, "Could not apply sorting '{Sorting}', using default.", input.Sorting);
                query = query.OrderByDescending(r => r.StartDate);
            }
            return query;
        }
    }
}
