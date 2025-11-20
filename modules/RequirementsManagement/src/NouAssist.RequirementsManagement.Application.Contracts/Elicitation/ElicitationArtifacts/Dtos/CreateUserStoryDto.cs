using System.ComponentModel.DataAnnotations;

namespace NouAssist.RequirementsManagement.Elicitation.ElicitationArtifacts.Dtos
{
    /// <summary>
    /// DTO for creating user story content
    /// </summary>
    public class CreateUserStoryDto : CreateElicitationArtifactDto
    {
        [Required]
        public CreateStoryTextDto StoryText { get; set; } = new();
    }

    /// <summary>
    /// DTO for creating story text
    /// </summary>
    public class CreateStoryTextDto
    {
        [Required]
        [StringLength(100)]
        public string Persona { get; set; } = string.Empty;

        [Required]
        [StringLength(200)]
        public string Goal { get; set; } = string.Empty;

        [Required]
        [StringLength(200)]
        public string Benefit { get; set; } = string.Empty;
    }
}
