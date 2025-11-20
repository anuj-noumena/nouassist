namespace NouAssist.BusinessOperations.CustomerRelationshipManagement.Exceptions
{
    /// <summary>
    /// Base exception for CRM domain exceptions
    /// </summary>
    public class CRMDomainException : BusinessException
    {
        public CRMDomainException(string code)
            : base(code)
        {
        }

        public CRMDomainException(string code, string message)
            : base(code, message)
        {
        }

        public CRMDomainException WithData(string name, object value)
        {
            Data[name] = value;
            return this;
        }
    }
}
