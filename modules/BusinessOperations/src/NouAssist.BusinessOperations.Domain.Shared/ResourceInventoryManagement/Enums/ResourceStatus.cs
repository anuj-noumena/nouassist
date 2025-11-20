namespace NouAssist.BusinessOperations.ResourceInventoryManagement.Enums
{
    /// <summary>
    /// Defines the current status of a resource instance
    /// </summary>
    public enum ResourceStatus
    {
        /// <summary>
        /// Resource is available for allocation
        /// </summary>
        Available = 1,

        /// <summary>
        /// Resource is currently allocated
        /// </summary>
        Allocated = 2,

        /// <summary>
        /// Resource is temporarily unavailable (maintenance, leave, etc.)
        /// </summary>
        Unavailable = 3,

        /// <summary>
        /// Resource is no longer in use but kept for historical records
        /// </summary>
        Retired = 4
    }
}
