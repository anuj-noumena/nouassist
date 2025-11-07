using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using NouAssist.Infrastructure.Notifications.Aggregates;
using NouAssist.Infrastructure.Notifications.Data;
using NouAssist.Infrastructure.Notifications.Enums;
using NouAssist.Infrastructure.Notifications.Notifications.Dtos;
using NouAssist.Infrastructure.Notifications.Permissions;
using NouAssist.Infrastructure.Notifications.Services;
using NouAssist.Infrastructure.Notifications.Specifications;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.Infrastructure.Notifications.Notifications
{
    [Authorize(NotificationsPermissions.Notifications.Default)]
    public class NotificationAppService : ApplicationService, INotificationAppService
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly NotificationDeliveryService _deliveryService;

        public NotificationAppService(
            INotificationRepository notificationRepository,
            NotificationDeliveryService deliveryService)
        {
            _notificationRepository = notificationRepository;
            _deliveryService = deliveryService;
        }

        public async Task<NotificationDto> GetAsync(Guid id)
        {
            var notification = await _notificationRepository.GetWithDeliveryLogsAsync(id);
            return ObjectMapper.Map<Notification, NotificationDto>(notification);
        }

        public async Task<PagedResultDto<NotificationDto>> GetListAsync(GetNotificationListDto input)
        {
            var spec = CreateListSpecification(input);
            
            var notifications = await _notificationRepository.GetListAsync(spec, input.MaxResultCount);
            var totalCount = await _notificationRepository.CountAsync(spec.ToExpression());

            return new PagedResultDto<NotificationDto>(
                totalCount,
                ObjectMapper.Map<List<Notification>, List<NotificationDto>>(notifications)
            );
        }

        [Authorize(NotificationsPermissions.Notifications.Manage)]
        public async Task<NotificationDto> CreateAsync(CreateNotificationDto input)
        {
            // Create notification aggregate
            var notification = new Notification(
                GuidGenerator.Create(),
                input.UserId,
                input.NotificationType,
                input.Title,
                CurrentTenant.Id,
                input.Message,
                input.Priority,
                input.ExpiresInDays ?? 30
            );

            if (!string.IsNullOrWhiteSpace(input.EntityType) && input.EntityId.HasValue)
            {
                notification.SetEntity(input.EntityType, input.EntityId.Value);
            }

            if (!string.IsNullOrWhiteSpace(input.ActionUrl))
            {
                notification.SetAction(input.ActionUrl, input.ActionLabel);
            }

            if (input.ActorId.HasValue)
            {
                notification.SetActor(input.ActorId.Value);
            }

            // Get delivery channels from domain service
            var channels = await _deliveryService.GetEnabledChannelsAsync(input.UserId, input.NotificationType);
            
            // Prepare notification for delivery
            _deliveryService.PrepareNotificationForDelivery(notification, channels);

            await _notificationRepository.InsertAsync(notification);

            return ObjectMapper.Map<Notification, NotificationDto>(notification);
        }

        public async Task<int> GetUnreadCountAsync()
        {
            return await _notificationRepository.GetUnreadCountAsync(CurrentUser.Id.Value);
        }

        public async Task MarkAsReadAsync(Guid id)
        {
            var notification = await _notificationRepository.GetAsync(id);
            notification.MarkAsRead();
            await _notificationRepository.UpdateAsync(notification);
        }

        public async Task DismissAsync(Guid id)
        {
            var notification = await _notificationRepository.GetAsync(id);
            notification.Dismiss();
            await _notificationRepository.UpdateAsync(notification);
        }

        [Authorize(NotificationsPermissions.Notifications.Manage)]
        public async Task DeleteExpiredNotificationsAsync()
        {
            var expiredNotifications = await _notificationRepository.GetExpiredNotificationsAsync();
            
            foreach (var notification in expiredNotifications)
            {
                await _notificationRepository.DeleteAsync(notification);
            }
        }

        private ISpecification<Notification> CreateListSpecification(GetNotificationListDto input)
        {
            var spec = new Specification<Notification>(n => n.UserId == CurrentUser.Id.Value);

            if (input.IsRead.HasValue)
            {
                spec = spec.And(new Specification<Notification>(n => n.IsRead == input.IsRead.Value));
            }

            if (!string.IsNullOrWhiteSpace(input.Priority))
            {
                var priority = Enum.Parse<NotificationPriority>(input.Priority);
                spec = spec.And(new Specification<Notification>(n => n.Priority == priority));
            }

            if (!string.IsNullOrWhiteSpace(input.NotificationType))
            {
                var notifType = Enum.Parse<NotificationType>(input.NotificationType);
                spec = spec.And(new Specification<Notification>(n => n.NotificationType == notifType));
            }

            if (input.FromDate.HasValue)
            {
                spec = spec.And(new Specification<Notification>(n => n.CreationTime >= input.FromDate.Value));
            }

            if (input.ToDate.HasValue)
            {
                spec = spec.And(new Specification<Notification>(n => n.CreationTime <= input.ToDate.Value));
            }

            return spec;
        }
    }
}
