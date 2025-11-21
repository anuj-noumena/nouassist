using NouAssist.Infrastructure.Notifications.Aggregates;
using NouAssist.Infrastructure.Notifications.Entities;
using NouAssist.Notification.Notifications.Notifications.Dtos;
using NouAssist.Notification.Notifications.Preferences.Dtos;
using Riok.Mapperly.Abstractions;
using Volo.Abp.Mapperly;

namespace NouAssist.Infrastructure.Notifications
{
    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class NotificationToNotificationDtoMapper : MapperBase<Aggregates.Notification, NotificationDto>
    {
        public override partial NotificationDto Map(Aggregates.Notification source);

        public override partial void Map(Aggregates.Notification source, NotificationDto destination);
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class NotificationDeliveryLogEntryToNotificationDeliveryLogDtoMapper : MapperBase<NotificationDeliveryLogEntry, NotificationDeliveryLogDto>
    {
        public override partial NotificationDeliveryLogDto Map(NotificationDeliveryLogEntry source);

        public override partial void Map(NotificationDeliveryLogEntry source, NotificationDeliveryLogDto destination);
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class NotificationPreferenceToNotificationPreferenceDtoMapper : MapperBase<NotificationPreference, NotificationPreferenceDto>
    {
        [MapperIgnoreTarget(nameof(NotificationPreferenceDto.IsEnabled))]
        [MapperIgnoreTarget(nameof(NotificationPreferenceDto.EnabledChannels))]
        [MapperIgnoreTarget(nameof(NotificationPreferenceDto.AllowSoundAlerts))]
        [MapperIgnoreTarget(nameof(NotificationPreferenceDto.AllowPushNotifications))]
        [MapperIgnoreTarget(nameof(NotificationPreferenceDto.AllowEmailNotifications))]
        [MapperIgnoreTarget(nameof(NotificationPreferenceDto.AllowSmsNotifications))]
        [MapperIgnoreTarget(nameof(NotificationPreferenceDto.QuietStartTime))]
        [MapperIgnoreTarget(nameof(NotificationPreferenceDto.QuietEndTime))]
        public override partial NotificationPreferenceDto Map(NotificationPreference source);

        [MapperIgnoreTarget(nameof(NotificationPreferenceDto.IsEnabled))]
        [MapperIgnoreTarget(nameof(NotificationPreferenceDto.EnabledChannels))]
        [MapperIgnoreTarget(nameof(NotificationPreferenceDto.AllowSoundAlerts))]
        [MapperIgnoreTarget(nameof(NotificationPreferenceDto.AllowPushNotifications))]
        [MapperIgnoreTarget(nameof(NotificationPreferenceDto.AllowEmailNotifications))]
        [MapperIgnoreTarget(nameof(NotificationPreferenceDto.AllowSmsNotifications))]
        [MapperIgnoreTarget(nameof(NotificationPreferenceDto.QuietStartTime))]
        [MapperIgnoreTarget(nameof(NotificationPreferenceDto.QuietEndTime))]
        public override partial void Map(NotificationPreference source, NotificationPreferenceDto destination);
    }
}
