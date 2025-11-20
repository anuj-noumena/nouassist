using System.ComponentModel.DataAnnotations;
using NouAssist.RequirementsManagement.ChangeManagement.Constants;

namespace NouAssist.RequirementsManagement.ChangeManagement.ChangeRequest.Dtos
{
    /// <summary>
    /// Input DTO for updating a change request
    /// </summary>
    public class UpdateChangeRequestDto
    {
        [Required]
        [StringLength(ChangeManagementConsts.MaxTitleLength)]
        public string Title { get; set; }

        [Required]
        [StringLength(ChangeManagementConsts.MaxDescriptionLength)]
        public string Description { get; set; }
    }
}
