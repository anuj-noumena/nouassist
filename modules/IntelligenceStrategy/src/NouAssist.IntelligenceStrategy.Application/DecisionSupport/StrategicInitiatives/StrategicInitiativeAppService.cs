using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using NouAssist.IntelligenceStrategy.DecisionSupport.Aggregates;
using NouAssist.IntelligenceStrategy.DecisionSupport.Enums;
using NouAssist.IntelligenceStrategy.DecisionSupport.Exceptions;
using NouAssist.IntelligenceStrategy.DecisionSupport.Interfaces.Services;
using NouAssist.IntelligenceStrategy.DecisionSupport.Localization;
using NouAssist.IntelligenceStrategy.DecisionSupport.Permissions;
using NouAssist.IntelligenceStrategy.DecisionSupport.Repositories;
using NouAssist.IntelligenceStrategy.DecisionSupport.StrategicInitiatives.Dtos;
using NouAssist.IntelligenceStrategy.DecisionSupport.ValueObjects;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.IntelligenceStrategy.DecisionSupport.StrategicInitiatives
{
    /// <summary>
    /// Application service for managing strategic initiatives
    /// </summary>
    [Authorize(DecisionSupportPermissions.StrategicInitiative.Default)]
    public class StrategicInitiativeAppService : ApplicationService, IStrategicInitiativeAppService
    {
        private readonly IStrategicInitiativeRepository _initiativeRepository;
        private readonly IStrategicInitiativeManager _initiativeManager;
        private readonly IRepository<DecisionModelInstance, Guid> _decisionModelRepository;

        /// <summary>
        /// Creates a new instance of <see cref="StrategicInitiativeAppService"/>
        /// </summary>
        public StrategicInitiativeAppService(
            IStrategicInitiativeRepository initiativeRepository,
            IStrategicInitiativeManager initiativeManager,
            IRepository<DecisionModelInstance, Guid> decisionModelRepository)
        {
            _initiativeRepository = initiativeRepository;
            _initiativeManager = initiativeManager;
            _decisionModelRepository = decisionModelRepository;
            
            LocalizationResource = typeof(DecisionSupportResource);
        }

        /// <inheritdoc/>
        [Authorize(DecisionSupportPermissions.StrategicInitiative.View)]
        public virtual async Task<StrategicInitiativeDto> GetAsync(Guid id)
        {
            try
            {
                var initiative = await _initiativeRepository.GetWithAnalysesAsync(id);
                return MapInitiativeToDto(initiative);
            }
            catch (EntityNotFoundException ex)
            {
                Logger.LogWarning(ex, "Strategic initiative with ID {Id} not found", id);
                throw new UserFriendlyException(L["StrategicInitiative:ErrorMessage:NotFound"]);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        /// <inheritdoc/>
        [Authorize(DecisionSupportPermissions.StrategicInitiative.View)]
        public virtual async Task<PagedResultDto<StrategicInitiativeListDto>> GetListAsync(GetStrategicInitiativeListDto input)
        {
            try
            {
                var query = await _initiativeRepository.GetQueryableAsync();
                
                // Apply filters
                if (!string.IsNullOrWhiteSpace(input.Filter))
                {
                    query = query.Where(i => 
                        i.Title.Contains(input.Filter) || 
                        i.Description.Contains(input.Filter));
                }
                
                if (input.Status.HasValue)
                {
                    query = query.Where(i => i.Status == input.Status.Value);
                }
                
                if (input.Alignment.HasValue)
                {
                    query = query.Where(i => i.StrategicAlignment == input.Alignment.Value);
                }
                
                if (input.DecisionModelId.HasValue)
                {
                    query = query.Where(i => i.RelatedDecisionModelId == input.DecisionModelId.Value);
                }
                
                // Get total count
                var totalCount = await AsyncExecuter.CountAsync(query);
                
                // Apply sorting and paging
                var sorting = input.Sorting ?? "creationTime desc";
                var initiatives = await AsyncExecuter.ToListAsync(
                    query.OrderBy(sorting)
                        .PageBy(input.SkipCount, input.MaxResultCount));
                
                // Map to DTOs
                var dtos = initiatives.Select(i => new StrategicInitiativeListDto
                {
                    Id = i.Id,
                    Title = i.Title,
                    Status = i.Status,
                    StatusName = L[$"ProposalStatus:{i.Status}"],
                    StrategicAlignment = i.StrategicAlignment,
                    AlignmentName = L[$"AlignmentStatus:{i.StrategicAlignment}"],
                    ProposedDate = i.ProposedDate,
                    DecisionDate = i.DecisionDate,
                    ProjectId = i.ProjectId?.Value,
                    CreationTime = i.CreationTime,
                    CreatorId = i.CreatorId,
                    LastModificationTime = i.LastModificationTime,
                    LastModifierId = i.LastModifierId
                }).ToList();
                
                return new PagedResultDto<StrategicInitiativeListDto>(totalCount, dtos);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        /// <inheritdoc/>
        [Authorize(DecisionSupportPermissions.StrategicInitiative.Create)]
        public virtual async Task<StrategicInitiativeDto> CreateAsync(CreateStrategicInitiativeDto input)
        {
            try
            {
                // Use domain service to create the initiative
                var initiative = await _initiativeManager.CreateProposalAsync(
                    input.Title,
                    input.Description,
                    new DecisionRationaleText(input.Rationale),
                    new ExpectedOutcomeText(input.ExpectedOutcomes),
                    input.RelatedDecisionModelId,
                    input.StrategicAlignment);
                
                return MapInitiativeToDto(initiative);
            }
            catch (DecisionSupportDomainException ex)
            {
                await HandleErrorAsync(ex, LogLevel.Warning);
                throw new UserFriendlyException(L[ex.Code ?? "DefaultErrorMessage", ex.Message]);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        /// <inheritdoc/>
        [Authorize(DecisionSupportPermissions.StrategicInitiative.Update)]
        public virtual async Task<StrategicInitiativeDto> UpdateAsync(Guid id, UpdateStrategicInitiativeDto input)
        {
            try
            {
                var initiative = await _initiativeRepository.GetAsync(id);
                
                // Update via domain entity methods
                initiative.Update(
                    input.Title,
                    input.Description,
                    new DecisionRationaleText(input.Rationale),
                    new ExpectedOutcomeText(input.ExpectedOutcomes),
                    input.StrategicAlignment,
                    input.RelatedDecisionModelId);
                
                await _initiativeRepository.UpdateAsync(initiative);
                
                return MapInitiativeToDto(initiative);
            }
            catch (DecisionSupportDomainException ex)
            {
                await HandleErrorAsync(ex, LogLevel.Warning);
                throw new UserFriendlyException(L[ex.Code ?? "DefaultErrorMessage", ex.Message]);
            }
            catch (EntityNotFoundException ex)
            {
                Logger.LogWarning(ex, "Strategic initiative with ID {Id} not found for update", id);
                throw new UserFriendlyException(L["StrategicInitiative:ErrorMessage:NotFound"]);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        /// <inheritdoc/>
        [Authorize(DecisionSupportPermissions.StrategicInitiative.Delete)]
        public virtual async Task DeleteAsync(Guid id)
        {
            try
            {
                await _initiativeRepository.DeleteAsync(id);
            }
            catch (EntityNotFoundException ex)
            {
                Logger.LogWarning(ex, "Strategic initiative with ID {Id} not found for deletion", id);
                throw new UserFriendlyException(L["StrategicInitiative:ErrorMessage:NotFound"]);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        /// <inheritdoc/>
        [Authorize(DecisionSupportPermissions.StrategicInitiative.Approve)]
        public virtual async Task<StrategicInitiativeDto> ApproveAsync(Guid id, ApproveInitiativeDto input)
        {
            try
            {
                var initiative = await _initiativeManager.ApproveProposalAsync(id, input.ProjectId);
                return MapInitiativeToDto(initiative);
            }
            catch (DecisionSupportDomainException ex)
            {
                await HandleErrorAsync(ex, LogLevel.Warning);
                throw new UserFriendlyException(L[ex.Code ?? "DefaultErrorMessage", ex.Message]);
            }
            catch (InitiativeNotFoundException ex)
            {
                Logger.LogWarning(ex, "Strategic initiative with ID {Id} not found for approval", id);
                throw new UserFriendlyException(L["StrategicInitiative:ErrorMessage:NotFound"]);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        /// <inheritdoc/>
        [Authorize(DecisionSupportPermissions.StrategicInitiative.Reject)]
        public virtual async Task<StrategicInitiativeDto> RejectAsync(Guid id, RejectInitiativeDto input)
        {
            try
            {
                var initiative = await _initiativeManager.RejectProposalAsync(id, input.RejectionReason);
                return MapInitiativeToDto(initiative);
            }
            catch (DecisionSupportDomainException ex)
            {
                await HandleErrorAsync(ex, LogLevel.Warning);
                throw new UserFriendlyException(L[ex.Code ?? "DefaultErrorMessage", ex.Message]);
            }
            catch (InitiativeNotFoundException ex)
            {
                Logger.LogWarning(ex, "Strategic initiative with ID {Id} not found for rejection", id);
                throw new UserFriendlyException(L["StrategicInitiative:ErrorMessage:NotFound"]);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        /// <inheritdoc/>
        [Authorize(DecisionSupportPermissions.StrategicInitiative.Update)]
        public virtual async Task<StrategicInitiativeDto> AddAnalysisReferenceAsync(Guid id, AddAnalysisReferenceDto input)
        {
            try
            {
                var initiative = await _initiativeRepository.GetWithAnalysesAsync(id);
                
                initiative.AddSupportingAnalysis(
                    input.ReferenceType,
                    input.ReferenceKey,
                    input.Description);
                
                await _initiativeRepository.UpdateAsync(initiative);
                
                return MapInitiativeToDto(initiative);
            }
            catch (DecisionSupportDomainException ex)
            {
                await HandleErrorAsync(ex, LogLevel.Warning);
                throw new UserFriendlyException(L[ex.Code ?? "DefaultErrorMessage", ex.Message]);
            }
            catch (EntityNotFoundException ex)
            {
                Logger.LogWarning(ex, "Strategic initiative with ID {Id} not found for adding analysis reference", id);
                throw new UserFriendlyException(L["StrategicInitiative:ErrorMessage:NotFound"]);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        /// <inheritdoc/>
        [Authorize(DecisionSupportPermissions.StrategicInitiative.Update)]
        public virtual async Task RemoveAnalysisReferenceAsync(Guid id, string referenceType, string referenceKey)
        {
            try
            {
                var initiative = await _initiativeRepository.GetWithAnalysesAsync(id);
                
                initiative.RemoveSupportingAnalysis(referenceType, referenceKey);
                
                await _initiativeRepository.UpdateAsync(initiative);
            }
            catch (DecisionSupportDomainException ex)
            {
                await HandleErrorAsync(ex, LogLevel.Warning);
                throw new UserFriendlyException(L[ex.Code ?? "DefaultErrorMessage", ex.Message]);
            }
            catch (EntityNotFoundException ex)
            {
                Logger.LogWarning(ex, "Strategic initiative with ID {Id} not found for removing analysis reference", id);
                throw new UserFriendlyException(L["StrategicInitiative:ErrorMessage:NotFound"]);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        /// <inheritdoc/>
        [Authorize(DecisionSupportPermissions.StrategicInitiative.Create)]
        public virtual async Task<StrategicInitiativeDto> CreateFromDecisionModelAsync(Guid decisionModelId, CreateStrategicInitiativeDto input)
        {
            try
            {
                var initiative = await _initiativeManager.CreateFromDecisionModelAsync(
                    decisionModelId,
                    input.Title,
                    input.Description,
                    new DecisionRationaleText(input.Rationale),
                    new ExpectedOutcomeText(input.ExpectedOutcomes),
                    input.StrategicAlignment);
                
                return MapInitiativeToDto(initiative);
            }
            catch (DecisionSupportDomainException ex)
            {
                await HandleErrorAsync(ex, LogLevel.Warning);
                throw new UserFriendlyException(L[ex.Code ?? "DefaultErrorMessage", ex.Message]);
            }
            catch (EntityNotFoundException ex)
            {
                Logger.LogWarning(ex, "Decision model with ID {Id} not found for creating initiative", decisionModelId);
                throw new UserFriendlyException(L["DecisionModel:ErrorMessage:NotFound"]);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        private StrategicInitiativeDto MapInitiativeToDto(StrategicInitiativeProposal initiative)
        {
            var dto = new StrategicInitiativeDto
            {
                Id = initiative.Id,
                Title = initiative.Title,
                Description = initiative.Description,
                Rationale = initiative.Rationale,
                ExpectedOutcomes = initiative.ExpectedOutcomes,
                Status = initiative.Status,
                StatusName = L[$"ProposalStatus:{initiative.Status}"],
                StrategicAlignment = initiative.StrategicAlignment,
                AlignmentName = L[$"AlignmentStatus:{initiative.StrategicAlignment}"],
                ProjectId = initiative.ProjectId?.Value,
                RejectionReason = initiative.RejectionReason,
                RelatedDecisionModelId = initiative.RelatedDecisionModelId,
                ProposedDate = initiative.ProposedDate,
                DecisionDate = initiative.DecisionDate,
                CreationTime = initiative.CreationTime,
                CreatorId = initiative.CreatorId,
                LastModificationTime = initiative.LastModificationTime,
                LastModifierId = initiative.LastModifierId,
                DeleterId = initiative.DeleterId,
                DeletionTime = initiative.DeletionTime,
                SupportingAnalyses = new List<AnalysisReferenceDto>()
            };
            
            if (initiative.SupportingAnalyses != null && initiative.SupportingAnalyses.Any())
            {
                foreach (var analysis in initiative.SupportingAnalyses)
                {
                    dto.SupportingAnalyses.Add(new AnalysisReferenceDto
                    {
                        StrategicInitiativeId = analysis.StrategicInitiativeId,
                        ReferenceType = analysis.ReferenceType,
                        ReferenceKey = analysis.ReferenceKey,
                        Description = analysis.Description
                    });
                }
            }
            
            return dto;
        }

        private async Task HandleErrorAsync(Exception exception, LogLevel logLevel = LogLevel.Error)
        {
            Logger.Log(logLevel, exception, "Error in StrategicInitiativeAppService: {ErrorMessage}", exception.Message);
            await Task.CompletedTask;
        }
    }
}
