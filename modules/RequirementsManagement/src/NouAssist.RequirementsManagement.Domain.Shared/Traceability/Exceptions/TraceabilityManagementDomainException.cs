namespace NouAssist.RequirementsManagement.Traceability.Exceptions
{
    /// <summary>
    /// Base exception for TraceabilityManagement domain
    /// </summary>
    public class TraceabilityManagementDomainException : BusinessException
    {
        public TraceabilityManagementDomainException(string code)
            : base(code)
        {
        }

        public TraceabilityManagementDomainException(string code, string message)
            : base(code, message)
        {
        }

        public TraceabilityManagementDomainException WithData(string name, object value)
        {
            Data[name] = value;
            return this;
        }
    }
}
