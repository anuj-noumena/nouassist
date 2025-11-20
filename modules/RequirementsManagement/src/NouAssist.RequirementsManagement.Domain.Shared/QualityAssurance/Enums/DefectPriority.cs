namespace NouAssist.RequirementsManagement.QualityAssurance.Enums
{
    /// <summary>
    /// Represents the priority of a defect
    /// </summary>
    public enum DefectPriority
    {
        /// <summary>
        /// Immediate priority - fix as soon as possible
        /// </summary>
        Immediate = 0,
        
        /// <summary>
        /// High priority - fix in current iteration
        /// </summary>
        High = 1,
        
        /// <summary>
        /// Medium priority - fix in upcoming iterations
        /// </summary>
        Medium = 2,
        
        /// <summary>
        /// Low priority - fix when time allows
        /// </summary>
        Low = 3
    }
}
