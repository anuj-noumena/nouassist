using System;

namespace NouAssist.RequirementsManagement.AnalysisPrioritization.Exceptions
{
    /// <summary>
    /// Exception thrown when an analyzed requirement is not found
    /// </summary>
    public class AnalyzedRequirementNotFoundException : RequirementsAnalysisToolsDomainException
    {
        public AnalyzedRequirementNotFoundException(Guid id)
            : base("RequirementsAnalysisTools:ErrorMessage:AnalyzedRequirementNotFound")
        {
            WithData("id", id);
        }
    }
}
