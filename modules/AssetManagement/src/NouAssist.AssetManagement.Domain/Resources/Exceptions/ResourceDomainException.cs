using Volo.Abp;

namespace NouAssist.AssetManagement.Resources.Exceptions
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
