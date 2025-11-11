using System;
using NouAssist.CoreProductivity.ProjectManagement.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.CoreProductivity.ProjectManagement.Projects.Dtos
{
    public class GetProjectListDto : PagedAndSortedResultRequestDto
    {
        public Guid? OwnerId { get; set; }
        public ProjectStatus? Status { get; set; }
        public EisenhowerQuadrant? Quadrant { get; set; }
        public bool? IsArchived { get; set; }
        public bool? IsOverdue { get; set; }
        public DateTime? DueAfter { get; set; }
        public DateTime? DueBefore { get; set; }
        public string Filter { get; set; }
    }
}
