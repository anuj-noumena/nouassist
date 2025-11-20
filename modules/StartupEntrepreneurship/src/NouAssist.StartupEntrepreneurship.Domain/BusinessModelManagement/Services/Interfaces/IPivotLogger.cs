namespace NouAssist.StartupEntrepreneurship.BusinessModelManagement.Services.Interfaces
{
    /// <summary>
    /// Domain service for strategic pivot logging and tracking
    /// </summary>
    public interface IPivotLogger
    {
        Task<StrategicPivot> InitiatePivotAsync(
            PivotType type,
            PivotRationale rationale,
            string description,
            Guid initiatedBy,
            DateTime initiatedDate,
            DateTime targetCompletionDate,
            Guid? tenantId);

        Task<ValidationResult> ValidatePivotRationaleAsync(PivotRationale rationale);

        Task<List<ImpactMetricDefinition>> SuggestImpactMetricsAsync(
            PivotType type,
            string description);

        Task LogPivotMilestoneAsync(
            StrategicPivot pivot,
            string milestone,
            DateTime milestoneDate,
            string notes);

        Task<List<StrategicPivot>> GetRelatedPivotsAsync(PivotType type, Guid? tenantId);
    }
}
