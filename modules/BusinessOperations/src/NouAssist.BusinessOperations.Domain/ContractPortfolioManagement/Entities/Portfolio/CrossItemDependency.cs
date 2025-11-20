namespace NouAssist.BusinessOperations.ContractPortfolioManagement.Entities.Portfolio
{
    /// <summary>
    /// Represents a dependency between roadmap milestones
    /// </summary>
    public class CrossItemDependency : Entity<Guid>
    {
        public Guid RoadmapId { get; private set; }
        public Guid SourceMilestoneId { get; private set; }
        public Guid TargetMilestoneId { get; private set; }
        public DependencyType Type { get; private set; }

        protected CrossItemDependency() { }

        public CrossItemDependency(
            Guid id,
            Guid roadmapId,
            Guid sourceMilestoneId,
            Guid targetMilestoneId,
            DependencyType type)
            : base(id)
        {
            RoadmapId = roadmapId;
            SourceMilestoneId = sourceMilestoneId;
            TargetMilestoneId = targetMilestoneId;
            Type = type;
        }

        public void UpdateType(DependencyType type)
        {
            Type = type;
        }
    }
}
