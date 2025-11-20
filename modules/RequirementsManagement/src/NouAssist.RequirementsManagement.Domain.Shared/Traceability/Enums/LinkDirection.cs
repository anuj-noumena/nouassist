namespace NouAssist.RequirementsManagement.Traceability.Enums
{
    /// <summary>
    /// Defines the direction of the relationship between artifacts
    /// </summary>
    public enum LinkDirection
    {
        /// <summary>
        /// Forward direction - from source to target
        /// </summary>
        Forward = 1,
        
        /// <summary>
        /// Backward direction - from target to source
        /// </summary>
        Backward = 2,
        
        /// <summary>
        /// Bidirectional relationship
        /// </summary>
        Bidirectional = 3
    }
}
