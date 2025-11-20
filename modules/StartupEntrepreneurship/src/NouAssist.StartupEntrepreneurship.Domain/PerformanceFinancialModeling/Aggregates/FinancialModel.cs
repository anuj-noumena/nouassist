namespace NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Aggregates
{
    /// <summary>
    /// Aggregate root for financial modeling with scenarios and projections
    /// </summary>
    public class FinancialModel : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Guid? TenantId { get; protected set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public ModelStatus Status { get; private set; }
        public int ProjectionYears { get; private set; }
        public DateTime BaseDate { get; private set; }
        public string Currency { get; private set; }
        public bool IsActive { get; private set; }

        // Collections
        private readonly List<ModelScenario> _scenarios;
        private readonly List<AssumptionSet> _assumptions;
        private readonly List<ProjectionResult> _projections;

        public IReadOnlyCollection<ModelScenario> Scenarios => _scenarios.AsReadOnly();
        public IReadOnlyCollection<AssumptionSet> Assumptions => _assumptions.AsReadOnly();
        public IReadOnlyCollection<ProjectionResult> Projections => _projections.AsReadOnly();

        protected FinancialModel()
        {
            _scenarios = new List<ModelScenario>();
            _assumptions = new List<AssumptionSet>();
            _projections = new List<ProjectionResult>();
        }

        public static FinancialModel Create(
            Guid id,
            string name,
            string description,
            int projectionYears,
            DateTime baseDate,
            string currency = "USD",
            Guid? tenantId = null)
        {
            return new FinancialModel(id, name, description, projectionYears, baseDate, currency, tenantId);
        }

        private FinancialModel(
            Guid id,
            string name,
            string description,
            int projectionYears,
            DateTime baseDate,
            string currency,
            Guid? tenantId) : base(id)
        {
            SetName(name);
            SetDescription(description);
            SetProjectionYears(projectionYears);
            BaseDate = baseDate;
            Currency = currency ?? "USD";
            TenantId = tenantId;
            Status = ModelStatus.Draft;
            IsActive = true;
            _scenarios = new List<ModelScenario>();
            _assumptions = new List<AssumptionSet>();
            _projections = new List<ProjectionResult>();

            AddDistributedEvent(new FinancialModelCreatedEto
            {
                Id = Id,
                Name = Name,
                Description = Description,
                Status = Status,
                ProjectionYears = ProjectionYears,
                TenantId = TenantId
            });
        }

        public void UpdateBasicInfo(string name, string description, int projectionYears)
        {
            SetName(name);
            SetDescription(description);
            SetProjectionYears(projectionYears);
        }

        public void ChangeStatus(ModelStatus newStatus)
        {
            if (!CanChangeStatus(newStatus))
            {
                throw new BusinessException("PerformanceFinancialModeling:ErrorMessage:InvalidStatusTransition")
                    .WithData("currentStatus", Status)
                    .WithData("newStatus", newStatus);
            }

            Status = newStatus;
        }

        public void Activate()
        {
            IsActive = true;
        }

        public void Deactivate()
        {
            IsActive = false;
        }

        public ModelScenario AddScenario(string name, ScenarioType type, string description = null)
        {
            // Check for duplicate scenario names
            if (_scenarios.Any(s => s.Name.Equals(name, StringComparison.OrdinalIgnoreCase)))
            {
                throw new BusinessException("PerformanceFinancialModeling:ErrorMessage:DuplicateScenarioName")
                    .WithData("scenarioName", name);
            }

            var scenario = new ModelScenario(Id, name, type, description);
            _scenarios.Add(scenario);
            return scenario;
        }

        public void RemoveScenario(Guid scenarioId)
        {
            var scenario = _scenarios.FirstOrDefault(s => s.Id == scenarioId);
            if (scenario == null)
            {
                throw new BusinessException("PerformanceFinancialModeling:ErrorMessage:ScenarioNotFound")
                    .WithData("scenarioId", scenarioId);
            }

            // Remove associated projections
            _projections.RemoveAll(p => p.ScenarioId == scenarioId);
            _scenarios.Remove(scenario);
        }

        public AssumptionSet AddAssumptionSet(string name, string description = null)
        {
            // Check for duplicate assumption set names
            if (_assumptions.Any(a => a.Name.Equals(name, StringComparison.OrdinalIgnoreCase)))
            {
                throw new BusinessException("PerformanceFinancialModeling:ErrorMessage:DuplicateAssumptionName")
                    .WithData("assumptionName", name);
            }

            var assumptionSet = new AssumptionSet(Id, name, description);
            _assumptions.Add(assumptionSet);
            return assumptionSet;
        }

        public void RemoveAssumptionSet(Guid assumptionSetId)
        {
            var assumptionSet = _assumptions.FirstOrDefault(a => a.Id == assumptionSetId);
            if (assumptionSet == null)
            {
                throw new BusinessException("PerformanceFinancialModeling:ErrorMessage:AssumptionSetNotFound")
                    .WithData("assumptionSetId", assumptionSetId);
            }

            _assumptions.Remove(assumptionSet);
        }

        public ProjectionResult AddProjection(Guid scenarioId, ProjectionType type, 
                                           List<ProjectedAmount> projectedAmounts, string notes = null)
        {
            var scenario = _scenarios.FirstOrDefault(s => s.Id == scenarioId);
            if (scenario == null)
            {
                throw new BusinessException("PerformanceFinancialModeling:ErrorMessage:ScenarioNotFound")
                    .WithData("scenarioId", scenarioId);
            }

            // Remove existing projection of the same type for this scenario
            _projections.RemoveAll(p => p.ScenarioId == scenarioId && p.Type == type);

            var projection = new ProjectionResult(Id, scenarioId, type, projectedAmounts, notes);
            _projections.Add(projection);

            // Publish event for scenario calculation
            AddDistributedEvent(new ModelScenarioCalculatedEto
            {
                Id = Id,
                ModelId = Id,
                ModelName = Name,
                ScenarioName = scenario.Name,
                ScenarioType = scenario.Type,
                ProjectionYears = ProjectionYears,
                FinalCashPosition = projectedAmounts.LastOrDefault()?.Amount ?? 0,
                TenantId = TenantId
            });

            return projection;
        }

        public ModelScenario GetScenario(Guid scenarioId)
        {
            return _scenarios.FirstOrDefault(s => s.Id == scenarioId);
        }

        public ModelScenario GetScenarioByName(string name)
        {
            return _scenarios.FirstOrDefault(s => s.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        public AssumptionSet GetAssumptionSet(Guid assumptionSetId)
        {
            return _assumptions.FirstOrDefault(a => a.Id == assumptionSetId);
        }

        public ProjectionResult GetProjection(Guid scenarioId, ProjectionType type)
        {
            return _projections.FirstOrDefault(p => p.ScenarioId == scenarioId && p.Type == type);
        }

        public List<ProjectionResult> GetProjectionsForScenario(Guid scenarioId)
        {
            return _projections.Where(p => p.ScenarioId == scenarioId).ToList();
        }

        public bool CanCalculate()
        {
            return Status != ModelStatus.Archived && 
                   Status != ModelStatus.Deprecated && 
                   _scenarios.Any() && 
                   _assumptions.Any();
        }

        private bool CanChangeStatus(ModelStatus newStatus)
        {
            return Status switch
            {
                ModelStatus.Draft => newStatus == ModelStatus.Active || newStatus == ModelStatus.UnderReview,
                ModelStatus.UnderReview => newStatus == ModelStatus.Draft || newStatus == ModelStatus.Approved,
                ModelStatus.Active => newStatus == ModelStatus.UnderReview || newStatus == ModelStatus.Archived,
                ModelStatus.Approved => newStatus == ModelStatus.Active || newStatus == ModelStatus.Archived,
                ModelStatus.Archived => newStatus == ModelStatus.Active,
                ModelStatus.Deprecated => false,
                _ => false
            };
        }

        private void SetName(string name)
        {
            Name = Check.NotNullOrWhiteSpace(
                name,
                nameof(name),
                maxLength: PerformanceFinancialModelingConsts.MaxModelNameLength
            );
        }

        private void SetDescription(string description)
        {
            Description = Check.NotNullOrWhiteSpace(
                description,
                nameof(description),
                maxLength: PerformanceFinancialModelingConsts.MaxModelDescriptionLength
            );
        }

        private void SetProjectionYears(int projectionYears)
        {
            if (projectionYears <= 0 || projectionYears > PerformanceFinancialModelingConsts.MaxProjectionYears)
            {
                throw new ArgumentException($"Projection years must be between 1 and {PerformanceFinancialModelingConsts.MaxProjectionYears}");
            }
            ProjectionYears = projectionYears;
        }
    }
}
