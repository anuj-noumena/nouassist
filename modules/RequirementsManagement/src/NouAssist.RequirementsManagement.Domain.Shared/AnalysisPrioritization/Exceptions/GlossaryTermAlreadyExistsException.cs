namespace NouAssist.RequirementsManagement.AnalysisPrioritization.Exceptions
{
    /// <summary>
    /// Exception thrown when attempting to create a glossary term that already exists
    /// </summary>
    public class GlossaryTermAlreadyExistsException : RequirementsAnalysisToolsDomainException
    {
        public GlossaryTermAlreadyExistsException(string termName)
            : base("RequirementsAnalysisTools:ErrorMessage:GlossaryTermAlreadyExists")
        {
            WithData("termName", termName);
        }
    }
}
