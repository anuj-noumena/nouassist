using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace NouAssist.Infrastructure.Notifications.Preferences.Dtos
{
    public class NotificationPreferenceDto : CreationAuditedEntityDto<Guid>
    {
        public Guid UserId { get; set; }
        public string NotificationType { get; set; }
        public bool IsEnabled { get; set; }
        public List<string> EnabledChannels { get; set; }
        public bool AllowSoundAlerts { get; set; }
        public bool AllowPushNotifications { get; set; }
        public bool AllowEmailNotifications { get; set; }
        public bool AllowSmsNotifications { get; set; }
        public string QuietStartTime { get; set; }
        public string QuietEndTime { get; set; }
        public string CustomSchedule { get; set; }
    }
}
