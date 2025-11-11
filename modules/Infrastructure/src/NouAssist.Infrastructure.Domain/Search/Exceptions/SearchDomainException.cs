using Volo.Abp;

namespace NouAssist.Search.Exceptions
{
    public class SearchDomainException : BusinessException
    {
        public SearchDomainException(string code, string message = null) 
            : base(code, message)
        {
            WithData("Domain", "Search");
        }
    }
}
