namespace NouAssist.BusinessOperations.ContractPortfolioManagement.Models
{
    /// <summary>
    /// Represents a resource allocation conflict
    /// </summary>
    public class ResourceConflict
    {
        public string SkillName { get; set; }
        public DateTime ConflictDate { get; set; }
        public int ResourcesNeeded { get; set; }
        public int ResourcesAvailable { get; set; }
        public List<Guid> AffectedMilestoneIds { get; set; }
        public string ConflictSeverity { get; set; }
    }
}
