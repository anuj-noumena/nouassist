using AutoMapper;
using NouAssist.Notifications.Aggregates;
using NouAssist.Notifications.Entities;
using NouAssist.Notifications.Notifications.Dtos;

namespace NouAssist.Notifications
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
