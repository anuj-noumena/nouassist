using System.ComponentModel.DataAnnotations;
using NouAssist.RequirementsManagement.Specification.Enums;

namespace NouAssist.RequirementsManagement.Specification.Requirements.Dtos
{
    /// <summary>
    /// DTO for changing requirement status
    /// </summary>
    public class ChangeRequirementStatusDto
    {
        [Required]
        public RequirementStatus Status { get; set; }

        [StringLength(500)]
        public string Reason { get; set; }
    }
}
