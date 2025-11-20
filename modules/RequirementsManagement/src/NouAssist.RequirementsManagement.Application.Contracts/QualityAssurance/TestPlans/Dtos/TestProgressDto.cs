namespace NouAssist.RequirementsManagement.QualityAssurance.TestPlans.Dtos
{
    /// <summary>
    /// DTO for test execution progress information
    /// </summary>
    public class TestProgressDto
    {
        /// <summary>
        /// Total number of tests
        /// </summary>
        public int Total { get; set; }
        
        /// <summary>
        /// Number of passed tests
        /// </summary>
        public int Passed { get; set; }
        
        /// <summary>
        /// Number of failed tests
        /// </summary>
        public int Failed { get; set; }
        
        /// <summary>
        /// Number of blocked tests
        /// </summary>
        public int Blocked { get; set; }
        
        /// <summary>
        /// Number of not run tests (including in progress and skipped)
        /// </summary>
        public int NotRun { get; set; }
        
        /// <summary>
        /// Pass rate percentage (Passed/Total * 100)
        /// </summary>
        public decimal PassRate => Total > 0 ? (decimal)Passed / Total * 100 : 0;
        
        /// <summary>
        /// Failure rate percentage (Failed/Total * 100)
        /// </summary>
        public decimal FailRate => Total > 0 ? (decimal)Failed / Total * 100 : 0;
        
        /// <summary>
        /// Completion percentage ((Total - NotRun)/Total * 100)
        /// </summary>
        public decimal CompletionRate => Total > 0 ? (decimal)(Total - NotRun) / Total * 100 : 0;
    }
}
