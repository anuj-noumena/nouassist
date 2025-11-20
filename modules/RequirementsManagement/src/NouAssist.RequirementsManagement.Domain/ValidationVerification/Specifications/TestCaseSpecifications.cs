using Volo.Abp.Specifications;

namespace NouAssist.RequirementsManagement.ValidationVerification.Specifications
{
    /// <summary>
    /// Base specification for TestCase
    /// </summary>
    public abstract class TestCaseSpecification : Specification<TestCase>
    {
    }

    /// <summary>
    /// Specification for executable test cases
    /// </summary>
    public class ExecutableTestCaseSpecification : TestCaseSpecification
    {
        public override Expression<Func<TestCase, bool>> ToExpression()
        {
            return testCase => testCase.Status == TestCaseStatus.Ready &&
                              testCase.TestSteps.Any();
        }
    }

    /// <summary>
    /// Specification for test cases covering a specific requirement
    /// </summary>
    public class RequirementCoverageSpecification : TestCaseSpecification
    {
        private readonly Guid _requirementId;

        public RequirementCoverageSpecification(Guid requirementId)
        {
            _requirementId = requirementId;
        }

        public override Expression<Func<TestCase, bool>> ToExpression()
        {
            return testCase => testCase.RequirementIds.Contains(_requirementId);
        }
    }

    /// <summary>
    /// Specification for high priority test cases
    /// </summary>
    public class HighPriorityTestCaseSpecification : TestCaseSpecification
    {
        public override Expression<Func<TestCase, bool>> ToExpression()
        {
            return testCase => testCase.Priority == TestCasePriority.High ||
                              testCase.Priority == TestCasePriority.Critical;
        }
    }

    /// <summary>
    /// Specification for test cases requiring attention
    /// </summary>
    public class TestCasesRequiringAttentionSpecification : TestCaseSpecification
    {
        public override Expression<Func<TestCase, bool>> ToExpression()
        {
            return testCase => testCase.Status == TestCaseStatus.Blocked ||
                              (testCase.Status == TestCaseStatus.Ready &&
                               testCase.ExecutionLogs.Any(el => el.Status == TestExecutionStatus.Fail)) ||
                              (testCase.Status == TestCaseStatus.Draft &&
                               testCase.CreationTime < DateTime.UtcNow.AddDays(-7));
        }
    }

    /// <summary>
    /// Specification for recently executed test cases
    /// </summary>
    public class RecentlyExecutedTestCaseSpecification : TestCaseSpecification
    {
        private readonly DateTime _sinceDate;

        public RecentlyExecutedTestCaseSpecification(DateTime sinceDate)
        {
            _sinceDate = sinceDate;
        }

        public override Expression<Func<TestCase, bool>> ToExpression()
        {
            return testCase => testCase.ExecutionLogs.Any(el => 
                el.CompletedAt.HasValue && el.CompletedAt.Value >= _sinceDate);
        }
    }

    /// <summary>
    /// Specification for test cases with no recent executions
    /// </summary>
    public class StaleTestCaseSpecification : TestCaseSpecification
    {
        private readonly DateTime _threshold;

        public StaleTestCaseSpecification(int daysThreshold = 30)
        {
            _threshold = DateTime.UtcNow.AddDays(-daysThreshold);
        }

        public override Expression<Func<TestCase, bool>> ToExpression()
        {
            return testCase => testCase.Status == TestCaseStatus.Ready &&
                              (!testCase.ExecutionLogs.Any() ||
                               testCase.ExecutionLogs.All(el =>
                                   !el.CompletedAt.HasValue || el.CompletedAt.Value < _threshold));
        }
    }
}
