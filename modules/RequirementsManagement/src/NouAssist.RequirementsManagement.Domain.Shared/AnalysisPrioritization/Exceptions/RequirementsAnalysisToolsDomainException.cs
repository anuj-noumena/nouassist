namespace NouAssist.RequirementsManagement.AnalysisPrioritization.Exceptions
{
    /// <summary>
    /// Base exception for Requirements Analysis Tools domain
    /// </summary>
    public class RequirementsAnalysisToolsDomainException : BusinessException
    {
        public RequirementsAnalysisToolsDomainException(string code)
            : base(code)
        {
        }

        public RequirementsAnalysisToolsDomainException(string code, string message)
            : base(code, message)
        {
        }

        public RequirementsAnalysisToolsDomainException WithData(string name, object value)
        {
            Data[name] = value;
            return this;
        }
    }
}
