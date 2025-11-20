namespace NouAssist.RequirementsManagement.QualityAssurance.Enums
{
    /// <summary>
    /// Represents the status of a test plan
    /// </summary>
    public enum TestPlanStatus
    {
        /// <summary>
        /// Test plan is in draft state
        /// </summary>
        Draft = 0,
        
        /// <summary>
        /// Test plan is active and ready for execution
        /// </summary>
        Active = 1,
        
        /// <summary>
        /// Test plan execution is completed
        /// </summary>
        Completed = 2,
        
        /// <summary>
        /// Test plan is archived
        /// </summary>
        Archived = 3,
        
        /// <summary>
        /// Test plan is cancelled
        /// </summary>
        Cancelled = 4
    }
}
