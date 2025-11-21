using System;
using System.Collections.Generic;
using System.Linq;
using NouAssist.RequirementsManagement.Specification.Entities;
using NouAssist.RequirementsManagement.Specification.Enums;
using NouAssist.RequirementsManagement.Specification.Events;
using NouAssist.RequirementsManagement.Specification.Exceptions;
using NouAssist.RequirementsManagement.Specification.Policies;
using NouAssist.RequirementsManagement.Specification.ValueObjects;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace NouAssist.RequirementsManagement.Specification.Aggregates
{
    /// <summary>
    /// Represents a requirement aggregate with its associated elements
    /// </summary>
    public class Requirement : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Guid? TenantId { get; protected set; }
        public RequirementIdentifier Identifier { get; private set; }
        public RequirementText Text { get; private set; }
        public RequirementType Type { get; private set; }
        public RequirementStatus Status { get; private set; }
        public RequirementPriority Priority { get; private set; }
        public string Version { get; private set; }

        // Collections for related entities
        private readonly List<AcceptanceCriterion> _acceptanceCriteria;
        private readonly List<AssociatedRule> _associatedRules;
        private readonly List<AssociatedConstraint> _associatedConstraints;
        private readonly List<DataModelReference> _dataModelReferences;
        private readonly List<InterfaceSpecification> _interfaceSpecifications;
        private readonly List<BusinessObjectiveLink> _businessObjectiveLinks;
        private RequirementScopeMarker _scopeMarker;

        // Public read-only access
        public IReadOnlyCollection<AcceptanceCriterion> AcceptanceCriteria => _acceptanceCriteria.AsReadOnly();
        public IReadOnlyCollection<AssociatedRule> AssociatedRules => _associatedRules.AsReadOnly();
        public IReadOnlyCollection<AssociatedConstraint> AssociatedConstraints => _associatedConstraints.AsReadOnly();
        public IReadOnlyCollection<DataModelReference> DataModelReferences => _dataModelReferences.AsReadOnly();
        public IReadOnlyCollection<InterfaceSpecification> InterfaceSpecifications => _interfaceSpecifications.AsReadOnly();
        public IReadOnlyCollection<BusinessObjectiveLink> BusinessObjectiveLinks => _businessObjectiveLinks.AsReadOnly();
        public RequirementScopeMarker ScopeMarker => _scopeMarker;

        protected Requirement()
        {
            _acceptanceCriteria = new List<AcceptanceCriterion>();
            _associatedRules = new List<AssociatedRule>();
            _associatedConstraints = new List<AssociatedConstraint>();
            _dataModelReferences = new List<DataModelReference>();
            _interfaceSpecifications = new List<InterfaceSpecification>();
            _businessObjectiveLinks = new List<BusinessObjectiveLink>();
        }

        /// <summary>
        /// Factory method to create a new requirement
        /// </summary>
        public static Requirement Create(
            Guid id,
            RequirementIdentifier identifier,
            RequirementText text,
            RequirementType type,
            RequirementPriority priority,
            Guid? tenantId)
        {
            return new Requirement(id, identifier, text, type, priority, tenantId);
        }

        private Requirement(
            Guid id,
            RequirementIdentifier identifier,
            RequirementText text,
            RequirementType type,
            RequirementPriority priority,
            Guid? tenantId) : base(id)
        {
            Identifier = identifier ?? throw new ArgumentNullException(nameof(identifier));
            Text = text ?? throw new ArgumentNullException(nameof(text));
            Type = type;
            Priority = priority;
            Status = RequirementStatus.Draft;
            Version = "1.0";
            TenantId = tenantId;

            _acceptanceCriteria = new List<AcceptanceCriterion>();
            _associatedRules = new List<AssociatedRule>();
            _associatedConstraints = new List<AssociatedConstraint>();
            _dataModelReferences = new List<DataModelReference>();
            _interfaceSpecifications = new List<InterfaceSpecification>();
            _businessObjectiveLinks = new List<BusinessObjectiveLink>();

            AddDistributedEvent(new RequirementCreatedEto
            {
                Id = Id,
                Identifier = Identifier.Value,
                Title = Text.Title,
                Type = Type,
                Status = Status,
                TenantId = TenantId,
                Version = Version
            });
        }

        /// <summary>
        /// Updates the requirement text
        /// </summary>
        public void UpdateText(RequirementText text)
        {
            EnsureCanBeModified();
            Text = text ?? throw new ArgumentNullException(nameof(text));
            IncrementVersion();
        }

        /// <summary>
        /// Changes the requirement priority
        /// </summary>
        public void ChangePriority(RequirementPriority priority)
        {
            EnsureCanBeModified();
            Priority = priority;
        }

        /// <summary>
        /// Changes the requirement status
        /// </summary>
        public void ChangeStatus(RequirementStatus newStatus, string reason = null)
        {
            if (!RequirementStatusTransitionPolicy.IsTransitionAllowed(Status, newStatus))
            {
                throw new InvalidRequirementStatusTransitionException(Status, newStatus);
            }

            var previousStatus = Status;
            Status = newStatus;

            AddDistributedEvent(new RequirementStatusChangedEto
            {
                Id = Id,
                Identifier = Identifier.Value,
                Title = Text.Title,
                Type = Type,
                Status = Status,
                PreviousStatus = previousStatus,
                NewStatus = newStatus,
                TenantId = TenantId,
                Reason = reason
            });
        }

        /// <summary>
        /// Adds an acceptance criterion to the requirement
        /// </summary>
        public AcceptanceCriterion AddAcceptanceCriterion(CriterionStatement statement)
        {
            EnsureCanBeModified();
            var criterion = new AcceptanceCriterion(
                GuidGenerator.Create(),
                Id,
                statement,
                _acceptanceCriteria.Count + 1
            );
            _acceptanceCriteria.Add(criterion);
            return criterion;
        }

        /// <summary>
        /// Removes an acceptance criterion
        /// </summary>
        public void RemoveAcceptanceCriterion(Guid criterionId)
        {
            EnsureCanBeModified();
            var criterion = _acceptanceCriteria.FirstOrDefault(c => c.Id == criterionId);
            if (criterion != null)
            {
                _acceptanceCriteria.Remove(criterion);
                ReorderAcceptanceCriteria();
            }
        }

        /// <summary>
        /// Adds a business rule to the requirement
        /// </summary>
        public AssociatedRule AddBusinessRule(RuleDescription rule)
        {
            EnsureCanBeModified();
            var associatedRule = new AssociatedRule(GuidGenerator.Create(), Id, rule);
            _associatedRules.Add(associatedRule);
            return associatedRule;
        }

        /// <summary>
        /// Removes a business rule
        /// </summary>
        public void RemoveBusinessRule(Guid ruleId)
        {
            EnsureCanBeModified();
            var rule = _associatedRules.FirstOrDefault(r => r.Id == ruleId);
            if (rule != null)
            {
                _associatedRules.Remove(rule);
            }
        }

        /// <summary>
        /// Adds a constraint to the requirement
        /// </summary>
        public AssociatedConstraint AddConstraint(ConstraintStatement constraint)
        {
            EnsureCanBeModified();
            var associatedConstraint = new AssociatedConstraint(GuidGenerator.Create(), Id, constraint);
            _associatedConstraints.Add(associatedConstraint);
            return associatedConstraint;
        }

        /// <summary>
        /// Removes a constraint
        /// </summary>
        public void RemoveConstraint(Guid constraintId)
        {
            EnsureCanBeModified();
            var constraint = _associatedConstraints.FirstOrDefault(c => c.Id == constraintId);
            if (constraint != null)
            {
                _associatedConstraints.Remove(constraint);
            }
        }

        /// <summary>
        /// Adds a data model reference
        /// </summary>
        public DataModelReference AddDataModelReference(DataFieldSpec fieldSpec)
        {
            EnsureCanBeModified();
            var reference = new DataModelReference(GuidGenerator.Create(), Id, fieldSpec);
            _dataModelReferences.Add(reference);
            return reference;
        }

        /// <summary>
        /// Removes a data model reference
        /// </summary>
        public void RemoveDataModelReference(Guid referenceId)
        {
            EnsureCanBeModified();
            var reference = _dataModelReferences.FirstOrDefault(r => r.Id == referenceId);
            if (reference != null)
            {
                _dataModelReferences.Remove(reference);
            }
        }

        /// <summary>
        /// Adds an interface specification
        /// </summary>
        public InterfaceSpecification AddInterfaceSpecification(ApiSignature signature)
        {
            EnsureCanBeModified();
            var specification = new InterfaceSpecification(GuidGenerator.Create(), Id, signature);
            _interfaceSpecifications.Add(specification);
            return specification;
        }

        /// <summary>
        /// Removes an interface specification
        /// </summary>
        public void RemoveInterfaceSpecification(Guid specificationId)
        {
            EnsureCanBeModified();
            var specification = _interfaceSpecifications.FirstOrDefault(s => s.Id == specificationId);
            if (specification != null)
            {
                _interfaceSpecifications.Remove(specification);
            }
        }

        /// <summary>
        /// Links the requirement to a business objective
        /// </summary>
        public BusinessObjectiveLink LinkToBusinessObjective(ObjectiveId objectiveId, string justification = null)
        {
            EnsureCanBeModified();
            
            // Check if already linked
            if (_businessObjectiveLinks.Any(l => l.ObjectiveId.Value == objectiveId.Value))
            {
                throw new BusinessException("RequirementSpecificationManagement:ErrorMessage:ObjectiveAlreadyLinked")
                    .WithData("requirementId", Id)
                    .WithData("objectiveId", objectiveId.Value);
            }

            var link = new BusinessObjectiveLink(GuidGenerator.Create(), Id, objectiveId, justification);
            _businessObjectiveLinks.Add(link);
            return link;
        }

        /// <summary>
        /// Removes a business objective link
        /// </summary>
        public void RemoveBusinessObjectiveLink(Guid linkId)
        {
            EnsureCanBeModified();
            var link = _businessObjectiveLinks.FirstOrDefault(l => l.Id == linkId);
            if (link != null)
            {
                _businessObjectiveLinks.Remove(link);
            }
        }

        /// <summary>
        /// Sets the scope status of the requirement
        /// </summary>
        public void SetScopeStatus(ScopeStatus scopeStatus, string justification, DateTime markedAt, Guid markedBy)
        {
            if (_scopeMarker == null)
            {
                _scopeMarker = new RequirementScopeMarker(
                    GuidGenerator.Create(),
                    Id,
                    scopeStatus,
                    justification,
                    markedAt,
                    markedBy
                );
            }
            else
            {
                _scopeMarker.UpdateScope(scopeStatus, justification, markedAt, markedBy);
            }
        }

        /// <summary>
        /// Checks if the requirement can be modified
        /// </summary>
        private void EnsureCanBeModified()
        {
            if (Status == RequirementStatus.Archived || Status == RequirementStatus.Tested)
            {
                throw new RequirementCannotBeModifiedException(Id, Status);
            }
        }

        /// <summary>
        /// Increments the version number
        /// </summary>
        private void IncrementVersion()
        {
            var version = Version?.Split('.') ?? new[] { "1", "0" };
            if (version.Length >= 2 && int.TryParse(version[1], out int minorVersion))
            {
                Version = $"{version[0]}.{minorVersion + 1}";
            }
            else
            {
                Version = "1.1";
            }

            AddDistributedEvent(new RequirementUpdatedEto
            {
                Id = Id,
                Identifier = Identifier.Value,
                Title = Text.Title,
                Type = Type,
                Status = Status,
                TenantId = TenantId,
                PreviousVersion = version.Length >= 2 ? $"{version[0]}.{version[1]}" : "1.0",
                NewVersion = Version
            });
        }

        /// <summary>
        /// Reorders acceptance criteria after removal
        /// </summary>
        private void ReorderAcceptanceCriteria()
        {
            for (int i = 0; i < _acceptanceCriteria.Count; i++)
            {
                _acceptanceCriteria[i].UpdateOrder(i + 1);
            }
        }
    }
}
