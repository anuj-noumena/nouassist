namespace NouAssist.RequirementsManagement.AnalysisPrioritization.Exceptions
{
    /// <summary>
    /// Exception thrown when an invalid prioritization method is used
    /// </summary>
    public class InvalidPrioritizationMethodException : RequirementsAnalysisToolsDomainException
    {
        public InvalidPrioritizationMethodException(PrioritizationMethod method)
            : base("RequirementsAnalysisTools:ErrorMessage:InvalidPrioritizationMethod")
        {
            WithData("method", method);
        }
    }
}
