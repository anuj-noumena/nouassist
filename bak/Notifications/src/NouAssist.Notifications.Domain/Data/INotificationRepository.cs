using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.Notifications.Aggregates;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Specifications;

namespace NouAssist.Notifications.Data
{
    public interface INotificationRepository : IRepository<Notification, Guid>
    {
        Task<Notification> GetWithDeliveryLogsAsync(Guid id);
        Task<List<Notification>> GetListAsync(ISpecification<Notification> spec, int maxResultCount = 50);
        Task<int> GetUnreadCountAsync(Guid userId);
        Task<List<Notification>> GetExpiredNotificationsAsync();
    }
}
