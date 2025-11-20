namespace NouAssist.RequirementsManagement.ChangeManagement.ChangeRequest.Dtos
{
    /// <summary>
    /// DTO for impact assessment results
    /// </summary>
    public class ImpactAssessmentResultDto
    {
        public decimal? EstimatedCost { get; set; }
        public int? EstimatedTimeDays { get; set; }
        public List<string> AffectedComponents { get; set; } = new();
        public List<DependencyAnalysisResultDto> Dependencies { get; set; } = new();
        public List<string> Risks { get; set; } = new();
        public List<ImpactDetailsDto> DetailedImpacts { get; set; } = new();
    }
}
