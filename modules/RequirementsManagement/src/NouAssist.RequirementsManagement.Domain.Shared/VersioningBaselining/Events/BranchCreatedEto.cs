using System;
using NouAssist.RequirementsManagement.VersioningBaselining.Events.Base;
using Volo.Abp.EventBus;

namespace NouAssist.RequirementsManagement.VersioningBaselining.Events
{
    /// <summary>
    /// Event triggered when a new branch is created
    /// </summary>
    [EventName("NouAssist.RequirementsManagement.VersioningBaselining.BranchCreated")]
    public class BranchCreatedEto : VersionControlEtoBase
    {
        /// <summary>
        /// ID of the branch
        /// </summary>
        public Guid BranchId { get; set; }
        
        /// <summary>
        /// Name of the branch
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// ID of the baseline or branch from which this branch was created
        /// </summary>
        public Guid SourceId { get; set; }
        
        /// <summary>
        /// Indicates whether the source is a baseline or branch
        /// </summary>
        public string SourceType { get; set; }
        
        /// <summary>
        /// ID of the user who created the branch
        /// </summary>
        public Guid CreatorId { get; set; }
        
        /// <summary>
        /// Timestamp when the branch was created
        /// </summary>
        public DateTime CreationTime { get; set; }
    }
}
