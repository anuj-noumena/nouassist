namespace NouAssist.RequirementsManagement.QualityAssurance.Entities
{
    /// <summary>
    /// Represents the execution of a specific test case within a test cycle
    /// </summary>
    public class TestRun : Entity, IHasCreationTime
    {
        /// <summary>
        /// ID of the test cycle this run belongs to
        /// </summary>
        public Guid TestCycleId { get; private set; }
        
        /// <summary>
        /// ID of the test case from V&V BC
        /// </summary>
        public Guid TestCaseId { get; private set; }
        
        /// <summary>
        /// Title of the test case
        /// </summary>
        public string TestCaseTitle { get; private set; }
        
        /// <summary>
        /// The expected outcome of the test
        /// </summary>
        public string ExpectedOutcome { get; private set; }
        
        /// <summary>
        /// The actual outcome of the test execution
        /// </summary>
        public ActualOutcome ActualOutcome { get; private set; }
        
        /// <summary>
        /// The result of the test run
        /// </summary>
        public TestRunResult Result { get; private set; }
        
        /// <summary>
        /// The user who executed the test
        /// </summary>
        public Guid? ExecutorId { get; private set; }
        
        /// <summary>
        /// When the test run was executed
        /// </summary>
        public DateTime? ExecutionTime { get; private set; }
        
        /// <summary>
        /// When the test run was created
        /// </summary>
        public DateTime CreationTime { get; private set; }

        protected TestRun()
        {
            CreationTime = DateTime.Now;
            Result = TestRunResult.NotRun;
        }

        internal TestRun(
            Guid testCycleId,
            Guid testCaseId,
            string testCaseTitle,
            string expectedOutcome)
        {
            TestCycleId = testCycleId;
            TestCaseId = testCaseId;
            TestCaseTitle = Check.NotNullOrWhiteSpace(testCaseTitle, nameof(testCaseTitle));
            ExpectedOutcome = Check.NotNullOrWhiteSpace(expectedOutcome, nameof(expectedOutcome), maxLength: QADomainConstants.MaxExpectedOutcomeLength);
            Result = TestRunResult.NotRun;
            CreationTime = DateTime.Now;
        }

        /// <summary>
        /// Updates the result of the test run
        /// </summary>
        internal void UpdateResult(
            TestRunResult result, 
            string actualOutcomeDescription, 
            string actualOutcomeLogData,
            Guid executorId,
            DateTime executionTime)
        {
            Result = result;
            ActualOutcome = new ActualOutcome(actualOutcomeDescription, actualOutcomeLogData);
            ExecutorId = executorId;
            ExecutionTime = executionTime;
        }

        /// <summary>
        /// Sets the test run as in progress
        /// </summary>
        internal void SetInProgress(Guid executorId)
        {
            Result = TestRunResult.InProgress;
            ExecutorId = executorId;
            ExecutionTime = null;
        }

        /// <summary>
        /// Resets the test run back to not run state
        /// </summary>
        internal void Reset()
        {
            Result = TestRunResult.NotRun;
            ActualOutcome = null;
            ExecutorId = null;
            ExecutionTime = null;
        }

        public override object[] GetKeys()
        {
            return new object[] { TestCycleId, TestCaseId };
        }
    }
}
