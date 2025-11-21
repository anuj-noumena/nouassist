using Volo.Abp;

namespace NouAssist.RequirementsManagement.Elicitation.Exceptions
{
    /// <summary>
    /// Base exception for Elicitation domain
    /// </summary>
    public class ElicitationDomainException : BusinessException
    {
        public ElicitationDomainException(string code)
            : base(code)
        {
        }

        public ElicitationDomainException(string code, string message)
            : base(code, message)
        {
        }

        public ElicitationDomainException WithData(string name, object value)
        {
            Data[name] = value;
            return this;
        }
    }
}
