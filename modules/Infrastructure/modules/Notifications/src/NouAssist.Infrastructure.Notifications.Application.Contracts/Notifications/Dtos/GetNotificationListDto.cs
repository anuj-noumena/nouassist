using System;
using Volo.Abp.Application.Dtos;

namespace NouAssist.Infrastructure.Notifications.Notifications.Dtos
{
    public class GetNotificationListDto : PagedAndSortedResultRequestDto
    {
        public bool? IsRead { get; set; }
        public string Priority { get; set; }
        public string NotificationType { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}
