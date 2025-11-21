using System;
using NouAssist.RequirementsManagement.QualityAssurance.Enums;
using NouAssist.RequirementsManagement.QualityAssurance.Events.Base;
using Volo.Abp.EventBus;

namespace NouAssist.RequirementsManagement.QualityAssurance.Events
{
    /// <summary>
    /// Event raised when a defect is created
    /// </summary>
    [EventName("NouAssist.RequirementsManagement.QualityAssurance.DefectCreated")]
    public class DefectCreatedEto : QAEtoBase
    {
        /// <summary>
        /// The title of the defect
        /// </summary>
        public string Title { get; set; }
        
        /// <summary>
        /// The severity of the defect
        /// </summary>
        public DefectSeverity Severity { get; set; }
        
        /// <summary>
        /// The priority of the defect
        /// </summary>
        public DefectPriority Priority { get; set; }
        
        /// <summary>
        /// The ID of the test run associated with this defect, if any
        /// </summary>
        public Guid? TestRunId { get; set; }
        
        /// <summary>
        /// The ID of the creator
        /// </summary>
        public Guid CreatorId { get; set; }
        
        /// <summary>
        /// The date when the defect was created
        /// </summary>
        public DateTime CreationTime { get; set; }
    }
}
