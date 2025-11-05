using System;
using System.Threading.Tasks;
using NouAssist.Notifications.Notifications.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.Notifications.Notifications
{
    public interface INotificationAppService : IApplicationService
    {
        Task<NotificationDto> GetAsync(Guid id);
        Task<PagedResultDto<NotificationDto>> GetListAsync(GetNotificationListDto input);
        Task<NotificationDto> CreateAsync(CreateNotificationDto input);
        Task<int> GetUnreadCountAsync();
        Task MarkAsReadAsync(Guid id);
        Task DismissAsync(Guid id);
        Task DeleteExpiredNotificationsAsync();
    }
}
