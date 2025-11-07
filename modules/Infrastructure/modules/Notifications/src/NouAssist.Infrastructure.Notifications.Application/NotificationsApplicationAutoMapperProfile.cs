using AutoMapper;
using NouAssist.Infrastructure.Notifications.Aggregates;
using NouAssist.Infrastructure.Notifications.Entities;
using NouAssist.Infrastructure.Notifications.Notifications.Dtos;

namespace NouAssist.Infrastructure.Notifications
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
