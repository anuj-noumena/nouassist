using Volo.Abp;

namespace NouAssist.RequirementsManagement.ChangeManagement.Exceptions
{
    /// <summary>
    /// Base exception for Change Management domain
    /// </summary>
    public class ChangeManagementDomainException : BusinessException
    {
        public ChangeManagementDomainException(string code)
            : base(code)
        {
        }

        public ChangeManagementDomainException(string code, string message)
            : base(code, message)
        {
        }

        public ChangeManagementDomainException WithData(string name, object value)
        {
            Data[name] = value;
            return this;
        }
    }
}
