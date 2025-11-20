namespace NouAssist.RequirementsManagement.AnalysisPrioritization.PrioritizationActivities.Dtos
{
    /// <summary>
    /// DTO representing a ranking score
    /// </summary>
    public class RankScoreDto
    {
        public decimal Score { get; set; }
        public int Rank { get; set; }
        public decimal Confidence { get; set; }
        public int VoteCount { get; set; }
    }
}
