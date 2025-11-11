using Volo.Abp;

namespace NouAssist.Notification.Notifications.Exceptions
{
    public class NotificationDomainException : BusinessException
    {
        public NotificationDomainException(string code, string message = null) 
            : base(code, message)
        {
            WithData("Domain", "Notifications");
        }
    }
}
