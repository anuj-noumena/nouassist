using System;

namespace NouAssist.RequirementsManagement.AnalysisPrioritization.Exceptions
{
    /// <summary>
    /// Exception thrown when a prioritization activity is not found
    /// </summary>
    public class PrioritizationActivityNotFoundException : RequirementsAnalysisToolsDomainException
    {
        public PrioritizationActivityNotFoundException(Guid id)
            : base("RequirementsAnalysisTools:ErrorMessage:PrioritizationActivityNotFound")
        {
            WithData("id", id);
        }
    }
}
