using Volo.Abp;

namespace NouAssist.RequirementsManagement.Specification.Exceptions
{
    /// <summary>
    /// Base exception for Requirements Specification Management domain
    /// </summary>
    public class RequirementSpecificationManagementDomainException : BusinessException
    {
        public RequirementSpecificationManagementDomainException(string code)
            : base(code)
        {
        }

        public RequirementSpecificationManagementDomainException(string code, string message)
            : base(code, message)
        {
        }

        public RequirementSpecificationManagementDomainException WithData(string name, object value)
        {
            Data[name] = value;
            return this;
        }
    }
}
