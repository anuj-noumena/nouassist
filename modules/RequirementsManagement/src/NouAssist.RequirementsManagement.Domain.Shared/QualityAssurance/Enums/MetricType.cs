namespace NouAssist.RequirementsManagement.QualityAssurance.Enums
{
    /// <summary>
    /// Represents the type of quality metric
    /// </summary>
    public enum MetricType
    {
        /// <summary>
        /// Defect density - number of defects per unit of code
        /// </summary>
        DefectDensity = 0,
        
        /// <summary>
        /// Test coverage percentage
        /// </summary>
        TestCoverage = 1,
        
        /// <summary>
        /// Test pass rate percentage
        /// </summary>
        TestPassRate = 2,
        
        /// <summary>
        /// Number of open defects
        /// </summary>
        OpenDefects = 3,
        
        /// <summary>
        /// Number of critical defects
        /// </summary>
        CriticalDefects = 4,
        
        /// <summary>
        /// Time spent on test execution
        /// </summary>
        TestExecutionTime = 5,
        
        /// <summary>
        /// Average time to fix defects
        /// </summary>
        AverageTimeToFix = 6
    }
}
