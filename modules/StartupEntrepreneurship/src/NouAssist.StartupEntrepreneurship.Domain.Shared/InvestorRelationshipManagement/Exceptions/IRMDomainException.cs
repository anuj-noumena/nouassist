using Volo.Abp;

namespace NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.Exceptions
{
    /// <summary>
    /// Base exception for IRM domain
    /// </summary>
    public class IRMDomainException : BusinessException
    {
        public IRMDomainException(string code)
            : base(code)
        {
        }

        public IRMDomainException(string code, string message)
            : base(code, message)
        {
        }

        public IRMDomainException WithData(string name, object value)
        {
            Data[name] = value;
            return this;
        }
    }
}
