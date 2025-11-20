namespace NouAssist.RequirementsManagement.QualityAssurance.Aggregates
{
    /// <summary>
    /// Represents a specific round of test execution linked to a Test Plan and a specific build
    /// </summary>
    public class TestCycle : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        /// <summary>
        /// The tenant that this test cycle belongs to
        /// </summary>
        public Guid? TenantId { get; protected set; }
        
        /// <summary>
        /// The ID of the test plan this cycle belongs to
        /// </summary>
        public Guid TestPlanId { get; private set; }
        
        /// <summary>
        /// The name of the test cycle
        /// </summary>
        public string Name { get; private set; }
        
        /// <summary>
        /// The description of the test cycle
        /// </summary>
        public string Description { get; private set; }
        
        /// <summary>
        /// Information about the build being tested
        /// </summary>
        public string BuildInformation { get; private set; }
        
        /// <summary>
        /// The current status of the test cycle
        /// </summary>
        public TestCycleStatus Status { get; private set; }
        
        /// <summary>
        /// Scheduled start date for the test cycle
        /// </summary>
        public DateTime StartDate { get; private set; }
        
        /// <summary>
        /// Scheduled end date for the test cycle
        /// </summary>
        public DateTime EndDate { get; private set; }
        
        /// <summary>
        /// The user responsible for this test cycle
        /// </summary>
        public Guid AssigneeId { get; private set; }
        
        /// <summary>
        /// The actual start time when the cycle was started
        /// </summary>
        public DateTime? ActualStartTime { get; private set; }
        
        /// <summary>
        /// The actual completion time when the cycle was completed
        /// </summary>
        public DateTime? ActualEndTime { get; private set; }

        /// <summary>
        /// Test runs associated with this test cycle
        /// </summary>
        private readonly List<TestRun> _testRuns;
        
        /// <summary>
        /// Read-only access to test runs
        /// </summary>
        public IReadOnlyCollection<TestRun> TestRuns => _testRuns.AsReadOnly();

        protected TestCycle()
        {
            _testRuns = new List<TestRun>();
        }

        /// <summary>
        /// Factory method to create a new test cycle
        /// </summary>
        public static TestCycle Create(
            Guid id,
            Guid testPlanId,
            string name,
            string description,
            string buildInformation,
            DateTime startDate,
            DateTime endDate,
            Guid assigneeId,
            Guid? tenantId = null)
        {
            return new TestCycle(
                id,
                testPlanId,
                name,
                description,
                buildInformation,
                startDate,
                endDate,
                assigneeId,
                tenantId);
        }

        private TestCycle(
            Guid id,
            Guid testPlanId,
            string name,
            string description,
            string buildInformation,
            DateTime startDate,
            DateTime endDate,
            Guid assigneeId,
            Guid? tenantId = null) : base(id)
        {
            TestPlanId = testPlanId;
            SetName(name);
            SetDescription(description);
            BuildInformation = buildInformation;
            SetDates(startDate, endDate);
            AssigneeId = assigneeId;
            Status = TestCycleStatus.Planned;
            TenantId = tenantId;
            _testRuns = new List<TestRun>();
        }

        /// <summary>
        /// Updates the basic information of the test cycle
        /// </summary>
        public void UpdateBasicInfo(
            string name,
            string description,
            string buildInformation,
            DateTime startDate,
            DateTime endDate,
            Guid assigneeId)
        {
            if (Status != TestCycleStatus.Planned)
            {
                throw new BusinessRuleViolationException("Test cycle can only be updated when in Planned status");
            }

            SetName(name);
            SetDescription(description);
            BuildInformation = buildInformation;
            SetDates(startDate, endDate);
            AssigneeId = assigneeId;
        }

        /// <summary>
        /// Starts the test cycle
        /// </summary>
        public void Start(DateTime startTime)
        {
            var spec = new StartableTestCycleSpecification();
            var validationResult = spec.ValidateStart(this);
            
            if (!validationResult.IsValid)
            {
                throw new TestCycleCannotBeStartedException(
                    Id, 
                    string.Join(", ", validationResult.Errors));
            }

            Status = TestCycleStatus.InProgress;
            ActualStartTime = startTime;
            
            AddDistributedEvent(new TestCycleStartedEto
            {
                Id = Id,
                TestPlanId = TestPlanId,
                Name = Name,
                BuildInformation = BuildInformation,
                StartDate = startTime,
                TenantId = TenantId
            });
        }

        /// <summary>
        /// Completes the test cycle
        /// </summary>
        public void Complete(DateTime endTime)
        {
            var spec = new CompletableTestCycleSpecification();
            var validationResult = spec.ValidateCompletion(this);
            
            if (!validationResult.IsValid)
            {
                throw new BusinessRuleViolationException(
                    $"Cannot complete test cycle: {string.Join(", ", validationResult.Errors)}");
            }

            Status = TestCycleStatus.Completed;
            ActualEndTime = endTime;
            
            var progress = GetTestExecutionProgress();
            
            AddDistributedEvent(new TestCycleCompletedEto
            {
                Id = Id,
                TestPlanId = TestPlanId,
                Name = Name,
                EndDate = endTime,
                TotalTestRuns = progress.Total,
                PassedTestRuns = progress.Passed,
                FailedTestRuns = progress.Failed,
                TenantId = TenantId
            });
        }

        /// <summary>
        /// Pauses the test cycle
        /// </summary>
        public void Pause()
        {
            if (Status != TestCycleStatus.InProgress)
            {
                throw new InvalidTestCycleStatusTransitionException(Status, TestCycleStatus.Paused);
            }

            Status = TestCycleStatus.Paused;
        }

        /// <summary>
        /// Resumes the test cycle after it was paused
        /// </summary>
        public void Resume()
        {
            if (Status != TestCycleStatus.Paused)
            {
                throw new InvalidTestCycleStatusTransitionException(Status, TestCycleStatus.InProgress);
            }

            Status = TestCycleStatus.InProgress;
        }

        /// <summary>
        /// Cancels the test cycle
        /// </summary>
        public void Cancel(string reason)
        {
            if (Status == TestCycleStatus.Completed)
            {
                throw new InvalidTestCycleStatusTransitionException(Status, TestCycleStatus.Cancelled);
            }

            Status = TestCycleStatus.Cancelled;
        }

        /// <summary>
        /// Adds a test run to the test cycle
        /// </summary>
        public TestRun AddTestRun(
            Guid testCaseId,
            string testCaseTitle,
            string expectedOutcome)
        {
            if (Status != TestCycleStatus.Planned)
            {
                throw new BusinessRuleViolationException(
                    "Test runs can only be added when the test cycle is in Planned status");
            }

            // Prevent duplicate test runs for the same test case
            if (_testRuns.Any(tr => tr.TestCaseId == testCaseId))
            {
                throw new BusinessRuleViolationException(
                    $"A test run for test case {testCaseId} already exists in this cycle");
            }

            var testRun = new TestRun(
                Id,
                testCaseId,
                testCaseTitle,
                expectedOutcome);

            _testRuns.Add(testRun);

            return testRun;
        }

        /// <summary>
        /// Records the result of a test run
        /// </summary>
        public void RecordTestResult(
            Guid testCaseId,
            TestRunResult result,
            string actualOutcomeDescription,
            string actualOutcomeLogData,
            Guid executorId,
            DateTime executionTime)
        {
            if (Status != TestCycleStatus.InProgress)
            {
                throw new BusinessRuleViolationException(
                    "Test results can only be recorded when the test cycle is in progress");
            }

            var testRun = _testRuns.FirstOrDefault(tr => tr.TestCaseId == testCaseId);
            if (testRun == null)
            {
                throw new BusinessRuleViolationException(
                    $"No test run found for test case {testCaseId} in this cycle");
            }

            testRun.UpdateResult(
                result,
                actualOutcomeDescription,
                actualOutcomeLogData,
                executorId,
                executionTime);
        }

        /// <summary>
        /// Gets the test execution progress for this test cycle
        /// </summary>
        public (int Total, int Passed, int Failed, int Blocked, int NotRun) GetTestExecutionProgress()
        {
            int total = _testRuns.Count;
            int passed = _testRuns.Count(tr => tr.Result == TestRunResult.Pass);
            int failed = _testRuns.Count(tr => tr.Result == TestRunResult.Fail);
            int blocked = _testRuns.Count(tr => tr.Result == TestRunResult.Blocked);
            int notRun = _testRuns.Count(tr => tr.Result == TestRunResult.NotRun || tr.Result == TestRunResult.InProgress || tr.Result == TestRunResult.Skipped);

            return (total, passed, failed, blocked, notRun);
        }

        private void SetName(string name)
        {
            Name = Check.NotNullOrWhiteSpace(
                name,
                nameof(name),
                maxLength: QADomainConstants.MaxTestCycleNameLength
            );
        }

        private void SetDescription(string description)
        {
            Description = Check.NotNullOrWhiteSpace(
                description,
                nameof(description),
                maxLength: QADomainConstants.MaxTestCycleDescriptionLength
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
