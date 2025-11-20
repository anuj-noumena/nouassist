namespace NouAssist.BusinessOperations.ContractPortfolioManagement.Aggregates.Portfolio
{
    /// <summary>
    /// Represents a roadmap for a portfolio with milestones and dependencies
    /// </summary>
    public class PortfolioRoadmapAggregate : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        #region Properties
        public Guid? TenantId { get; protected set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public Guid PortfolioId { get; private set; }
        public DateTime StartDate { get; private set; }
        public DateTime EndDate { get; private set; }

        private readonly List<RoadmapMilestone> _milestones;
        public IReadOnlyCollection<RoadmapMilestone> Milestones => _milestones.AsReadOnly();

        private readonly List<CrossItemDependency> _dependencies;
        public IReadOnlyCollection<CrossItemDependency> Dependencies => _dependencies.AsReadOnly();
        #endregion

        #region Constructors
        protected PortfolioRoadmapAggregate()
        {
            _milestones = new List<RoadmapMilestone>();
            _dependencies = new List<CrossItemDependency>();
        }

        public static PortfolioRoadmapAggregate Create(
            Guid id,
            Guid? tenantId,
            string title,
            string description,
            Guid portfolioId,
            DateTime startDate,
            DateTime endDate)
        {
            return new PortfolioRoadmapAggregate(
                id, 
                tenantId, 
                title, 
                description, 
                portfolioId, 
                startDate, 
                endDate);
        }

        private PortfolioRoadmapAggregate(
            Guid id,
            Guid? tenantId,
            string title,
            string description,
            Guid portfolioId,
            DateTime startDate,
            DateTime endDate)
            : base(id)
        {
            TenantId = tenantId;
            SetTitle(title);
            SetDescription(description);
            PortfolioId = portfolioId;
            
            if (endDate <= startDate)
            {
                throw new BusinessException("ContractPortfolioManagement:Validation:DateRange")
                    .WithData("property", "EndDate")
                    .WithData("reference", "StartDate");
            }
            
            StartDate = startDate;
            EndDate = endDate;

            _milestones = new List<RoadmapMilestone>();
            _dependencies = new List<CrossItemDependency>();

            // Add domain event
            AddDistributedEvent(new PortfolioRoadmapCreatedEto
            {
                Id = Id,
                Title = Title,
                PortfolioId = PortfolioId,
                TenantId = TenantId
            });
        }
        #endregion

        #region Public Methods
        public void UpdateBasicDetails(
            string title,
            string description,
            DateTime startDate,
            DateTime endDate)
        {
            SetTitle(title);
            SetDescription(description);
            
            if (endDate <= startDate)
            {
                throw new BusinessException("ContractPortfolioManagement:Validation:DateRange")
                    .WithData("property", "EndDate")
                    .WithData("reference", "StartDate");
            }
            
            StartDate = startDate;
            EndDate = endDate;
        }

        public RoadmapMilestone AddMilestone(
            string title,
            string description,
            MilestoneDate date,
            Guid? portfolioItemId = null)
        {
            if (date.PlannedDate < StartDate || date.PlannedDate > EndDate)
            {
                throw new BusinessException("ContractPortfolioManagement:Validation:DateRange")
                    .WithData("property", "MilestoneDate")
                    .WithData("reference", "Roadmap date range");
            }

            var milestone = new RoadmapMilestone(
                GuidGenerator.Create(),
                Id,
                title,
                description,
                date,
                portfolioItemId);

            _milestones.Add(milestone);
            return milestone;
        }

        public void RemoveMilestone(Guid milestoneId)
        {
            var milestone = _milestones.FirstOrDefault(m => m.Id == milestoneId);
            if (milestone != null)
            {
                // Remove any dependencies related to this milestone
                var relatedDependencies = _dependencies
                    .Where(d => d.SourceMilestoneId == milestoneId || d.TargetMilestoneId == milestoneId)
                    .ToList();

                foreach (var dependency in relatedDependencies)
                {
                    _dependencies.Remove(dependency);
                }

                _milestones.Remove(milestone);
            }
        }

        public CrossItemDependency AddDependency(
            Guid sourceMilestoneId,
            Guid targetMilestoneId,
            DependencyType dependencyType)
        {
            // Validate milestones exist
            var sourceMilestone = _milestones.FirstOrDefault(m => m.Id == sourceMilestoneId);
            var targetMilestone = _milestones.FirstOrDefault(m => m.Id == targetMilestoneId);
            
            if (sourceMilestone == null)
            {
                throw new BusinessException("ContractPortfolioManagement:ErrorMessage:MilestoneNotFound")
                    .WithData("milestoneId", sourceMilestoneId)
                    .WithData("roadmapId", Id);
            }
            
            if (targetMilestone == null)
            {
                throw new BusinessException("ContractPortfolioManagement:ErrorMessage:MilestoneNotFound")
                    .WithData("milestoneId", targetMilestoneId)
                    .WithData("roadmapId", Id);
            }

            // Check for existing dependency
            var existingDependency = _dependencies.FirstOrDefault(d => 
                d.SourceMilestoneId == sourceMilestoneId && 
                d.TargetMilestoneId == targetMilestoneId);
                
            if (existingDependency != null)
            {
                existingDependency.UpdateType(dependencyType);
                return existingDependency;
            }

            var dependency = new CrossItemDependency(
                GuidGenerator.Create(),
                Id,
                sourceMilestoneId,
                targetMilestoneId,
                dependencyType);

            _dependencies.Add(dependency);
            return dependency;
        }

        public void RemoveDependency(Guid dependencyId)
        {
            var dependency = _dependencies.FirstOrDefault(d => d.Id == dependencyId);
            if (dependency != null)
            {
                _dependencies.Remove(dependency);
            }
        }

        public void RemoveDependency(Guid sourceMilestoneId, Guid targetMilestoneId)
        {
            var dependency = _dependencies.FirstOrDefault(d => 
                d.SourceMilestoneId == sourceMilestoneId && 
                d.TargetMilestoneId == targetMilestoneId);
                
            if (dependency != null)
            {
                _dependencies.Remove(dependency);
            }
        }

        public double CalculateOverallProgress()
        {
            if (!_milestones.Any())
            {
                return 0;
            }

            var totalMilestones = _milestones.Count;
            var completedMilestones = _milestones.Count(m => m.Date.IsCompleted);

            return ((double)completedMilestones / totalMilestones) * 100;
        }

        public List<RoadmapMilestone> GetMilestonesInDateRange(DateTime startDate, DateTime endDate)
        {
            return _milestones
                .Where(m => m.Date.PlannedDate >= startDate && m.Date.PlannedDate <= endDate)
                .OrderBy(m => m.Date.PlannedDate)
                .ToList();
        }

        public List<CrossItemDependency> GetDependenciesForMilestone(Guid milestoneId)
        {
            return _dependencies
                .Where(d => d.SourceMilestoneId == milestoneId || d.TargetMilestoneId == milestoneId)
                .ToList();
        }
        #endregion

        #region Private Methods
        private void SetTitle(string title)
        {
            Title = Check.NotNullOrWhiteSpace(
                title,
                nameof(title),
                maxLength: ContractPortfolioManagementConsts.PortfolioRoadmap.MaxTitleLength
            );
        }

        private void SetDescription(string description)
        {
            Description = Check.Length(
                description,
                nameof(description),
                maxLength: ContractPortfolioManagementConsts.PortfolioRoadmap.MaxDescriptionLength
            );
        }
    }
}
