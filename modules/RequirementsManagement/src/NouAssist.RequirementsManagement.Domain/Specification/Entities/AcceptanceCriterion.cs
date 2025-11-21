using System;
using NouAssist.RequirementsManagement.Specification.ValueObjects;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace NouAssist.RequirementsManagement.Specification.Entities
{
    /// <summary>
    /// Represents an acceptance criterion for a requirement
    /// </summary>
    public class AcceptanceCriterion : Entity<Guid>
    {
        public Guid RequirementId { get; private set; }
        public CriterionStatement Statement { get; private set; }
        public int Order { get; private set; }

        protected AcceptanceCriterion() { }

        internal AcceptanceCriterion(
            Guid id,
            Guid requirementId,
            CriterionStatement statement,
            int order) : base(id)
        {
            RequirementId = requirementId;
            Statement = statement ?? throw new ArgumentNullException(nameof(statement));
            Order = order;
        }

        internal void UpdateStatement(CriterionStatement statement)
        {
            Statement = statement ?? throw new ArgumentNullException(nameof(statement));
        }

        internal void UpdateOrder(int order)
        {
            if (order < 0)
            {
                throw new BusinessException("RequirementSpecificationManagement:ErrorMessage:InvalidCriterionOrder");
            }

            Order = order;
        }
    }
}
