using Volo.Abp;

namespace NouAssist.Infrastructure.Activity.Exceptions
{
    public class ActivityDomainException : BusinessException
    {
        public ActivityDomainException(string code, string message = null) 
            : base(code, message)
        {
            WithData("Domain", "Activity");
        }
    }
}
