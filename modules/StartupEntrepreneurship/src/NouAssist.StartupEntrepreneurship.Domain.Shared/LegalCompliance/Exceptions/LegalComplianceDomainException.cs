using Volo.Abp;

namespace NouAssist.StartupEntrepreneurship.LegalCompliance.Exceptions
{
    /// <summary>
    /// Base exception for Legal & Compliance domain
    /// </summary>
    public class LegalComplianceDomainException : BusinessException
    {
        public LegalComplianceDomainException(string code)
            : base(code)
        {
        }

        public LegalComplianceDomainException(string code, string message)
            : base(code, message)
        {
        }

        public LegalComplianceDomainException WithData(string name, object value)
        {
            Data[name] = value;
            return this;
        }
    }
}
