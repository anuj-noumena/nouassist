using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NouAssist.RequirementsManagement.ValidationVerification.TestCases.Dtos
{
    /// <summary>
    /// Input DTO for updating requirement links
    /// </summary>
    public class UpdateRequirementLinksDto
    {
        [Required]
        [MinLength(1, ErrorMessage = "At least one requirement must be linked")]
        public List<Guid> RequirementIds { get; set; } = new();
    }
}
