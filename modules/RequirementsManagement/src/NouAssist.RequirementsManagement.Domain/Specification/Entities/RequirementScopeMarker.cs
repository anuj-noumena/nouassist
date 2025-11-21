using System;
using NouAssist.RequirementsManagement.Specification.Enums;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace NouAssist.RequirementsManagement.Specification.Entities
{
    /// <summary>
    /// Represents scope marking for a requirement
    /// </summary>
    public class RequirementScopeMarker : Entity<Guid>
    {
        public Guid RequirementId { get; private set; }
        public ScopeStatus ScopeStatus { get; private set; }
        public string Justification { get; private set; }
        public DateTime MarkedAt { get; private set; }
        public Guid MarkedBy { get; private set; }

        protected RequirementScopeMarker() { }

        internal RequirementScopeMarker(
            Guid id,
            Guid requirementId,
            ScopeStatus scopeStatus,
            string justification,
            DateTime markedAt,
            Guid markedBy) : base(id)
        {
            RequirementId = requirementId;
            ScopeStatus = scopeStatus;
            SetJustification(justification);
            MarkedAt = markedAt;
            MarkedBy = markedBy;
        }

        internal void UpdateScope(ScopeStatus scopeStatus, string justification, DateTime markedAt, Guid markedBy)
        {
            ScopeStatus = scopeStatus;
            SetJustification(justification);
            MarkedAt = markedAt;
            MarkedBy = markedBy;
        }

        private void SetJustification(string justification)
        {
            Justification = Check.Length(justification, nameof(justification), maxLength: 1000);
        }
    }
}
