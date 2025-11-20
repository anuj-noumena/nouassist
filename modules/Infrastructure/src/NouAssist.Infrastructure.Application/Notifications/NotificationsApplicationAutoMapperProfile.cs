using AutoMapper;
using NouAssist.Infrastructure.Notifications.Aggregates;
using NouAssist.Infrastructure.Notifications.Entities;
using NouAssist.Notification.Notifications.Notifications.Dtos;
using NouAssist.Notification.Notifications.Preferences.Dtos;

namespace NouAssist.Infrastructure.Notifications
{
    public class NotificationsApplicationAutoMapperProfile : Profile
    {
        public NotificationsApplicationAutoMapperProfile()
        {
            CreateMap<Aggregates.Notification, NotificationDto>();
            CreateMap<NotificationDeliveryLogEntry, NotificationDeliveryLogDto>();
            CreateMap<NotificationPreference, NotificationPreferenceDto>();
            CreateMap<NotificationPreference, NotificationPreferenceDto>();
        }
    }
}
