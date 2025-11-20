namespace NouAssist.RequirementsManagement.QualityAssurance.Specifications
{
    /// <summary>
    /// Specifications for test cycles
    /// </summary>
    public abstract class TestCycleSpecification : QASpecification<TestCycle>
    {
    }

    /// <summary>
    /// Specification for test cycles that can be started
    /// </summary>
    public class StartableTestCycleSpecification : TestCycleSpecification
    {
        public override Expression<Func<TestCycle, bool>> ToExpression()
        {
            return testCycle => 
                testCycle.Status == TestCycleStatus.Planned &&
                !string.IsNullOrEmpty(testCycle.BuildInformation) &&
                testCycle.StartDate <= DateTime.Now;
        }
        
        /// <summary>
        /// Validates if a test cycle can be started and returns detailed reasons if not
        /// </summary>
        public ValidationResult ValidateStart(TestCycle testCycle)
        {
            var result = new ValidationResult();
            
            if (testCycle.Status != TestCycleStatus.Planned)
            {
                result.AddError($"Test cycle must be in Planned status to start, current status: {testCycle.Status}");
            }
            
            if (string.IsNullOrEmpty(testCycle.BuildInformation))
            {
                result.AddError("Build information is required before starting the test cycle");
            }
            
            return result;
        }
    }

    /// <summary>
    /// Specification for test cycles that can be completed
    /// </summary>
    public class CompletableTestCycleSpecification : TestCycleSpecification
    {
        public override Expression<Func<TestCycle, bool>> ToExpression()
        {
            return testCycle => 
                testCycle.Status == TestCycleStatus.InProgress &&
                (testCycle.TestRuns.Count == 0 || 
                 testCycle.TestRuns.All(tr => tr.Result != TestRunResult.NotRun && tr.Result != TestRunResult.InProgress));
        }
        
        /// <summary>
        /// Validates if a test cycle can be completed and returns detailed reasons if not
        /// </summary>
        public ValidationResult ValidateCompletion(TestCycle testCycle)
        {
            var result = new ValidationResult();
            
            if (testCycle.Status != TestCycleStatus.InProgress)
            {
                result.AddError($"Test cycle must be in InProgress status to complete, current status: {testCycle.Status}");
                return result;
            }
            
            var incompleteRuns = testCycle.TestRuns
                .Where(tr => tr.Result == TestRunResult.NotRun || tr.Result == TestRunResult.InProgress)
                .ToList();
                
            if (incompleteRuns.Any())
            {
                result.AddError($"All test runs must be completed before completing the test cycle. " +
                                $"Found {incompleteRuns.Count} incomplete run(s)");
            }
            
            return result;
        }
    }
}
