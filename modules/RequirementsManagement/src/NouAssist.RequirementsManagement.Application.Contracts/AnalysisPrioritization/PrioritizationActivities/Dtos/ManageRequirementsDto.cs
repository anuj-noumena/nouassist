using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NouAssist.RequirementsManagement.AnalysisPrioritization.PrioritizationActivities.Dtos
{
    /// <summary>
    /// DTO for managing activity requirements
    /// </summary>
    public class ManageRequirementsDto
    {
        [Required]
        public List<Guid> RequirementIds { get; set; } = new();
    }
}
