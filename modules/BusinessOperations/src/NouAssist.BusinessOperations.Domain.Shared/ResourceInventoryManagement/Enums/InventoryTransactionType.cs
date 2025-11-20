namespace NouAssist.BusinessOperations.ResourceInventoryManagement.Enums
{
    /// <summary>
    /// Defines the type of inventory transaction
    /// </summary>
    public enum InventoryTransactionType
    {
        /// <summary>
        /// Initial stock level set
        /// </summary>
        Initial = 1,

        /// <summary>
        /// Stock receipt (adding to inventory)
        /// </summary>
        Receipt = 2,

        /// <summary>
        /// Stock issuance (removing from inventory)
        /// </summary>
        Issuance = 3,

        /// <summary>
        /// Stock transfer between locations
        /// </summary>
        Transfer = 4,

        /// <summary>
        /// Stock adjustment (for corrections)
        /// </summary>
        Adjustment = 5,

        /// <summary>
        /// Stock loss/damage
        /// </summary>
        Loss = 6,

        /// <summary>
        /// Stock return (after issuance)
        /// </summary>
        Return = 7,

        /// <summary>
        /// Reconciliation after audit
        /// </summary>
        Reconciliation = 8
    }
}
