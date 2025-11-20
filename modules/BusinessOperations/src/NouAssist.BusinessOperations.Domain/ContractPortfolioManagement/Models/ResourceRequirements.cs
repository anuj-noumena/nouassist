namespace NouAssist.BusinessOperations.ContractPortfolioManagement.Models
{
    /// <summary>
    /// Represents resource requirements for forecasting
    /// </summary>
    public class ResourceRequirements
    {
        public Guid PortfolioId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Dictionary<string, int> SkillsNeeded { get; set; }
        public Dictionary<DateTime, int> ResourceCountByDate { get; set; }
        public List<ResourceSkillRequirement> CriticalSkillRequirements { get; set; }
    }
}
