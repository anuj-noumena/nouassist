using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using NouAssist.RequirementsManagement.ChangeManagement.Constants;

namespace NouAssist.RequirementsManagement.ChangeManagement.ChangeRequest.Dtos
{
    /// <summary>
    /// Input DTO for creating a change request
    /// </summary>
    public class CreateChangeRequestDto
    {
        [Required]
        [StringLength(ChangeManagementConsts.MaxTitleLength)]
        public string Title { get; set; }

        [Required]
        [StringLength(ChangeManagementConsts.MaxDescriptionLength)]
        public string Description { get; set; }

        [Required]
        public Guid AffectedRequirementId { get; set; }

        [Required]
        public string AffectedRequirementVersion { get; set; }

        public List<CreateChangeRationaleDto> Rationales { get; set; } = new();
    }
}
