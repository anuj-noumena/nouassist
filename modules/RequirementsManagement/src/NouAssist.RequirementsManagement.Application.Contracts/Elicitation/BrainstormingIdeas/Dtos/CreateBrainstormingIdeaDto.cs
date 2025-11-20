using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NouAssist.RequirementsManagement.Elicitation.BrainstormingIdeas.Dtos
{
    /// <summary>
    /// DTO for creating a new brainstorming idea
    /// </summary>
    public class CreateBrainstormingIdeaDto
    {
        [Required]
        public Guid SessionId { get; set; }

        [Required]
        public Guid ContributorId { get; set; }

        [Required]
        [StringLength(1000)]
        public string Description { get; set; } = string.Empty;

        [Range(1, 10)]
        public int Priority { get; set; } = 1;

        public List<CreateIdeaTagDto> Tags { get; set; } = new();
    }

    /// <summary>
    /// DTO for creating an idea tag
    /// </summary>
    public class CreateIdeaTagDto
    {
        [Required]
        [StringLength(30)]
        public string Name { get; set; } = string.Empty;

        [StringLength(7)]
        [RegularExpression(@"^#[0-9A-Fa-f]{6}$")]
        public string Color { get; set; } = "#007ACC";

        [StringLength(100)]
        public string? Description { get; set; }
    }
}
