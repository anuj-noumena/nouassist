using System;
using Volo.Abp.Application.Dtos;

namespace NouAssist.RequirementsManagement.Elicitation.BrainstormingIdeas.Dtos
{
    /// <summary>
    /// DTO for filtering and paging brainstorming idea lists
    /// </summary>
    public class GetBrainstormingIdeaListDto : PagedAndSortedResultRequestDto
    {
        public string? Filter { get; set; }
        public Guid? SessionId { get; set; }
        public Guid? ContributorId { get; set; }
        public int? MinPriority { get; set; }
        public int? MaxPriority { get; set; }
        public bool? IsSelected { get; set; }
        public string? TagName { get; set; }
    }
}
