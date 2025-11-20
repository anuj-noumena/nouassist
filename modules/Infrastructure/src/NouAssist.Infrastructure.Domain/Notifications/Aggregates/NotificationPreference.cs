using System;
using NouAssist.Notification.Notifications.Enums;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace NouAssist.Infrastructure.Notifications.Aggregates
{
    public class NotificationPreference : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Guid UserId { get; private set; }
        public NotificationType NotificationType { get; private set; }
        public bool PushEnabled { get; private set; }
        public bool EmailEnabled { get; private set; }
        public bool SmsEnabled { get; private set; }
        public string CustomSchedule { get; private set; }
        public bool IsActive { get; private set; }
        public Guid? TenantId { get; private set; }

        protected NotificationPreference() { /* ORM */ }

        public NotificationPreference(
            Guid id,
            Guid userId,
            NotificationType notificationType,
            Guid? tenantId,
            bool pushEnabled = true,
            bool emailEnabled = true,
            bool smsEnabled = false) : base(id)
        {
            UserId = Check.NotNull(userId, nameof(userId));
            NotificationType = notificationType;
            TenantId = tenantId;
            PushEnabled = pushEnabled;
            EmailEnabled = emailEnabled;
            SmsEnabled = smsEnabled;
            IsActive = true;
        }

        public void EnablePush() => PushEnabled = true;
        public void DisablePush() => PushEnabled = false;
        public void EnableEmail() => EmailEnabled = true;
        public void DisableEmail() => EmailEnabled = false;
        public void EnableSms() => SmsEnabled = true;
        public void DisableSms() => SmsEnabled = false;

        public void DisableAll()
        {
            PushEnabled = false;
            EmailEnabled = false;
            SmsEnabled = false;
        }

        public void SetCustomSchedule(string schedule)
        {
            CustomSchedule = schedule;
        }

        public void Activate() => IsActive = true;
        public void Deactivate() => IsActive = false;

        public bool HasAnyChannelEnabled => PushEnabled || EmailEnabled || SmsEnabled;
    }
}
