using Volo.Abp;

namespace NouAssist.Resources.Exceptions
{
    public class ResourceDomainException : BusinessException
    {
        public ResourceDomainException(string code, string message = null) 
            : base(code, message)
        {
            WithData("Domain", "Resources");
        }
    }
}
