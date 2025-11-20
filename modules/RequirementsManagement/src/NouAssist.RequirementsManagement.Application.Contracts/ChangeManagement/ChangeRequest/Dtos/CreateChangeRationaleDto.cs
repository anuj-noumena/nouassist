using System.ComponentModel.DataAnnotations;
using NouAssist.RequirementsManagement.ChangeManagement.Constants;

namespace NouAssist.RequirementsManagement.ChangeManagement.ChangeRequest.Dtos
{
    /// <summary>
    /// Input DTO for creating a change rationale
    /// </summary>
    public class CreateChangeRationaleDto
    {
        [Required]
        [StringLength(ChangeManagementConsts.MaxRationaleLength)]
        public string RationaleText { get; set; }

        [StringLength(100)]
        public string Category { get; set; }
    }
}
