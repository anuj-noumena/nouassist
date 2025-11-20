using Volo.Abp;

namespace NouAssist.RequirementsManagement.ValidationVerification.Exceptions
{
    /// <summary>
    /// Base exception for ValidationVerification domain
    /// </summary>
    public class ValidationVerificationDomainException : BusinessException
    {
        public ValidationVerificationDomainException(string code)
            : base(code)
        {
        }

        public ValidationVerificationDomainException(string code, string message)
            : base(code, message)
        {
        }

        public ValidationVerificationDomainException WithData(string name, object value)
        {
            Data[name] = value;
            return this;
        }
    }
}
