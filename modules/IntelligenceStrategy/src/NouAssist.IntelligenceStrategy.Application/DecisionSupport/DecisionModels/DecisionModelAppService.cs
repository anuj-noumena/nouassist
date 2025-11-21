using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using NouAssist.IntelligenceStrategy.DecisionSupport.Aggregates;
using NouAssist.IntelligenceStrategy.DecisionSupport.DecisionModels.Dtos;
using NouAssist.IntelligenceStrategy.DecisionSupport.Exceptions;
using NouAssist.IntelligenceStrategy.DecisionSupport.Interfaces.Services;
using NouAssist.IntelligenceStrategy.DecisionSupport.Localization;
using NouAssist.IntelligenceStrategy.DecisionSupport.Permissions;
using NouAssist.IntelligenceStrategy.DecisionSupport.Repositories;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.IntelligenceStrategy.DecisionSupport.DecisionModels
{
    /// <summary>
    /// Application service for managing decision models
    /// </summary>
    [Authorize(DecisionSupportPermissions.DecisionModel.Default)]
    public class DecisionModelAppService : ApplicationService, IDecisionModelAppService
    {
        private readonly IDecisionModelRepository _decisionModelRepository;
        private readonly IDecisionModelManager _decisionModelManager;

        /// <summary>
        /// Creates a new instance of <see cref="DecisionModelAppService"/>
        /// </summary>
        public DecisionModelAppService(
            IDecisionModelRepository decisionModelRepository,
            IDecisionModelManager decisionModelManager)
        {
            _decisionModelRepository = decisionModelRepository;
            _decisionModelManager = decisionModelManager;
            
            LocalizationResource = typeof(DecisionSupportResource);
        }

        /// <inheritdoc/>
        [Authorize(DecisionSupportPermissions.DecisionModel.View)]
        public virtual async Task<DecisionModelDto> GetAsync(Guid id)
        {
            try
            {
                var decisionModel = await _decisionModelRepository.GetWithDetailsAsync(id);
                return MapDecisionModelToDto(decisionModel);
            }
            catch (EntityNotFoundException ex)
            {
                Logger.LogWarning(ex, "Decision model with ID {Id} not found", id);
                throw new UserFriendlyException(L["DecisionModel:ErrorMessage:NotFound"]);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        /// <inheritdoc/>
        [Authorize(DecisionSupportPermissions.DecisionModel.View)]
        public virtual async Task<PagedResultDto<DecisionModelListDto>> GetListAsync(GetDecisionModelListDto input)
        {
            try
            {
                var query = await _decisionModelRepository.GetQueryableAsync();
                
                // Apply filters
                if (!string.IsNullOrWhiteSpace(input.Filter))
                {
                    query = query.Where(d => 
                        d.Title.Contains(input.Filter) || 
                        d.Description.Contains(input.Filter));
                }
                
                if (input.IsFinalized.HasValue)
                {
                    query = query.Where(d => d.IsFinalized == input.IsFinalized.Value);
                }
                
                if (!string.IsNullOrWhiteSpace(input.ModelType))
                {
                    query = query.Where(d => d.ModelType == input.ModelType);
                }
                
                // Include criteria and alternatives for counts
                query = query.Include(x => x.Criteria)
                             .Include(x => x.Alternatives);
                
                // Get total count
                var totalCount = await AsyncExecuter.CountAsync(query);
                
                // Apply sorting and paging
                var sorting = input.Sorting ?? "creationTime desc";
                var decisionModels = await AsyncExecuter.ToListAsync(
                    query.OrderBy(sorting)
                        .PageBy(input.SkipCount, input.MaxResultCount));
                
                // Map to DTOs
                var dtos = decisionModels.Select(d => new DecisionModelListDto
                {
                    Id = d.Id,
                    Title = d.Title,
                    ModelType = d.ModelType,
                    IsFinalized = d.IsFinalized,
                    DecisionDate = d.DecisionDate,
                    CriteriaCount = d.Criteria.Count,
                    AlternativesCount = d.Alternatives.Count,
                    CreationTime = d.CreationTime,
                    CreatorId = d.CreatorId,
                    LastModificationTime = d.LastModificationTime,
                    LastModifierId = d.LastModifierId
                }).ToList();
                
                return new PagedResultDto<DecisionModelListDto>(totalCount, dtos);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        /// <inheritdoc/>
        [Authorize(DecisionSupportPermissions.DecisionModel.Create)]
        public virtual async Task<DecisionModelDto> CreateAsync(CreateDecisionModelDto input)
        {
            try
            {
                // Use domain service to create the decision model
                var decisionModel = await _decisionModelManager.CreateModelAsync(
                    input.Title,
                    input.Description,
                    input.ModelType);
                
                return MapDecisionModelToDto(decisionModel);
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
        [Authorize(DecisionSupportPermissions.DecisionModel.Delete)]
        public virtual async Task DeleteAsync(Guid id)
        {
            try
            {
                await _decisionModelRepository.DeleteAsync(id);
            }
            catch (EntityNotFoundException ex)
            {
                Logger.LogWarning(ex, "Decision model with ID {Id} not found for deletion", id);
                throw new UserFriendlyException(L["DecisionModel:ErrorMessage:NotFound"]);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        /// <inheritdoc/>
        [Authorize(DecisionSupportPermissions.DecisionModel.Update)]
        public virtual async Task<CriterionDto> AddCriterionAsync(Guid id, AddCriterionDto input)
        {
            try
            {
                var criterion = await _decisionModelManager.AddCriterionAsync(
                    id,
                    input.Name,
                    input.Description,
                    input.Weight);
                
                return new CriterionDto
                {
                    Id = criterion.Id,
                    DecisionModelId = criterion.DecisionModelId,
                    Name = criterion.Name,
                    Description = criterion.Description,
                    Weight = criterion.Weight
                };
            }
            catch (DecisionSupportDomainException ex)
            {
                await HandleErrorAsync(ex, LogLevel.Warning);
                throw new UserFriendlyException(L[ex.Code ?? "DefaultErrorMessage", ex.Message]);
            }
            catch (EntityNotFoundException ex)
            {
                Logger.LogWarning(ex, "Decision model with ID {Id} not found for adding criterion", id);
                throw new UserFriendlyException(L["DecisionModel:ErrorMessage:NotFound"]);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        /// <inheritdoc/>
        [Authorize(DecisionSupportPermissions.DecisionModel.Update)]
        public virtual async Task<AlternativeDto> AddAlternativeAsync(Guid id, AddAlternativeDto input)
        {
            try
            {
                var alternative = await _decisionModelManager.AddAlternativeAsync(
                    id,
                    input.Name,
                    input.Description);
                
                return new AlternativeDto
                {
                    Id = alternative.Id,
                    DecisionModelId = alternative.DecisionModelId,
                    Name = alternative.Name,
                    Description = alternative.Description,
                    Scores = new List<AlternativeScoreDto>(),
                    IsSelected = false
                };
            }
            catch (DecisionSupportDomainException ex)
            {
                await HandleErrorAsync(ex, LogLevel.Warning);
                throw new UserFriendlyException(L[ex.Code ?? "DefaultErrorMessage", ex.Message]);
            }
            catch (EntityNotFoundException ex)
            {
                Logger.LogWarning(ex, "Decision model with ID {Id} not found for adding alternative", id);
                throw new UserFriendlyException(L["DecisionModel:ErrorMessage:NotFound"]);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        /// <inheritdoc/>
        [Authorize(DecisionSupportPermissions.DecisionModel.Update)]
        public virtual async Task SetScoreAsync(Guid id, Guid alternativeId, SetScoreDto input)
        {
            try
            {
                await _decisionModelManager.SetScoreAsync(
                    id,
                    alternativeId,
                    input.CriterionId,
                    input.Score);
            }
            catch (DecisionSupportDomainException ex)
            {
                await HandleErrorAsync(ex, LogLevel.Warning);
                throw new UserFriendlyException(L[ex.Code ?? "DefaultErrorMessage", ex.Message]);
            }
            catch (EntityNotFoundException ex)
            {
                Logger.LogWarning(ex, "Decision model with ID {Id} or alternative with ID {AlternativeId} not found for setting score", id, alternativeId);
                throw new UserFriendlyException(L["DecisionModel:ErrorMessage:EntityNotFound"]);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        /// <inheritdoc/>
        [Authorize(DecisionSupportPermissions.DecisionModel.Finalize)]
        public virtual async Task<DecisionModelDto> FinalizeDecisionAsync(Guid id, FinalizeDecisionDto input)
        {
            try
            {
                var decisionModel = await _decisionModelManager.FinalizeDecisionAsync(
                    id,
                    input.SelectedAlternativeId);
                
                return MapDecisionModelToDto(decisionModel);
            }
            catch (DecisionSupportDomainException ex)
            {
                await HandleErrorAsync(ex, LogLevel.Warning);
                throw new UserFriendlyException(L[ex.Code ?? "DefaultErrorMessage", ex.Message]);
            }
            catch (EntityNotFoundException ex)
            {
                Logger.LogWarning(ex, "Decision model with ID {Id} or alternative with ID {AlternativeId} not found for finalizing", id, input.SelectedAlternativeId);
                throw new UserFriendlyException(L["DecisionModel:ErrorMessage:EntityNotFound"]);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        private DecisionModelDto MapDecisionModelToDto(DecisionModelInstance decisionModel)
        {
            var dto = new DecisionModelDto
            {
                Id = decisionModel.Id,
                Title = decisionModel.Title,
                Description = decisionModel.Description,
                ModelType = decisionModel.ModelType,
                IsFinalized = decisionModel.IsFinalized,
                DecisionDate = decisionModel.DecisionDate,
                SelectedAlternativeId = decisionModel.SelectedAlternativeId,
                CreationTime = decisionModel.CreationTime,
                CreatorId = decisionModel.CreatorId,
                LastModificationTime = decisionModel.LastModificationTime,
                LastModifierId = decisionModel.LastModifierId,
                DeleterId = decisionModel.DeleterId,
                DeletionTime = decisionModel.DeletionTime,
                Criteria = new List<CriterionDto>(),
                Alternatives = new List<AlternativeDto>()
            };
            
            // Map criteria
            if (decisionModel.Criteria != null && decisionModel.Criteria.Any())
            {
                foreach (var criterion in decisionModel.Criteria)
                {
                    dto.Criteria.Add(new CriterionDto
                    {
                        Id = criterion.Id,
                        DecisionModelId = criterion.DecisionModelId,
                        Name = criterion.Name,
                        Description = criterion.Description,
                        Weight = criterion.Weight
                    });
                }
            }
            
            // Map alternatives with scores
            if (decisionModel.Alternatives != null && decisionModel.Alternatives.Any())
            {
                // Create dictionary for efficient criterion lookup
                var criteriaDict = dto.Criteria.ToDictionary(c => c.Id, c => c);
                
                foreach (var alternative in decisionModel.Alternatives)
                {
                    var altDto = new AlternativeDto
                    {
                        Id = alternative.Id,
                        DecisionModelId = alternative.DecisionModelId,
                        Name = alternative.Name,
                        Description = alternative.Description,
                        IsSelected = decisionModel.SelectedAlternativeId == alternative.Id,
                        Scores = new List<AlternativeScoreDto>(),
                        TotalScore = 0
                    };
                    
                    // Map scores and calculate total weighted score
                    if (alternative.Scores != null && alternative.Scores.Any())
                    {
                        decimal totalWeightedScore = 0;
                        
                        foreach (var score in alternative.Scores)
                        {
                            var criterionWeight = criteriaDict.TryGetValue(score.CriterionId, out var criterion) 
                                ? criterion.Weight 
                                : 0;
                                
                            var weightedScore = score.Score * criterionWeight;
                            totalWeightedScore += weightedScore;
                            
                            altDto.Scores.Add(new AlternativeScoreDto
                            {
                                AlternativeId = score.AlternativeId,
                                CriterionId = score.CriterionId,
                                Score = score.Score,
                                WeightedScore = weightedScore
                            });
                        }
                        
                        altDto.TotalScore = totalWeightedScore;
                    }
                    
                    dto.Alternatives.Add(altDto);
                }
            }
            
            return dto;
        }

        private async Task HandleErrorAsync(Exception exception, LogLevel logLevel = LogLevel.Error)
        {
            Logger.Log(logLevel, exception, "Error in DecisionModelAppService: {ErrorMessage}", exception.Message);
            await Task.CompletedTask;
        }
    }
}
