using Volo.Abp;

namespace NouAssist.BusinessOperations.DocumentManagement.Exceptions
{
    /// <summary>
    /// Base exception for DocumentManagement domain
    /// </summary>
    public class DocumentManagementDomainException : BusinessException
    {
        public DocumentManagementDomainException(string code)
            : base(code)
        {
        }

        public DocumentManagementDomainException(string code, string message)
            : base(code, message)
        {
        }

        public DocumentManagementDomainException WithData(string name, object value)
        {
            Data[name] = value;
            return this;
        }
    }
}
