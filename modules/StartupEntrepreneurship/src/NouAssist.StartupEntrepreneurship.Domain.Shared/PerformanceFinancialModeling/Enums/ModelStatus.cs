namespace NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Enums
{
    /// <summary>
    /// Status of financial models
    /// </summary>
    public enum ModelStatus
    {
        /// <summary>
        /// Model is being created/edited
        /// </summary>
        Draft = 1,
        
        /// <summary>
        /// Model is active and being used
        /// </summary>
        Active = 2,
        
        /// <summary>
        /// Model is under review
        /// </summary>
        UnderReview = 3,
        
        /// <summary>
        /// Model has been approved
        /// </summary>
        Approved = 4,
        
        /// <summary>
        /// Model is archived/inactive
        /// </summary>
        Archived = 5,
        
        /// <summary>
        /// Model is deprecated
        /// </summary>
        Deprecated = 6
    }
}
