using System;
using NouAssist.RequirementsManagement.ChangeManagement.Constants;
using NouAssist.RequirementsManagement.ChangeManagement.ValueObjects;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace NouAssist.RequirementsManagement.ChangeManagement.Entities
{
    /// <summary>
    /// Represents an impact statement describing the effects of a change request
    /// </summary>
    public class ChangeImpactStatement : AuditedEntity<Guid>
    {
        public Guid ChangeRequestId { get; private set; }
        public ImpactDetails ImpactDetails { get; private set; }
        public string Description { get; private set; }
        public DateTime AssessmentDate { get; private set; }

        protected ChangeImpactStatement() { }

        internal ChangeImpactStatement(
            Guid changeRequestId,
            ImpactDetails impactDetails,
            string description)
        {
            Id = Guid.NewGuid();
            ChangeRequestId = Check.NotEmpty(changeRequestId, nameof(changeRequestId));
            ImpactDetails = Check.NotNull(impactDetails, nameof(impactDetails));
            SetDescription(description);
            AssessmentDate = DateTime.UtcNow;
        }

        internal void UpdateImpactDetails(ImpactDetails newImpactDetails, string description)
        {
            ImpactDetails = Check.NotNull(newImpactDetails, nameof(newImpactDetails));
            SetDescription(description);
        }

        private void SetDescription(string description)
        {
            Description = Check.NotNullOrWhiteSpace(
                description,
                nameof(description),
                maxLength: ChangeManagementConsts.MaxDescriptionLength
            );
        }
    }
}
