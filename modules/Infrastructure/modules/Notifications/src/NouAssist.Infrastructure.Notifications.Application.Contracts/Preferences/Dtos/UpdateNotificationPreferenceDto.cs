using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NouAssist.Infrastructure.Notifications.Preferences.Dtos
{
    public class UpdateNotificationPreferenceDto
    {
        [Required]
        public bool IsEnabled { get; set; }

        public List<string> EnabledChannels { get; set; }
        public bool AllowSoundAlerts { get; set; }
        public bool AllowPushNotifications { get; set; }
        public bool AllowEmailNotifications { get; set; }
        public bool AllowSmsNotifications { get; set; }
        public string QuietStartTime { get; set; }
        public string QuietEndTime { get; set; }
    }
}
