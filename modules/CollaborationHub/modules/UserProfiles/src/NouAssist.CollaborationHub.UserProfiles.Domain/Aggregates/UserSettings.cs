using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace NouAssist.CollaborationHub.UserProfiles.Aggregates
{
    public class UserSettings : FullAuditedAggregateRoot<Guid>
    {
        public Guid UserId { get; private set; }
        public string? Theme { get; set; }
        public string? Language { get; set; }
        public string? Timezone { get; set; }
        public string? DateFormat { get; set; }
        public string? TimeFormat { get; set; }
        public string WeekStartDay { get; set; }
        public int DefaultPomodoroMinutes { get; private set; }
        public int DefaultShortBreakMinutes { get; private set; }
        public int DefaultLongBreakMinutes { get; private set; }
        public bool EnableNotifications { get; set; }
        public bool EnableSounds { get; set; }
        public bool EnableDarkMode { get; set; }
        public string? CustomSettingsJson { get; set; }

        protected UserSettings() { }

        public UserSettings(Guid id, Guid userId) : base(id)
        {
            UserId = userId;
            WeekStartDay = "sunday";
            DefaultPomodoroMinutes = 25;
            DefaultShortBreakMinutes = 5;
            DefaultLongBreakMinutes = 15;
            EnableNotifications = true;
            EnableSounds = true;
            EnableDarkMode = false;
        }

        public void UpdateTheme(string theme)
        {
            Theme = theme;
        }

        public void ToggleDarkMode()
        {
            EnableDarkMode = !EnableDarkMode;
        }

        public void UpdatePomodoroSettings(int? pomodoro, int? shortBreak, int? longBreak)
        {
            if (pomodoro.HasValue)
            {
                if (pomodoro.Value < 1 || pomodoro.Value > 60)
                    throw new BusinessException("Users:InvalidPomodoroTime", 
                        "Pomodoro time must be between 1 and 60 minutes.");
                DefaultPomodoroMinutes = pomodoro.Value;
            }

            if (shortBreak.HasValue)
            {
                if (shortBreak.Value < 1 || shortBreak.Value > 30)
                    throw new BusinessException("Users:InvalidBreakTime", 
                        "Short break must be between 1 and 30 minutes.");
                DefaultShortBreakMinutes = shortBreak.Value;
            }

            if (longBreak.HasValue)
            {
                if (longBreak.Value < 1 || longBreak.Value > 60)
                    throw new BusinessException("Users:InvalidBreakTime", 
                        "Long break must be between 1 and 60 minutes.");
                DefaultLongBreakMinutes = longBreak.Value;
            }
        }
    }
}
