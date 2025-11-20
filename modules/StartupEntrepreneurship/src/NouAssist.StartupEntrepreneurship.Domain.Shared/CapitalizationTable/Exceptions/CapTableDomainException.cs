using Volo.Abp;

namespace NouAssist.StartupEntrepreneurship.CapitalizationTable.Exceptions
{
    /// <summary>
    /// Base exception for CapTable domain
    /// </summary>
    public class CapTableDomainException : BusinessException
    {
        public CapTableDomainException(string code)
            : base(code)
        {
        }

        public CapTableDomainException(string code, string message)
            : base(code, message)
        {
        }

        public CapTableDomainException WithData(string name, object value)
        {
            Data[name] = value;
            return this;
        }
    }
}
