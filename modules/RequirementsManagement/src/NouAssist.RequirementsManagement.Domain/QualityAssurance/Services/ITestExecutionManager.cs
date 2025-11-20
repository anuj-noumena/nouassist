namespace NouAssist.RequirementsManagement.QualityAssurance.Services
{
    /// <summary>
    /// Domain service for managing test execution
    /// </summary>
    public interface ITestExecutionManager
    {
        /// <summary>
        /// Creates a new test cycle
        /// </summary>
        Task<TestCycle> CreateTestCycleAsync(
            Guid testPlanId,
            string name,
            string description,
            string buildInformation,
            DateTime startDate,
            DateTime endDate,
            Guid assigneeId);
        
        /// <summary>
        /// Starts a test cycle
        /// </summary>
        Task<TestCycle> StartTestCycleAsync(Guid testCycleId);
        
        /// <summary>
        /// Completes a test cycle
        /// </summary>
        Task<TestCycle> CompleteTestCycleAsync(Guid testCycleId);
        
        /// <summary>
        /// Adds a test case to a test cycle
        /// </summary>
        Task<TestRun> AddTestCaseAsync(
            Guid testCycleId,
            Guid testCaseId,
            string testCaseTitle,
            string expectedOutcome);
        
        /// <summary>
        /// Records the result of a test run
        /// </summary>
        Task RecordTestResultAsync(
            Guid testCycleId,
            Guid testCaseId,
            TestRunResult result,
            string actualOutcomeDescription,
            string actualOutcomeLogData = null);
        
        /// <summary>
        /// Gets the test execution progress for a test plan
        /// </summary>
        Task<(int Total, int Passed, int Failed, int Blocked, int NotRun)> GetTestPlanProgressAsync(Guid testPlanId);
        
        /// <summary>
        /// Gets the test execution progress for a test cycle
        /// </summary>
        Task<(int Total, int Passed, int Failed, int Blocked, int NotRun)> GetTestCycleProgressAsync(Guid testCycleId);
    }
}
