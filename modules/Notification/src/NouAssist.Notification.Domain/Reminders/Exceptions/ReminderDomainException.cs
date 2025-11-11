using Volo.Abp;

namespace NouAssist.Notification.Reminders.Exceptions
{
    public class ReminderDomainException : BusinessException
    {
        public ReminderDomainException(string code, string message = null) 
            : base(code, message)
        {
            WithData("Domain", "Reminders");
        }
    }
}
