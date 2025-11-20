namespace NouAssist.RequirementsManagement.Traceability.Enums
{
    /// <summary>
    /// Defines the type of traceable artifact
    /// </summary>
    public enum ArtifactType
    {
        /// <summary>
        /// Requirement artifact
        /// </summary>
        Requirement = 1,
        
        /// <summary>
        /// Test artifact
        /// </summary>
        Test = 2,
        
        /// <summary>
        /// Code artifact
        /// </summary>
        Code = 3,
        
        /// <summary>
        /// Design artifact
        /// </summary>
        Design = 4,
        
        /// <summary>
        /// Document artifact
        /// </summary>
        Document = 5,
        
        /// <summary>
        /// Other artifacts not categorized
        /// </summary>
        Other = 99
    }
}
