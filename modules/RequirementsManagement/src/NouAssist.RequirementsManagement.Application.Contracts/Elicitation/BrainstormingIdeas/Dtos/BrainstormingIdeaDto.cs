using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace NouAssist.RequirementsManagement.Elicitation.BrainstormingIdeas.Dtos
{
    /// <summary>
    /// DTO representing a brainstorming idea
    /// </summary>
    public class BrainstormingIdeaDto : EntityDto<Guid>
    {
        public Guid SessionId { get; set; }
        public string? SessionTitle { get; set; }
        public Guid ContributorId { get; set; }
        public string? ContributorName { get; set; }
        public string Description { get; set; } = string.Empty;
        public int Priority { get; set; }
        public bool IsSelected { get; set; }
        public List<IdeaTagDto> Tags { get; set; } = new();
    }

    /// <summary>
    /// DTO for idea tag
    /// </summary>
    public class IdeaTagDto
    {
        public string Name { get; set; } = string.Empty;
        public string Color { get; set; } = "#007ACC";
        public string? Description { get; set; }
    }
}
