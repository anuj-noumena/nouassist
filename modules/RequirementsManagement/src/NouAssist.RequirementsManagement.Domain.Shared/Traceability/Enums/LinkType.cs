namespace NouAssist.RequirementsManagement.Traceability.Enums
{
    /// <summary>
    /// Defines the type of relationship between artifacts
    /// </summary>
    public enum LinkType
    {
        /// <summary>
        /// Source artifact satisfies the target artifact's needs
        /// </summary>
        Satisfies = 1,
        
        /// <summary>
        /// Source artifact verifies the target artifact
        /// </summary>
        Verifies = 2,
        
        /// <summary>
        /// Source artifact refines or elaborates on the target artifact
        /// </summary>
        Refines = 3,
        
        /// <summary>
        /// Source artifact conflicts with the target artifact
        /// </summary>
        Conflicts = 4,
        
        /// <summary>
        /// Source artifact requires the target artifact
        /// </summary>
        Requires = 5,
        
        /// <summary>
        /// Source artifact implements the target artifact
        /// </summary>
        Implements = 6,
        
        /// <summary>
        /// Custom relationship type
        /// </summary>
        Custom = 99
    }
}
