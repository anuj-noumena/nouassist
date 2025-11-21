using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NouAssist.RequirementsManagement.AnalysisPrioritization.Aggregates;
using NouAssist.RequirementsManagement.AnalysisPrioritization.Enums;
using NouAssist.RequirementsManagement.AnalysisPrioritization.Exceptions;
using NouAssist.RequirementsManagement.AnalysisPrioritization.Repositories;
using NouAssist.RequirementsManagement.AnalysisPrioritization.Specifications;
using NouAssist.RequirementsManagement.AnalysisPrioritization.ValueObjects;
using Volo.Abp;
using Volo.Abp.Domain.Services;
using Volo.Abp.Users;

namespace NouAssist.RequirementsManagement.AnalysisPrioritization.Services
{
    /// <summary>
    /// Domain service for managing analyzed requirements
    /// </summary>
    public class AnalyzedRequirementManager : DomainService, IAnalyzedRequirementManager
    {
        private readonly IAnalyzedRequirementRepository _analyzedRequirementRepository;
        private readonly IRelationshipMappingService _relationshipMappingService;
        private readonly ILogger<AnalyzedRequirementManager> _logger;

        public AnalyzedRequirementManager(
            IAnalyzedRequirementRepository analyzedRequirementRepository,
            IRelationshipMappingService relationshipMappingService,
            ILogger<AnalyzedRequirementManager> logger)
        {
            _analyzedRequirementRepository = analyzedRequirementRepository;
            _relationshipMappingService = relationshipMappingService;
            _logger = logger;
        }

        public async Task<AnalyzedRequirement> CreateOrUpdateAsync(
            Guid requirementId,
            string requirementTitle,
            CancellationToken cancellationToken = default)
        {
            Check.NotNullOrWhiteSpace(requirementTitle, nameof(requirementTitle));

            var existingAnalyzedRequirement = await _analyzedRequirementRepository
                .FindByRequirementIdAsync(requirementId, cancellationToken);

            if (existingAnalyzedRequirement != null)
            {
                existingAnalyzedRequirement.UpdateRequirementTitle(requirementTitle);
                return await _analyzedRequirementRepository.UpdateAsync(existingAnalyzedRequirement, cancellationToken: cancellationToken);
            }

            var analyzedRequirement = AnalyzedRequirement.Create(
                GuidGenerator.Create(),
                requirementId,
                requirementTitle,
                CurrentTenant.Id,
                CurrentUser.Id ?? Guid.Empty,
                Clock.Now);

            return await _analyzedRequirementRepository.InsertAsync(analyzedRequirement, cancellationToken: cancellationToken);
        }

        public async Task SetAttributeAsync(
            Guid analyzedRequirementId,
            string attributeName,
            AttributeType attributeType,
            AttributeValue attributeValue,
            CancellationToken cancellationToken = default)
        {
            var analyzedRequirement = await _analyzedRequirementRepository.GetAsync(analyzedRequirementId, cancellationToken: cancellationToken);

            analyzedRequirement.SetAttribute(
                attributeName,
                attributeType,
                attributeValue,
                CurrentUser.Id ?? Guid.Empty,
                Clock.Now);

            await _analyzedRequirementRepository.UpdateAsync(analyzedRequirement, cancellationToken: cancellationToken);

            _logger.LogInformation(
                "Attribute {AttributeName} set for analyzed requirement {Id}",
                attributeName,
                analyzedRequirementId);
        }

        public async Task RemoveAttributeAsync(
            Guid analyzedRequirementId,
            string attributeName,
            CancellationToken cancellationToken = default)
        {
            var analyzedRequirement = await _analyzedRequirementRepository.GetAsync(analyzedRequirementId, cancellationToken: cancellationToken);

            analyzedRequirement.RemoveAttribute(attributeName);
            await _analyzedRequirementRepository.UpdateAsync(analyzedRequirement, cancellationToken: cancellationToken);

            _logger.LogInformation(
                "Attribute {AttributeName} removed from analyzed requirement {Id}",
                attributeName,
                analyzedRequirementId);
        }

        public async Task AddRelationshipAsync(
            Guid sourceRequirementId,
            Guid targetRequirementId,
            RelationshipType relationshipType,
            string? description = null,
            CancellationToken cancellationToken = default)
        {
            // Validate the relationship using the domain service
            var isValid = await _relationshipMappingService.ValidateRelationshipAsync(
                sourceRequirementId,
                targetRequirementId,
                relationshipType,
                cancellationToken);

            if (!isValid)
            {
                throw new CircularDependencyException(sourceRequirementId, targetRequirementId);
            }

            var sourceAnalyzedRequirement = await _analyzedRequirementRepository
                .FindByRequirementIdAsync(sourceRequirementId, cancellationToken);

            if (sourceAnalyzedRequirement == null)
            {
                throw new BusinessException("RequirementsAnalysisTools:ErrorMessage:SourceRequirementNotAnalyzed");
            }

            sourceAnalyzedRequirement.AddRelationship(
                targetRequirementId,
                relationshipType,
                description,
                CurrentUser.Id ?? Guid.Empty,
                Clock.Now);

            await _analyzedRequirementRepository.UpdateAsync(sourceAnalyzedRequirement, cancellationToken: cancellationToken);

            _logger.LogInformation(
                "Relationship {RelationshipType} added from {SourceId} to {TargetId}",
                relationshipType,
                sourceRequirementId,
                targetRequirementId);
        }

        public async Task RemoveRelationshipAsync(
            Guid analyzedRequirementId,
            Guid relationshipId,
            CancellationToken cancellationToken = default)
        {
            var analyzedRequirement = await _analyzedRequirementRepository.GetAsync(analyzedRequirementId, cancellationToken: cancellationToken);

            analyzedRequirement.RemoveRelationship(relationshipId);
            await _analyzedRequirementRepository.UpdateAsync(analyzedRequirement, cancellationToken: cancellationToken);

            _logger.LogInformation(
                "Relationship {RelationshipId} removed from analyzed requirement {Id}",
                relationshipId,
                analyzedRequirementId);
        }

        public async Task SetImpactAssessmentAsync(
            Guid analyzedRequirementId,
            ImpactAssessmentResult impactAssessment,
            CancellationToken cancellationToken = default)
        {
            var analyzedRequirement = await _analyzedRequirementRepository.GetAsync(analyzedRequirementId, cancellationToken: cancellationToken);

            analyzedRequirement.SetImpactAssessment(
                impactAssessment,
                CurrentUser.Id ?? Guid.Empty,
                Clock.Now);

            await _analyzedRequirementRepository.UpdateAsync(analyzedRequirement, cancellationToken: cancellationToken);

            _logger.LogInformation(
                "Impact assessment set for analyzed requirement {Id} with overall score {Score}",
                analyzedRequirementId,
                impactAssessment.OverallScore);
        }

        public async Task<List<AnalyzedRequirement>> GetRequirementsNeedingAnalysisAsync(
            CancellationToken cancellationToken = default)
        {
            // Get requirements that either don't have impact assessment or haven't been analyzed recently
            var missingImpactSpec = new MissingImpactAssessmentSpecification();
            var oldAnalysisSpec = new AnalyzedWithinPeriodSpecification(DateTime.MinValue, Clock.Now.AddDays(-30));

            var combinedSpec = missingImpactSpec.Or(oldAnalysisSpec.Not());

            return await _analyzedRequirementRepository.GetListAsync(combinedSpec, cancellationToken);
        }
    }
}
