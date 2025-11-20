namespace NouAssist.RequirementsManagement.QualityAssurance.Enums
{
    /// <summary>
    /// Represents the severity of a defect
    /// </summary>
    public enum DefectSeverity
    {
        /// <summary>
        /// Critical severity - system crash, data loss, security vulnerability
        /// </summary>
        Critical = 0,
        
        /// <summary>
        /// High severity - major functionality broken, no workaround
        /// </summary>
        High = 1,
        
        /// <summary>
        /// Medium severity - functionality issue with workaround
        /// </summary>
        Medium = 2,
        
        /// <summary>
        /// Low severity - minor issues, cosmetic problems
        /// </summary>
        Low = 3
    }
}
