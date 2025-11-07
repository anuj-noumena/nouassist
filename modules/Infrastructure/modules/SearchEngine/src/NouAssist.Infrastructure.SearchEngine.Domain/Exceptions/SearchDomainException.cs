using Volo.Abp;

namespace NouAssist.Infrastructure.SearchEngine.Exceptions
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
