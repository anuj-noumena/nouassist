namespace NouAssist.RequirementsManagement.AnalysisPrioritization.Services
{
    /// <summary>
    /// Domain service interface for performing impact analysis on requirements
    /// </summary>
    public interface IImpactAnalysisEngine
    {
        Task<ImpactAssessmentResult> AnalyzeImpactAsync(
            Guid requirementId,
            CancellationToken cancellationToken = default);

        Task<List<ImpactAssessmentResult>> AnalyzeBatchImpactAsync(
            List<Guid> requirementIds,
            CancellationToken cancellationToken = default);

        Task<ImpactAssessmentResult> AnalyzeChangeImpactAsync(
            Guid requirementId,
            string changeDescription,
            CancellationToken cancellationToken = default);
    }
}
