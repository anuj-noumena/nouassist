namespace NouAssist.RequirementsManagement.QualityAssurance.Specifications
{
    /// <summary>
    /// Specifications for test plans
    /// </summary>
    public abstract class TestPlanSpecification : QASpecification<TestPlan>
    {
    }

    /// <summary>
    /// Specification for active test plans
    /// </summary>
    public class ActiveTestPlanSpecification : TestPlanSpecification
    {
        public override Expression<Func<TestPlan, bool>> ToExpression()
        {
            return testPlan => testPlan.Status == TestPlanStatus.Active;
        }
    }

    /// <summary>
    /// Specification for test plans that can be activated
    /// </summary>
    public class ActivatableTestPlanSpecification : TestPlanSpecification
    {
        public override Expression<Func<TestPlan, bool>> ToExpression()
        {
            return testPlan => testPlan.Status == TestPlanStatus.Draft;
        }
        
        /// <summary>
        /// Validates if a test plan can be activated and returns detailed reasons if not
        /// </summary>
        public ValidationResult ValidateActivation(TestPlan testPlan)
        {
            var result = new ValidationResult();
            
            if (testPlan.Status != TestPlanStatus.Draft)
            {
                result.AddError($"Test plan must be in Draft status to activate, current status: {testPlan.Status}");
            }
            
            if (string.IsNullOrEmpty(testPlan.ReleaseVersion))
            {
                result.AddError("Release version is required before activation");
            }
            
            return result;
        }
    }

    /// <summary>
    /// Specification for test plans that can be completed
    /// </summary>
    public class CompletableTestPlanSpecification : TestPlanSpecification
    {
        public override Expression<Func<TestPlan, bool>> ToExpression()
        {
            return testPlan => 
                testPlan.Status == TestPlanStatus.Active && 
                testPlan.TestCycles.All(tc => 
                    tc.Status == TestCycleStatus.Completed || 
                    tc.Status == TestCycleStatus.Cancelled);
        }
        
        /// <summary>
        /// Validates if a test plan can be completed and returns detailed reasons if not
        /// </summary>
        public ValidationResult ValidateCompletion(TestPlan testPlan)
        {
            var result = new ValidationResult();
            
            if (testPlan.Status != TestPlanStatus.Active)
            {
                result.AddError($"Test plan must be in Active status to complete, current status: {testPlan.Status}");
                return result;
            }
            
            var incompleteCycles = testPlan.TestCycles
                .Where(tc => tc.Status != TestCycleStatus.Completed && tc.Status != TestCycleStatus.Cancelled)
                .ToList();
                
            if (incompleteCycles.Any())
            {
                result.AddError($"All test cycles must be completed or cancelled before completing the test plan. " +
                                $"Found {incompleteCycles.Count} incomplete cycle(s)");
            }
            
            return result;
        }
    }
}
