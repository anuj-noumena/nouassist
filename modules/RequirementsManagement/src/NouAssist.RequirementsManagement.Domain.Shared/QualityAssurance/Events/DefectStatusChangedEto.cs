using System;
using NouAssist.RequirementsManagement.QualityAssurance.Enums;
using NouAssist.RequirementsManagement.QualityAssurance.Events.Base;
using Volo.Abp.EventBus;

namespace NouAssist.RequirementsManagement.QualityAssurance.Events
{
    /// <summary>
    /// Event raised when a defect status is changed
    /// </summary>
    [EventName("NouAssist.RequirementsManagement.QualityAssurance.DefectStatusChanged")]
    public class DefectStatusChangedEto : QAEtoBase
    {
        /// <summary>
        /// The title of the defect
        /// </summary>
        public string Title { get; set; }
        
        /// <summary>
        /// The previous status of the defect
        /// </summary>
        public DefectStatus PreviousStatus { get; set; }
        
        /// <summary>
        /// The new status of the defect
        /// </summary>
        public DefectStatus NewStatus { get; set; }
        
        /// <summary>
        /// The ID of the user who changed the status
        /// </summary>
        public Guid ChangedById { get; set; }
        
        /// <summary>
        /// The time when the status was changed
        /// </summary>
        public DateTime ChangeTime { get; set; }
    }
}
