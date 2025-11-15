using System;
using NouAssist.WorkManagement.ProjectManagement.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.WorkManagement.ProjectManagement.Projects.Dtos
{
    public class GetProjectListDto : PagedAndSortedResultRequestDto
    {
        public Guid? OwnerId { get; set; }
        public Guid? AreaId { get; set; }
        public Guid? GoalId { get; set; }
        public ProjectStatus? Status { get; set; }
        public bool? IsArchived { get; set; }
        public bool? IsOverdue { get; set; }
        public ProjectHealth? Health { get; set; }
        public DateTime? PlannedDueAfter { get; set; }
        public DateTime? PlannedDueBefore { get; set; }
        public DateTime? PlannedStartAfter { get; set; }
        public DateTime? PlannedStartBefore { get; set; }
        public string Filter { get; set; }
    }
}