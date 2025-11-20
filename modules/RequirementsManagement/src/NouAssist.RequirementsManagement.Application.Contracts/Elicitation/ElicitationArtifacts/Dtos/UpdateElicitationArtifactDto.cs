using System.ComponentModel.DataAnnotations;
using NouAssist.RequirementsManagement.Elicitation.Constants;

namespace NouAssist.RequirementsManagement.Elicitation.ElicitationArtifacts.Dtos
{
    /// <summary>
    /// DTO for updating an existing elicitation artifact
    /// </summary>
    public class UpdateElicitationArtifactDto
    {
        [Required]
        [StringLength(ElicitationConsts.MaxArtifactTitleLength)]
        public string Title { get; set; } = string.Empty;

        [StringLength(ElicitationConsts.MaxArtifactDescriptionLength)]
        public string? Description { get; set; }
    }
}
