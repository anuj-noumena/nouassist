using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NouAssist.RequirementsManagement.Specification.Enums;
using NouAssist.RequirementsManagement.Specification.Exceptions;
using NouAssist.RequirementsManagement.Specification.Interfaces.Services;
using NouAssist.RequirementsManagement.Specification.Repositories;
using NouAssist.RequirementsManagement.Specification.ValueObjects;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace NouAssist.RequirementsManagement.Specification.Services
{
    /// <summary>
    /// Domain service for requirement management
    /// </summary>
    public class RequirementManager : DomainService, IRequirementManager
    {
        private readonly IRequirementRepository _requirementRepository;
        private readonly ILogger<RequirementManager> _logger;

        public RequirementManager(
            IRequirementRepository requirementRepository,
            ILogger<RequirementManager> logger)
        {
            _requirementRepository = requirementRepository;
            _logger = logger;
        }

        public async Task<Requirement> CreateAsync(
            RequirementIdentifier identifier,
            RequirementText text,
            RequirementType type,
            RequirementPriority priority,
            Guid? tenantId = null)
        {
            Check.NotNull(identifier, nameof(identifier));
            Check.NotNull(text, nameof(text));

            // Verify identifier uniqueness
            if (!await IsIdentifierUniqueAsync(identifier))
            {
                throw new DuplicateRequirementIdentifierException(identifier.Value);
            }

            try
            {
                var requirement = Requirement.Create(
                    GuidGenerator.Create(),
                    identifier,
                    text,
                    type,
                    priority,
                    tenantId
                );

                _logger.LogInformation(
                    "Creating requirement with identifier {Identifier} and type {Type}",
                    identifier.Value,
                    type
                );

                return requirement;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Failed to create requirement with identifier {Identifier}",
                    identifier.Value);
                throw;
            }
        }

        public async Task<bool> IsIdentifierUniqueAsync(RequirementIdentifier identifier, Guid? excludeId = null)
        {
            Check.NotNull(identifier, nameof(identifier));

            var existingRequirement = await _requirementRepository.FindByIdentifierAsync(identifier);
            return existingRequirement == null || (excludeId.HasValue && existingRequirement.Id == excludeId.Value);
        }

        public async Task ValidateForDeletionAsync(Requirement requirement)
        {
            Check.NotNull(requirement, nameof(requirement));

            // Check if requirement has business objective links
            if (requirement.BusinessObjectiveLinks.Any())
            {
                throw new BusinessException("RequirementSpecificationManagement:ErrorMessage:CannotDeleteLinkedRequirement")
                    .WithData("requirementId", requirement.Id)
                    .WithData("objectiveCount", requirement.BusinessObjectiveLinks.Count);
            }

            // Check if requirement is in a state that prevents deletion
            if (requirement.Status == RequirementStatus.Implemented || requirement.Status == RequirementStatus.Tested)
            {
                throw new BusinessException("RequirementSpecificationManagement:ErrorMessage:CannotDeleteImplementedRequirement")
                    .WithData("requirementId", requirement.Id)
                    .WithData("status", requirement.Status);
            }

            _logger.LogInformation(
                "Validated requirement {RequirementId} for deletion",
                requirement.Id
            );
        }
    }
}
