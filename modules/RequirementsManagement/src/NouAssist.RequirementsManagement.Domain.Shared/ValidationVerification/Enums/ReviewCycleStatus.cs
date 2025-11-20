namespace NouAssist.RequirementsManagement.ValidationVerification.Enums
{
    /// <summary>
    /// Status of a requirement review cycle
    /// </summary>
    public enum ReviewCycleStatus
    {
        /// <summary>
        /// Review cycle is being prepared
        /// </summary>
        Draft = 0,

        /// <summary>
        /// Review cycle has been initiated and is in progress
        /// </summary>
        InProgress = 1,

        /// <summary>
        /// All reviews have been completed successfully
        /// </summary>
        Completed = 2,

        /// <summary>
        /// Review cycle has been cancelled
        /// </summary>
        Cancelled = 3,

        /// <summary>
        /// Review cycle requires changes before completion
        /// </summary>
        RequiresChanges = 4
    }
}
