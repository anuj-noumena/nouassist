using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using NouAssist.RequirementsManagement.Specification.Enums;

namespace NouAssist.RequirementsManagement.Specification.Specifications
{
    /// <summary>
    /// Base specification class for Requirement aggregate
    /// </summary>
    public abstract class RequirementSpecification : Specification<Requirement>
    {
    }

    /// <summary>
    /// Specification for approved requirements
    /// </summary>
    public class ApprovedRequirementSpecification : RequirementSpecification
    {
        public override Expression<Func<Requirement, bool>> ToExpression()
        {
            return requirement => requirement.Status == RequirementStatus.Approved;
        }
    }

    /// <summary>
    /// Specification for requirements by type
    /// </summary>
    public class RequirementByTypeSpecification : RequirementSpecification
    {
        private readonly RequirementType _type;

        public RequirementByTypeSpecification(RequirementType type)
        {
            _type = type;
        }

        public override Expression<Func<Requirement, bool>> ToExpression()
        {
            return requirement => requirement.Type == _type;
        }
    }

    /// <summary>
    /// Specification for requirements by priority
    /// </summary>
    public class RequirementByPrioritySpecification : RequirementSpecification
    {
        private readonly RequirementPriority _priority;

        public RequirementByPrioritySpecification(RequirementPriority priority)
        {
            _priority = priority;
        }

        public override Expression<Func<Requirement, bool>> ToExpression()
        {
            return requirement => requirement.Priority == _priority;
        }
    }

    /// <summary>
    /// Specification for requirements in scope
    /// </summary>
    public class InScopeRequirementSpecification : RequirementSpecification
    {
        public override Expression<Func<Requirement, bool>> ToExpression()
        {
            return requirement => requirement.ScopeMarker != null &&
                                  requirement.ScopeMarker.ScopeStatus == ScopeStatus.InScope;
        }
    }

    /// <summary>
    /// Specification for requirements linked to business objectives
    /// </summary>
    public class RequirementLinkedToObjectiveSpecification : RequirementSpecification
    {
        private readonly Guid _objectiveId;

        public RequirementLinkedToObjectiveSpecification(Guid objectiveId)
        {
            _objectiveId = objectiveId;
        }

        public override Expression<Func<Requirement, bool>> ToExpression()
        {
            return requirement => requirement.BusinessObjectiveLinks
                .Any(link => link.ObjectiveId.Value == _objectiveId);
        }
    }

    /// <summary>
    /// Specification for requirements with acceptance criteria
    /// </summary>
    public class RequirementWithAcceptanceCriteriaSpecification : RequirementSpecification
    {
        public override Expression<Func<Requirement, bool>> ToExpression()
        {
            return requirement => requirement.AcceptanceCriteria.Any();
        }
    }

    /// <summary>
    /// Specification for requirements by status list
    /// </summary>
    public class RequirementByStatusListSpecification : RequirementSpecification
    {
        private readonly List<RequirementStatus> _statuses;

        public RequirementByStatusListSpecification(params RequirementStatus[] statuses)
        {
            _statuses = statuses?.ToList() ?? new List<RequirementStatus>();
        }

        public override Expression<Func<Requirement, bool>> ToExpression()
        {
            return requirement => _statuses.Contains(requirement.Status);
        }
    }

    /// <summary>
    /// Specification for requirements by identifier pattern
    /// </summary>
    public class RequirementByIdentifierPatternSpecification : RequirementSpecification
    {
        private readonly string _pattern;

        public RequirementByIdentifierPatternSpecification(string pattern)
        {
            _pattern = pattern ?? string.Empty;
        }

        public override Expression<Func<Requirement, bool>> ToExpression()
        {
            return requirement => requirement.Identifier.Value.Contains(_pattern);
        }
    }

    /// <summary>
    /// Specification for requirements that can be modified
    /// </summary>
    public class ModifiableRequirementSpecification : RequirementSpecification
    {
        public override Expression<Func<Requirement, bool>> ToExpression()
        {
            return requirement => requirement.Status != RequirementStatus.Archived &&
                                  requirement.Status != RequirementStatus.Tested;
        }

        /// <summary>
        /// Validates if a requirement can be modified
        /// </summary>
        public ValidationResult ValidateRequirement(Requirement requirement)
        {
            var result = new ValidationResult();

            if (requirement.Status == RequirementStatus.Archived)
            {
                result.AddError("Cannot modify archived requirements");
            }

            if (requirement.Status == RequirementStatus.Tested)
            {
                result.AddError("Cannot modify tested requirements");
            }

            return result;
        }
    }
}
