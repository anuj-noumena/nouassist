namespace NouAssist.StartupEntrepreneurship.BusinessModelManagement.Exceptions
{
    /// <summary>
    /// Base exception for Business Model Management domain
    /// </summary>
    public class BusinessModelManagementDomainException : BusinessException
    {
        public BusinessModelManagementDomainException(string code)
            : base(code)
        {
        }

        public BusinessModelManagementDomainException(string code, string message)
            : base(code, message)
        {
        }

        public BusinessModelManagementDomainException WithData(string name, object value)
        {
            Data[name] = value;
            return this;
        }
    }
}
