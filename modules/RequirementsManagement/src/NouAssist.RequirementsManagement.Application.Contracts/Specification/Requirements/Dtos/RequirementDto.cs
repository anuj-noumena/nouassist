using System;
using System.Collections.Generic;
using NouAssist.RequirementsManagement.Specification.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.RequirementsManagement.Specification.Requirements.Dtos
{
    /// <summary>
    /// DTO representing a requirement for client consumption
    /// </summary>
    public class RequirementDto : FullAuditedEntityDto<Guid>
    {
        public string Identifier { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public RequirementType Type { get; set; }
        public RequirementStatus Status { get; set; }
        public RequirementPriority Priority { get; set; }
        public string Version { get; set; }

        // Related entities
        public List<AcceptanceCriterionDto> AcceptanceCriteria { get; set; } = new();
        public List<AssociatedRuleDto> AssociatedRules { get; set; } = new();
        public List<AssociatedConstraintDto> AssociatedConstraints { get; set; } = new();
        public List<DataModelReferenceDto> DataModelReferences { get; set; } = new();
        public List<InterfaceSpecificationDto> InterfaceSpecifications { get; set; } = new();
        public List<BusinessObjectiveLinkDto> BusinessObjectiveLinks { get; set; } = new();
        public RequirementScopeMarkerDto ScopeMarker { get; set; }

        // Computed properties
        public bool CanBeModified { get; set; }
        public int AcceptanceCriteriaCount { get; set; }
        public int BusinessRulesCount { get; set; }
        public int ConstraintsCount { get; set; }
    }
}
