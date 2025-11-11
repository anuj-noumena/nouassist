using Volo.Abp.Application.Dtos;

namespace NouAssist.WorkManagement.Workflow.Workflows.Dtos
{
    public class GetWorkflowListDto : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDefault { get; set; }
    }
}
