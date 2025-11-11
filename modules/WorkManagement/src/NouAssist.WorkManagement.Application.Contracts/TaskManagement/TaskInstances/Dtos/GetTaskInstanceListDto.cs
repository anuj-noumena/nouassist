using System;
using NouAssist.WorkManagement.Shared.Enums;
using NouAssist.WorkManagement.TaskManagement.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.WorkManagement.TaskManagement.TaskInstances.Dtos
{
    public class GetTaskInstanceListDto : PagedAndSortedResultRequestDto
    {
        public Guid? ProjectId { get; set; }
        public Guid? AssigneeId { get; set; }
        public Guid? OwnerId { get; set; }
        public WorkflowStatusCategory? Status { get; set; }
        public bool? IsBlocked { get; set; }
        public bool? IsNextAction { get; set; }
        public bool? IsOverdue { get; set; }
        public EisenhowerQuadrant? Quadrant { get; set; }
        public DateTime? DueDateFrom { get; set; }
        public DateTime? DueDateTo { get; set; }
        public string SearchTerm { get; set; }
    }
}
