using System;
using NouAssist.RequirementsManagement.Specification.ValueObjects;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace NouAssist.RequirementsManagement.Specification.Entities
{
    /// <summary>
    /// Represents a link between a requirement and a business objective
    /// </summary>
    public class BusinessObjectiveLink : Entity<Guid>
    {
        public Guid RequirementId { get; private set; }
        public ObjectiveId ObjectiveId { get; private set; }
        public string Justification { get; private set; }

        protected BusinessObjectiveLink() { }

        internal BusinessObjectiveLink(
            Guid id,
            Guid requirementId,
            ObjectiveId objectiveId,
            string justification = null) : base(id)
        {
            RequirementId = requirementId;
            ObjectiveId = objectiveId ?? throw new ArgumentNullException(nameof(objectiveId));
            SetJustification(justification);
        }

        internal void UpdateObjective(ObjectiveId objectiveId, string justification = null)
        {
            ObjectiveId = objectiveId ?? throw new ArgumentNullException(nameof(objectiveId));
            SetJustification(justification);
        }

        private void SetJustification(string justification)
        {
            Justification = Check.Length(justification, nameof(justification), maxLength: 1000);
        }
    }
}
