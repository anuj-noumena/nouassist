namespace NouAssist.BusinessOperations.FreelanceOperations.Enums
{
    /// <summary>
    /// Represents the status of a freelance client
    /// </summary>
    public enum ClientStatus
    {
        /// <summary>
        /// Client is active and in good standing
        /// </summary>
        Active = 1,
        
        /// <summary>
        /// Client is inactive (no current projects)
        /// </summary>
        Inactive = 2,
        
        /// <summary>
        /// Client is a lead (potential client)
        /// </summary>
        Lead = 3,
        
        /// <summary>
        /// Client has been archived
        /// </summary>
        Archived = 4
    }
}
