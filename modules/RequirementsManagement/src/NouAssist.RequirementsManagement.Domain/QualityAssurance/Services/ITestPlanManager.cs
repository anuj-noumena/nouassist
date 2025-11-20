namespace NouAssist.RequirementsManagement.QualityAssurance.Services
{
    /// <summary>
    /// Domain service for managing test plans
    /// </summary>
    public interface ITestPlanManager
    {
        /// <summary>
        /// Creates a new test plan
        /// </summary>
        Task<TestPlan> CreateAsync(
            string name,
            string description,
            string releaseVersion,
            TestingStrategy strategy,
            EnvironmentConfiguration environment,
            DateTime startDate,
            DateTime endDate);
        
        /// <summary>
        /// Activates a test plan
        /// </summary>
        Task<TestPlan> ActivateAsync(Guid testPlanId);
        
        /// <summary>
        /// Completes a test plan
        /// </summary>
        Task<TestPlan> CompleteAsync(Guid testPlanId);
        
        /// <summary>
        /// Cancels a test plan
        /// </summary>
        Task<TestPlan> CancelAsync(Guid testPlanId, string reason);
        
        /// <summary>
        /// Archives a test plan
        /// </summary>
        Task<TestPlan> ArchiveAsync(Guid testPlanId);
        
        /// <summary>
        /// Links a requirement to a test plan
        /// </summary>
        Task LinkRequirementAsync(Guid testPlanId, Guid requirementId);
        
        /// <summary>
        /// Unlinks a requirement from a test plan
        /// </summary>
        Task UnlinkRequirementAsync(Guid testPlanId, Guid requirementId);
    }
}
