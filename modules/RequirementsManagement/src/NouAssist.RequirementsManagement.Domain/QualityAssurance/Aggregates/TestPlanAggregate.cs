namespace NouAssist.RequirementsManagement.QualityAssurance.Aggregates
{
    /// <summary>
    /// Represents a test plan that defines the scope, strategy, resources, and schedule 
    /// for testing activities for a specific release or feature set
    /// </summary>
    public class TestPlan : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        /// <summary>
        /// The tenant that this test plan belongs to
        /// </summary>
        public Guid? TenantId { get; protected set; }
        
        /// <summary>
        /// The name of the test plan
        /// </summary>
        public string Name { get; private set; }
        
        /// <summary>
        /// The description of the test plan
        /// </summary>
        public string Description { get; private set; }
        
        /// <summary>
        /// The version or release this test plan is associated with
        /// </summary>
        public string ReleaseVersion { get; private set; }
        
        /// <summary>
        /// The current status of the test plan
        /// </summary>
        public TestPlanStatus Status { get; private set; }
        
        /// <summary>
        /// The testing strategy for this plan
        /// </summary>
        public TestingStrategy Strategy { get; private set; }
        
        /// <summary>
        /// The environment configuration for this test plan
        /// </summary>
        public EnvironmentConfiguration Environment { get; private set; }
        
        /// <summary>
        /// Scheduled start date for the test plan
        /// </summary>
        public DateTime StartDate { get; private set; }
        
        /// <summary>
        /// Scheduled end date for the test plan
        /// </summary>
        public DateTime EndDate { get; private set; }
        
        /// <summary>
        /// IDs of requirements that are in scope for this test plan (from another BC)
        /// </summary>
        private readonly List<Guid> _inScopeRequirementIds;
        
        /// <summary>
        /// Read-only access to in-scope requirement IDs
        /// </summary>
        public IReadOnlyCollection<Guid> InScopeRequirementIds => _inScopeRequirementIds.AsReadOnly();
        
        /// <summary>
        /// Test cycles associated with this test plan
        /// </summary>
        private readonly List<TestCycle> _testCycles;
        
        /// <summary>
        /// Read-only access to test cycles
        /// </summary>
        public IReadOnlyCollection<TestCycle> TestCycles => _testCycles.AsReadOnly();

        protected TestPlan()
        {
            _inScopeRequirementIds = new List<Guid>();
            _testCycles = new List<TestCycle>();
        }

        /// <summary>
        /// Factory method to create a new test plan
        /// </summary>
        public static TestPlan Create(
            Guid id,
            string name,
            string description,
            string releaseVersion,
            TestingStrategy strategy,
            EnvironmentConfiguration environment,
            DateTime startDate,
            DateTime endDate,
            Guid? tenantId = null)
        {
            return new TestPlan(
                id,
                name,
                description,
                releaseVersion,
                strategy,
                environment,
                startDate,
                endDate,
                tenantId);
        }

        private TestPlan(
            Guid id,
            string name,
            string description,
            string releaseVersion,
            TestingStrategy strategy,
            EnvironmentConfiguration environment,
            DateTime startDate,
            DateTime endDate,
            Guid? tenantId = null) : base(id)
        {
            SetName(name);
            SetDescription(description);
            ReleaseVersion = releaseVersion;
            Strategy = strategy;
            Environment = environment;
            SetDates(startDate, endDate);
            Status = TestPlanStatus.Draft;
            TenantId = tenantId;
            _inScopeRequirementIds = new List<Guid>();
            _testCycles = new List<TestCycle>();
            
            AddDistributedEvent(new TestPlanCreatedEto
            {
                Id = Id,
                Name = Name,
                ReleaseVersion = ReleaseVersion,
                TenantId = TenantId,
                CreationTime = Clock.Now
            });
        }

        /// <summary>
        /// Updates the basic information of the test plan
        /// </summary>
        public void UpdateBasicInfo(
            string name,
            string description,
            string releaseVersion,
            TestingStrategy strategy,
            EnvironmentConfiguration environment,
            DateTime startDate,
            DateTime endDate)
        {
            if (Status != TestPlanStatus.Draft)
            {
                throw new BusinessRuleViolationException("Test plan can only be updated when in Draft status");
            }

            SetName(name);
            SetDescription(description);
            ReleaseVersion = releaseVersion;
            Strategy = strategy;
            Environment = environment;
            SetDates(startDate, endDate);
        }

        /// <summary>
        /// Activates the test plan, allowing test cycles to be executed
        /// </summary>
        public void Activate()
        {
            var spec = new ActivatableTestPlanSpecification();
            var validationResult = spec.ValidateActivation(this);
            
            if (!validationResult.IsValid)
            {
                throw new BusinessRuleViolationException(
                    $"Cannot activate test plan: {string.Join(", ", validationResult.Errors)}");
            }

            Status = TestPlanStatus.Active;
        }

        /// <summary>
        /// Completes the test plan after all test cycles are finished
        /// </summary>
        public void Complete()
        {
            var spec = new CompletableTestPlanSpecification();
            var validationResult = spec.ValidateCompletion(this);
            
            if (!validationResult.IsValid)
            {
                throw new BusinessRuleViolationException(
                    $"Cannot complete test plan: {string.Join(", ", validationResult.Errors)}");
            }

            Status = TestPlanStatus.Completed;
        }

        /// <summary>
        /// Cancels the test plan
        /// </summary>
        public void Cancel(string reason)
        {
            if (Status == TestPlanStatus.Completed || Status == TestPlanStatus.Archived)
            {
                throw new InvalidTestPlanStatusTransitionException(Status, TestPlanStatus.Cancelled);
            }

            Status = TestPlanStatus.Cancelled;
        }

        /// <summary>
        /// Archives the test plan
        /// </summary>
        public void Archive()
        {
            if (Status != TestPlanStatus.Completed && Status != TestPlanStatus.Cancelled)
            {
                throw new InvalidTestPlanStatusTransitionException(Status, TestPlanStatus.Archived);
            }

            Status = TestPlanStatus.Archived;
        }

        /// <summary>
        /// Adds a test cycle to the test plan
        /// </summary>
        public TestCycle AddTestCycle(
            string name,
            string description,
            string buildInformation,
            DateTime startDate,
            DateTime endDate,
            Guid assigneeId)
        {
            if (Status != TestPlanStatus.Draft && Status != TestPlanStatus.Active)
            {
                throw new BusinessRuleViolationException(
                    "Test cycles can only be added when the test plan is in Draft or Active status");
            }

            var testCycle = TestCycle.Create(
                GuidGenerator.Create(),
                Id,
                name,
                description,
                buildInformation,
                startDate,
                endDate,
                assigneeId,
                TenantId);

            _testCycles.Add(testCycle);

            return testCycle;
        }

        /// <summary>
        /// Adds a requirement to the scope of this test plan
        /// </summary>
        public void AddRequirementToScope(Guid requirementId)
        {
            if (Status != TestPlanStatus.Draft)
            {
                throw new BusinessRuleViolationException(
                    "Requirements can only be added to scope when the test plan is in Draft status");
            }

            if (!_inScopeRequirementIds.Contains(requirementId))
            {
                _inScopeRequirementIds.Add(requirementId);
            }
        }

        /// <summary>
        /// Removes a requirement from the scope of this test plan
        /// </summary>
        public void RemoveRequirementFromScope(Guid requirementId)
        {
            if (Status != TestPlanStatus.Draft)
            {
                throw new BusinessRuleViolationException(
                    "Requirements can only be removed from scope when the test plan is in Draft status");
            }

            _inScopeRequirementIds.Remove(requirementId);
        }

        /// <summary>
        /// Gets a test cycle by ID
        /// </summary>
        public TestCycle GetTestCycle(Guid testCycleId)
        {
            return _testCycles.FirstOrDefault(tc => tc.Id == testCycleId);
        }

        /// <summary>
        /// Gets the overall test execution progress for this test plan
        /// </summary>
        public (int Total, int Passed, int Failed, int Blocked, int NotRun) GetTestExecutionProgress()
        {
            int total = 0, passed = 0, failed = 0, blocked = 0, notRun = 0;

            foreach (var cycle in _testCycles)
            {
                var progress = cycle.GetTestExecutionProgress();
                total += progress.Total;
                passed += progress.Passed;
                failed += progress.Failed;
                blocked += progress.Blocked;
                notRun += progress.NotRun;
            }

            return (total, passed, failed, blocked, notRun);
        }

        private void SetName(string name)
        {
            Name = Check.NotNullOrWhiteSpace(
                name,
                nameof(name),
                maxLength: QADomainConstants.MaxTestPlanNameLength
            );
        }

        private void SetDescription(string description)
        {
            Description = Check.NotNullOrWhiteSpace(
                description,
                nameof(description),
                maxLength: QADomainConstants.MaxTestPlanDescriptionLength
            );
        }

        private void SetDates(DateTime startDate, DateTime endDate)
        {
            if (endDate <= startDate)
            {
                throw new BusinessRuleViolationException("End date must be after start date");
            }

            StartDate = startDate;
            EndDate = endDate;
        }
    }
}
