using Volo.Abp;

namespace NouAssist.BusinessOperations.FreelanceOperations.Exceptions
{
    /// <summary>
    /// Base exception for FreelanceOperations domain
    /// </summary>
    public class FreelanceOperationsDomainException : BusinessException
    {
        public FreelanceOperationsDomainException(string code)
            : base(code)
        {
        }

        public FreelanceOperationsDomainException(string code, string message)
            : base(code, message)
        {
        }

        public FreelanceOperationsDomainException WithData(string name, object value)
        {
            Data[name] = value;
            return this;
        }
    }
}
