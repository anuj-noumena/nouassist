using System;
using System.Collections.Generic;

namespace NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.UserProfiles.Dtos
{
    /// <summary>
    /// DTO for task context
    /// </summary>
    public class TaskContextDto
    {
        /// <summary>
        /// Project ID
        /// </summary>
        public Guid? ProjectId { get; set; }
        
        /// <summary>
        /// Team ID
        /// </summary>
        public Guid? TeamId { get; set; }
        
        /// <summary>
        /// Tags for the task
        /// </summary>
        public List<string> Tags { get; set; }
        
        /// <summary>
        /// Location context
        /// </summary>
        public string Location { get; set; }
        
        /// <summary>
        /// Energy level required (High, Medium, Low)
        /// </summary>
        public string EnergyLevel { get; set; }
        
        /// <summary>
        /// Focus level required (High, Medium, Low)
        /// </summary>
        public string FocusLevel { get; set; }
    }
}
