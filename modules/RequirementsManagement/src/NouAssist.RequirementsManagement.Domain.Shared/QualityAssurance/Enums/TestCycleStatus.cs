namespace NouAssist.RequirementsManagement.QualityAssurance.Enums
{
    /// <summary>
    /// Represents the status of a test cycle
    /// </summary>
    public enum TestCycleStatus
    {
        /// <summary>
        /// Test cycle is planned but not started
        /// </summary>
        Planned = 0,
        
        /// <summary>
        /// Test cycle is in progress
        /// </summary>
        InProgress = 1,
        
        /// <summary>
        /// Test cycle is paused
        /// </summary>
        Paused = 2,
        
        /// <summary>
        /// Test cycle is completed
        /// </summary>
        Completed = 3,
        
        /// <summary>
        /// Test cycle is cancelled
        /// </summary>
        Cancelled = 4
    }
}
