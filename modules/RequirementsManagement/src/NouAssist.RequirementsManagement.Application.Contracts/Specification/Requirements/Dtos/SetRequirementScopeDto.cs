using System.ComponentModel.DataAnnotations;
using NouAssist.RequirementsManagement.Specification.Enums;

namespace NouAssist.RequirementsManagement.Specification.Requirements.Dtos
{
    /// <summary>
    /// DTO for setting requirement scope
    /// </summary>
    public class SetRequirementScopeDto
    {
        [Required]
        public ScopeStatus ScopeStatus { get; set; }

        [StringLength(1000)]
        public string Justification { get; set; }
    }
}
