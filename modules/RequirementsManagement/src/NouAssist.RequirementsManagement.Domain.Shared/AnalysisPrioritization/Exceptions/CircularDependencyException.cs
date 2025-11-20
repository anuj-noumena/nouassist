namespace NouAssist.RequirementsManagement.AnalysisPrioritization.Exceptions
{
    /// <summary>
    /// Exception thrown when a circular dependency is detected between requirements
    /// </summary>
    public class CircularDependencyException : RequirementsAnalysisToolsDomainException
    {
        public CircularDependencyException(Guid sourceId, Guid targetId)
            : base("RequirementsAnalysisTools:ErrorMessage:CircularDependency")
        {
            WithData("sourceId", sourceId);
            WithData("targetId", targetId);
        }
    }
}
