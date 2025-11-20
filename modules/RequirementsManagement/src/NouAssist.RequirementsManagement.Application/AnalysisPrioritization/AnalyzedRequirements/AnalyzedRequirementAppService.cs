using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using NouAssist.RequirementsManagement.AnalysisPrioritization.AnalyzedRequirements.Dtos;
using NouAssist.RequirementsManagement.AnalysisPrioritization.Aggregates;
using NouAssist.RequirementsManagement.AnalysisPrioritization.Permissions;
using NouAssist.RequirementsManagement.AnalysisPrioritization.Repositories;
using NouAssist.RequirementsManagement.AnalysisPrioritization.Services;
using NouAssist.RequirementsManagement.AnalysisPrioritization.Specifications;
using NouAssist.RequirementsManagement.AnalysisPrioritization.ValueObjects;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.RequirementsManagement.AnalysisPrioritization.AnalyzedRequirements
{
    [Authorize(RequirementsAnalysisToolsPermissions.AnalyzedRequirements.Default)]
    public class AnalyzedRequirementAppService : ApplicationService, IAnalyzedRequirementAppService
    {
        private readonly IAnalyzedRequirementRepository _analyzedRequirementRepository;
        private readonly IAnalyzedRequirementManager _analyzedRequirementManager;
        private readonly IImpactAnalysisEngine _impactAnalysisEngine;

        public AnalyzedRequirementAppService(
            IAnalyzedRequirementRepository analyzedRequirementRepository,
            IAnalyzedRequirementManager analyzedRequirementManager,
            IImpactAnalysisEngine impactAnalysisEngine)
        {
            _analyzedRequirementRepository = analyzedRequirementRepository;
            _analyzedRequirementManager = analyzedRequirementManager;
            _impactAnalysisEngine = impactAnalysisEngine;

            LocalizationResource = typeof(RequirementsAnalysisToolsResource);
        }

        [Authorize(RequirementsAnalysisToolsPermissions.AnalyzedRequirements.View)]
        public virtual async Task<AnalyzedRequirementDto> GetAsync(Guid id)
        {
            try
            {
                var analyzedRequirement = await _analyzedRequirementRepository.GetAsync(id);
                var dto = ObjectMapper.Map<AnalyzedRequirement, AnalyzedRequirementDto>(analyzedRequirement);
                return dto;
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        [Authorize(RequirementsAnalysisToolsPermissions.AnalyzedRequirements.View)]
        public virtual async Task<PagedResultDto<AnalyzedRequirementDto>> GetListAsync(GetAnalyzedRequirementListDto input)
        {
            try
            {
                var query = await _analyzedRequirementRepository.GetQueryableAsync();

                // Apply filters
                query = query.WhereIf(!input.Filter.IsNullOrWhiteSpace(),
                    x => x.RequirementTitle.Contains(input.Filter!));

                if (input.AttributeType.HasValue)
                {
                    query = query.Where(x => x.Attributes.Any(a => a.AttributeType == input.AttributeType));
                }

                if (input.RelationshipType.HasValue)
                {
                    query = query.Where(x => x.Relationships.Any(r => r.RelationshipType == input.RelationshipType));
                }

                if (input.HasImpactAssessment.HasValue)
                {
                    if (input.HasImpactAssessment.Value)
                    {
                        query = query.Where(x => x.ImpactAssessment != null);
                    }
                    else
                    {
                        query = query.Where(x => x.ImpactAssessment == null);
                    }
                }

                if (input.AnalyzedAfter.HasValue)
                {
                    query = query.Where(x => x.LastAnalyzedAt.HasValue && x.LastAnalyzedAt >= input.AnalyzedAfter);
                }

                if (input.AnalyzedBefore.HasValue)
                {
                    query = query.Where(x => x.LastAnalyzedAt.HasValue && x.LastAnalyzedAt <= input.AnalyzedBefore);
                }

                var totalCount = await AsyncExecuter.CountAsync(query);

                var items = new List<AnalyzedRequirement>();
                if (totalCount > 0)
                {
                    items = await AsyncExecuter.ToListAsync(
                        query.OrderBy(input.Sorting ?? nameof(AnalyzedRequirement.RequirementTitle))
                            .PageBy(input)
                    );
                }

                return new PagedResultDto<AnalyzedRequirementDto>(
                    totalCount,
                    ObjectMapper.Map<List<AnalyzedRequirement>, List<AnalyzedRequirementDto>>(items)
                );
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        [Authorize(RequirementsAnalysisToolsPermissions.AnalyzedRequirements.Create)]
        public virtual async Task<AnalyzedRequirementDto> CreateAsync(CreateAnalyzedRequirementDto input)
        {
            try
            {
                var analyzedRequirement = await _analyzedRequirementManager.CreateOrUpdateAsync(
                    input.RequirementId,
                    input.RequirementTitle
                );

                return ObjectMapper.Map<AnalyzedRequirement, AnalyzedRequirementDto>(analyzedRequirement);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        [Authorize(RequirementsAnalysisToolsPermissions.AnalyzedRequirements.Update)]
        public virtual async Task<AnalyzedRequirementDto> UpdateAsync(Guid id, UpdateAnalyzedRequirementDto input)
        {
            try
            {
                var analyzedRequirement = await _analyzedRequirementRepository.GetAsync(id);
                analyzedRequirement.UpdateRequirementTitle(input.RequirementTitle);

                await _analyzedRequirementRepository.UpdateAsync(analyzedRequirement);

                return ObjectMapper.Map<AnalyzedRequirement, AnalyzedRequirementDto>(analyzedRequirement);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        [Authorize(RequirementsAnalysisToolsPermissions.AnalyzedRequirements.Delete)]
        public virtual async Task DeleteAsync(Guid id)
        {
            try
            {
                await _analyzedRequirementRepository.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        [Authorize(RequirementsAnalysisToolsPermissions.AnalyzedRequirements.ManageAttributes)]
        public virtual async Task SetAttributeAsync(Guid id, SetAttributeDto input)
        {
            try
            {
                var attributeValue = ObjectMapper.Map<AttributeValueDto, AttributeValue>(input.AttributeValue);

                await _analyzedRequirementManager.SetAttributeAsync(
                    id,
                    input.AttributeName,
                    input.AttributeType,
                    attributeValue
                );
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        [Authorize(RequirementsAnalysisToolsPermissions.AnalyzedRequirements.ManageAttributes)]
        public virtual async Task RemoveAttributeAsync(Guid id, string attributeName)
        {
            try
            {
                await _analyzedRequirementManager.RemoveAttributeAsync(id, attributeName);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        [Authorize(RequirementsAnalysisToolsPermissions.AnalyzedRequirements.ManageRelationships)]
        public virtual async Task AddRelationshipAsync(Guid id, AddRelationshipDto input)
        {
            try
            {
                var analyzedRequirement = await _analyzedRequirementRepository.GetAsync(id);

                await _analyzedRequirementManager.AddRelationshipAsync(
                    analyzedRequirement.RequirementId,
                    input.TargetRequirementId,
                    input.RelationshipType,
                    input.Description
                );
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        [Authorize(RequirementsAnalysisToolsPermissions.AnalyzedRequirements.ManageRelationships)]
        public virtual async Task RemoveRelationshipAsync(Guid id, Guid relationshipId)
        {
            try
            {
                await _analyzedRequirementManager.RemoveRelationshipAsync(id, relationshipId);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        [Authorize(RequirementsAnalysisToolsPermissions.AnalyzedRequirements.ManageImpactAssessment)]
        public virtual async Task SetImpactAssessmentAsync(Guid id, SetImpactAssessmentDto input)
        {
            try
            {
                var impactAssessment = new ImpactAssessmentResult(
                    input.TechnicalImpact,
                    input.BusinessImpact,
                    input.UserImpact,
                    input.RiskImpact,
                    input.Description
                );

                await _analyzedRequirementManager.SetImpactAssessmentAsync(id, impactAssessment);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        [Authorize(RequirementsAnalysisToolsPermissions.AnalyzedRequirements.ManageImpactAssessment)]
        public virtual async Task<ImpactAssessmentDto> AnalyzeImpactAsync(Guid id)
        {
            try
            {
                var impactAssessment = await _impactAnalysisEngine.AnalyzeImpactAsync(id);
                
                // Save the analysis result
                await _analyzedRequirementManager.SetImpactAssessmentAsync(id, impactAssessment);

                return ObjectMapper.Map<ImpactAssessmentResult, ImpactAssessmentDto>(impactAssessment);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        [Authorize(RequirementsAnalysisToolsPermissions.AnalyzedRequirements.View)]
        public virtual async Task<PagedResultDto<AnalyzedRequirementDto>> GetRequirementsNeedingAnalysisAsync(
            PagedResultRequestDto input)
        {
            try
            {
                var requirements = await _analyzedRequirementManager.GetRequirementsNeedingAnalysisAsync();

                var totalCount = requirements.Count;
                var pagedItems = requirements
                    .Skip(input.SkipCount)
                    .Take(input.MaxResultCount)
                    .ToList();

                return new PagedResultDto<AnalyzedRequirementDto>(
                    totalCount,
                    ObjectMapper.Map<List<AnalyzedRequirement>, List<AnalyzedRequirementDto>>(pagedItems)
                );
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        private async Task HandleErrorAsync(Exception exception, LogLevel logLevel = LogLevel.Error)
        {
            Logger.Log(logLevel, exception, 
                "Error in AnalyzedRequirementAppService: {ErrorMessage}", 
                exception.Message);
        }
    }
}
