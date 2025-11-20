using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Specifications;

namespace NouAssist.Infrastructure.Notifications.Data
{
    public interface INotificationRepository : IRepository<Aggregates.Notification, Guid>
    {
        Task<Aggregates.Notification> GetWithDeliveryLogsAsync(Guid id);
        Task<List<Aggregates.Notification>> GetListAsync(ISpecification<Aggregates.Notification> spec, int maxResultCount = 50);
        Task<int> GetUnreadCountAsync(Guid userId);
        Task<List<Aggregates.Notification>> GetExpiredNotificationsAsync();
    }
}
