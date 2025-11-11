using AutoMapper;
using NouAssist.Notification.Notifications.Aggregates;
using NouAssist.Notification.Notifications.Entities;
using NouAssist.Notification.Notifications.Notifications.Dtos;

namespace NouAssist.Notification.Notifications
{
    public class NotificationsApplicationAutoMapperProfile : Profile
    {
        public NotificationsApplicationAutoMapperProfile()
        {
            CreateMap<Notification, NotificationDto>();
            CreateMap<NotificationDeliveryLogEntry, NotificationDeliveryLogDto>();
            CreateMap<NotificationPreference, NotificationPreferenceDto>();
            CreateMap<NotificationPreference, NotificationPreferenceDto>();
        }
    }
}
