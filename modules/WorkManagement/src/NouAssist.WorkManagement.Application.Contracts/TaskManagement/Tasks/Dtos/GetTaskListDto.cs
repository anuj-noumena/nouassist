using System;
using Volo.Abp.Application.Dtos;

namespace NouAssist.WorkManagement.TaskManagement.Tasks.Dtos
{
    public class GetTaskListDto : PagedAndSortedResultRequestDto
    {
        public Guid? OwnerId { get; set; }
        public Guid? ProjectId { get; set; }
        public Guid? AreaId { get; set; }
        public Guid? ContextId { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsRecurring { get; set; }
        public string SearchTerm { get; set; }
    }
}