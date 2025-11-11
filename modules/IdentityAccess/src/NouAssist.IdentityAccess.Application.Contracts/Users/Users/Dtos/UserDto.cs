using System;
using Volo.Abp.Application.Dtos;

namespace NouAssist.IdentityAccess.Users.Users.Dtos
{
    public class UserDto : FullAuditedEntityDto<Guid>
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Bio { get; set; }
        public string? Avatar { get; set; }
        public string? Email { get; set; }
        public string? Timezone { get; set; }
        public string? Locale { get; set; }
        public bool IsActive { get; set; }
        public DateTime? LastLoginAt { get; set; }
        public string FullName { get; set; }
        public string DisplayName { get; set; }
        public string Initials { get; set; }
    }

    public class UpdateUserProfileDto
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Bio { get; set; }
        public string? Avatar { get; set; }
    }

    public class UserSettingsDto : FullAuditedEntityDto<Guid>
    {
        public Guid UserId { get; set; }
        public string? Theme { get; set; }
        public string? Language { get; set; }
        public string? Timezone { get; set; }
        public string? DateFormat { get; set; }
        public string? TimeFormat { get; set; }
        public string WeekStartDay { get; set; }
        public int DefaultPomodoroMinutes { get; set; }
        public int DefaultShortBreakMinutes { get; set; }
        public int DefaultLongBreakMinutes { get; set; }
        public bool EnableNotifications { get; set; }
        public bool EnableSounds { get; set; }
        public bool EnableDarkMode { get; set; }
    }

    public class UpdateUserSettingsDto
    {
        public string? Theme { get; set; }
        public string? Language { get; set; }
        public string? Timezone { get; set; }
        public string? DateFormat { get; set; }
        public string? TimeFormat { get; set; }
        public string? WeekStartDay { get; set; }
        public bool? EnableNotifications { get; set; }
        public bool? EnableSounds { get; set; }
        public bool? EnableDarkMode { get; set; }
    }

    public class UpdatePomodoroSettingsDto
    {
        public int? DefaultPomodoroMinutes { get; set; }
        public int? DefaultShortBreakMinutes { get; set; }
        public int? DefaultLongBreakMinutes { get; set; }
    }

    public class UserEisenhowerPreferenceDto : FullAuditedEntityDto<Guid>
    {
        public Guid UserId { get; set; }
        public double ImportanceThreshold { get; set; }
        public double UrgencyThreshold { get; set; }
        public bool UseAutoClassification { get; set; }
    }

    public class UpdateEisenhowerPreferenceDto
    {
        public double? ImportanceThreshold { get; set; }
        public double? UrgencyThreshold { get; set; }
        public bool? UseAutoClassification { get; set; }
    }
}
