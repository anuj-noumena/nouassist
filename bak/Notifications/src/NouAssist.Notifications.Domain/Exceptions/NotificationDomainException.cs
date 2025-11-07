using Volo.Abp;

namespace NouAssist.Notifications.Exceptions
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
