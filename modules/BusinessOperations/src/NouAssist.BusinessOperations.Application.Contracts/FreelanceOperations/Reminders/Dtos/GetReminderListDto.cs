using Volo.Abp.Application.Dtos;

namespace NouAssist.BusinessOperations.FreelanceOperations.Reminders.Dtos
{
    public class GetReminderListDto : PagedAndSortedResultRequestDto
    {
        public bool? IsCompleted { get; set; }
        public bool? IsOverdue { get; set; }
        public string EntityType { get; set; }
    }
}
