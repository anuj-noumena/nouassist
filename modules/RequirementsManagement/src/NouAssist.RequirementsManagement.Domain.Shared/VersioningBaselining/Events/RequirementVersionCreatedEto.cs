using System;
using NouAssist.RequirementsManagement.VersioningBaselining.Events.Base;
using Volo.Abp.EventBus;

namespace NouAssist.RequirementsManagement.VersioningBaselining.Events
{
    /// <summary>
    /// Event triggered when a new requirement version is created
    /// </summary>
    [EventName("NouAssist.RequirementsManagement.VersioningBaselining.RequirementVersionCreated")]
    public class RequirementVersionCreatedEto : VersionControlEtoBase
    {
        /// <summary>
        /// ID of the requirement
        /// </summary>
        public Guid RequirementId { get; set; }
        
        /// <summary>
        /// Version identifier of the new version
        /// </summary>
        public string Version { get; set; }
        
        /// <summary>
        /// ID of the user who created the version
        /// </summary>
        public Guid AuthorId { get; set; }
        
        /// <summary>
        /// Name of the author
        /// </summary>
        public string AuthorName { get; set; }
        
        /// <summary>
        /// Comment describing the changes in this version
        /// </summary>
        public string ChangeComment { get; set; }
        
        /// <summary>
        /// Timestamp when the version was created
        /// </summary>
        public DateTime CreationTime { get; set; }
    }
}
