namespace NouAssist.RequirementsManagement.QualityAssurance.Enums
{
    /// <summary>
    /// Represents the result of a test run
    /// </summary>
    public enum TestRunResult
    {
        /// <summary>
        /// Test has not been executed yet
        /// </summary>
        NotRun = 0,
        
        /// <summary>
        /// Test passed
        /// </summary>
        Pass = 1,
        
        /// <summary>
        /// Test failed
        /// </summary>
        Fail = 2,
        
        /// <summary>
        /// Test execution was blocked
        /// </summary>
        Blocked = 3,
        
        /// <summary>
        /// Test was skipped
        /// </summary>
        Skipped = 4,
        
        /// <summary>
        /// Test execution is in progress
        /// </summary>
        InProgress = 5
    }
}
