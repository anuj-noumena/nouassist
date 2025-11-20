using NouAssist.RequirementsManagement.AnalysisPrioritization.Enums;

namespace NouAssist.RequirementsManagement.AnalysisPrioritization.PrioritizationActivities.Dtos
{
    /// <summary>
    /// DTO representing a vote value
    /// </summary>
    public class VoteValueDto
    {
        public decimal NumericValue { get; set; }
        public string? StringValue { get; set; }
        public PrioritizationMethod Method { get; set; }
    }
}
