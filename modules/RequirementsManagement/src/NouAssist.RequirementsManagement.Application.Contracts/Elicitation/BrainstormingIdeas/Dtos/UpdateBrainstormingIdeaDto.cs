using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NouAssist.RequirementsManagement.Elicitation.BrainstormingIdeas.Dtos
{
    /// <summary>
    /// DTO for updating an existing brainstorming idea
    /// </summary>
    public class UpdateBrainstormingIdeaDto
    {
        [Required]
        [StringLength(1000)]
        public string Description { get; set; } = string.Empty;

        [Range(1, 10)]
        public int Priority { get; set; } = 1;

        public List<CreateIdeaTagDto> Tags { get; set; } = new();
    }
}
