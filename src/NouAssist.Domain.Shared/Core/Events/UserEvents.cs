using System;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace NouAssist.Core.Events
{
    [EventName("NouAssist.Users.ProfileUpdated")]
    public class UserProfileUpdatedEto : IMultiTenant
    {
        public Guid UserId { get; set; }
        public Guid? TenantId { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
    }

    [EventName("NouAssist.Users.SettingsChanged")]
    public class UserSettingsChangedEto : IMultiTenant
    {
        public Guid UserId { get; set; }
        public Guid? TenantId { get; set; }
        public DateTime ChangedAt { get; set; }
        public string SettingType { get; set; }
    }

    [EventName("NouAssist.Core.TagUsageUpdated")]
    public class TagUsageUpdatedEto : IMultiTenant
    {
        public Guid TagId { get; set; }
        public Guid? TenantId { get; set; }
        public int NewUsageCount { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
