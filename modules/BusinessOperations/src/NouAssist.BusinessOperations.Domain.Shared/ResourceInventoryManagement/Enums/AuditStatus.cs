namespace NouAssist.BusinessOperations.ResourceInventoryManagement.Enums
{
    /// <summary>
    /// Defines the status of an inventory audit session
    /// </summary>
    public enum AuditStatus
    {
        /// <summary>
        /// Audit is planned but not started
        /// </summary>
        Planned = 1,

        /// <summary>
        /// Audit is in progress
        /// </summary>
        InProgress = 2,

        /// <summary>
        /// Audit is complete with no discrepancies
        /// </summary>
        CompleteNoDiscrepancies = 3,

        /// <summary>
        /// Audit is complete with discrepancies
        /// </summary>
        CompleteWithDiscrepancies = 4,

        /// <summary>
        /// Audit is reconciled after discrepancies
        /// </summary>
        Reconciled = 5,

        /// <summary>
        /// Audit was cancelled
        /// </summary>
        Cancelled = 6
    }
}
