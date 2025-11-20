namespace NouAssist.BusinessOperations.FreelanceOperations.Enums
{
    /// <summary>
    /// Represents the billing type for a freelance project
    /// </summary>
    public enum BillingType
    {
        /// <summary>
        /// Billing based on hourly rate
        /// </summary>
        Hourly = 1,
        
        /// <summary>
        /// Billing based on fixed project price
        /// </summary>
        FixedPrice = 2,
        
        /// <summary>
        /// Billing based on milestone completion
        /// </summary>
        Milestone = 3,
        
        /// <summary>
        /// Retainer based billing
        /// </summary>
        Retainer = 4
    }
}
