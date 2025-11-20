using Volo.Abp.Domain.Repositories;

namespace NouAssist.RequirementsManagement.ValidationVerification.Repositories
{
    /// <summary>
    /// Repository for TestCase aggregate
    /// </summary>
    public interface ITestCaseRepository : IRepository<TestCase, Guid>
    {
        /// <summary>
        /// Gets test cases linked to a specific requirement
        /// </summary>
        Task<List<TestCase>> GetTestCasesForRequirementAsync(
            Guid requirementId,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets test cases for multiple requirements
        /// </summary>
        Task<List<TestCase>> GetTestCasesForRequirementsAsync(
            List<Guid> requirementIds,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets test cases by status
        /// </summary>
        Task<List<TestCase>> GetTestCasesByStatusAsync(
            TestCaseStatus status,
            Guid? tenantId = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets test cases by priority
        /// </summary>
        Task<List<TestCase>> GetTestCasesByPriorityAsync(
            TestCasePriority priority,
            Guid? tenantId = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Searches test cases by name or description
        /// </summary>
        Task<List<TestCase>> SearchTestCasesAsync(
            string searchTerm,
            Guid? tenantId = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets test cases with execution logs in date range
        /// </summary>
        Task<List<TestCase>> GetTestCasesWithExecutionsInRangeAsync(
            DateTime startDate,
            DateTime endDate,
            Guid? tenantId = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets test case execution statistics
        /// </summary>
        Task<List<TestCaseExecutionSummary>> GetTestCaseExecutionSummariesAsync(
            List<Guid> testCaseIds,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Counts test cases by various criteria
        /// </summary>
        Task<TestCaseMetrics> GetTestCaseMetricsAsync(
            Guid? tenantId = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets test cases requiring attention (failed, blocked, etc.)
        /// </summary>
        Task<List<TestCase>> GetTestCasesRequiringAttentionAsync(
            Guid? tenantId = null,
            CancellationToken cancellationToken = default);
    }

    /// <summary>
    /// Execution summary for a test case
    /// </summary>
    public class TestCaseExecutionSummary
    {
        public Guid TestCaseId { get; set; }
        public string TestCaseName { get; set; }
        public int TotalExecutions { get; set; }
        public int PassedExecutions { get; set; }
        public int FailedExecutions { get; set; }
        public TestExecutionStatus? LastExecutionStatus { get; set; }
        public DateTime? LastExecutionDate { get; set; }
        public TimeSpan? AverageExecutionTime { get; set; }
    }

    /// <summary>
    /// Aggregate metrics for test cases
    /// </summary>
    public class TestCaseMetrics
    {
        public int TotalTestCases { get; set; }
        public Dictionary<TestCaseStatus, int> TestCasesByStatus { get; set; } = new();
        public Dictionary<TestCasePriority, int> TestCasesByPriority { get; set; } = new();
        public Dictionary<TestExecutionStatus, int> LastExecutionsByStatus { get; set; } = new();
        public int TestCasesWithoutExecutions { get; set; }
        public int RequirementsCovered { get; set; }
        public double AverageExecutionTime { get; set; }
    }
}
