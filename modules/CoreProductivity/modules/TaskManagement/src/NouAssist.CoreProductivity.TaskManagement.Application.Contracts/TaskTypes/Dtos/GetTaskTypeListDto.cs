using Volo.Abp.Application.Dtos;

namespace NouAssist.Tasks.TaskTypes.Dtos
{
    public class GetTaskTypeListDto : PagedAndSortedResultRequestDto
    {
        public bool? IsActive { get; set; }
        public string SearchTerm { get; set; }
    }
}
