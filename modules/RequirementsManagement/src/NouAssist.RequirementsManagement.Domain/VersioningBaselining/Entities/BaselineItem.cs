using System;
using NouAssist.RequirementsManagement.VersioningBaselining.ValueObjects;
using Volo.Abp.Domain.Entities;

namespace NouAssist.RequirementsManagement.VersioningBaselining.Entities
{
    /// <summary>
    /// Entity representing a requirement version included in a baseline
    /// </summary>
    public class BaselineItem : Entity
    {
        /// <summary>
        /// ID of the baseline this item belongs to
        /// </summary>
        public Guid BaselineId { get; private set; }
        
        /// <summary>
        /// ID of the requirement
        /// </summary>
        public Guid RequirementId { get; private set; }
        
        /// <summary>
        /// Version identifier of the requirement
        /// </summary>
        public VersionIdentifier Version { get; private set; }
        
        /// <summary>
        /// Timestamp when this item was added to the baseline
        /// </summary>
        public DateTime AddedAt { get; private set; }

        protected BaselineItem() { }

        internal BaselineItem(
            Guid baselineId,
            Guid requirementId,
            VersionIdentifier version,
            DateTime addedAt)
        {
            BaselineId = baselineId;
            RequirementId = requirementId;
            Version = version;
            AddedAt = addedAt;
        }

        public override object[] GetKeys()
        {
            return new object[] { BaselineId, RequirementId };
        }
    }
}
