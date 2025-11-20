using System;
using System.ComponentModel.DataAnnotations;
using NouAssist.RequirementsManagement.Elicitation.Constants;
using NouAssist.RequirementsManagement.Elicitation.Enums;

namespace NouAssist.RequirementsManagement.Elicitation.ElicitationArtifacts.Dtos
{
    /// <summary>
    /// DTO for creating a new elicitation artifact
    /// </summary>
    public class CreateElicitationArtifactDto
    {
        [Required]
        [StringLength(ElicitationConsts.MaxArtifactTitleLength)]
        public string Title { get; set; } = string.Empty;

        [StringLength(ElicitationConsts.MaxArtifactDescriptionLength)]
        public string? Description { get; set; }

        [Required]
        public ArtifactType Type { get; set; }

        [Required]
        public Guid SessionId { get; set; }

        [Required]
        public Guid CreatedByStakeholderId { get; set; }
    }
}
