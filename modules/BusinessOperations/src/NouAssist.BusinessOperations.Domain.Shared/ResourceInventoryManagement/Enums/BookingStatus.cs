namespace NouAssist.BusinessOperations.ResourceInventoryManagement.Enums
{
    /// <summary>
    /// Defines the status of a resource booking
    /// </summary>
    public enum BookingStatus
    {
        /// <summary>
        /// Booking request is pending approval
        /// </summary>
        Pending = 1,

        /// <summary>
        /// Booking is confirmed and active
        /// </summary>
        Confirmed = 2,

        /// <summary>
        /// Resource is currently in use as per booking
        /// </summary>
        InUse = 3,

        /// <summary>
        /// Booking has completed
        /// </summary>
        Completed = 4,

        /// <summary>
        /// Booking was cancelled
        /// </summary>
        Cancelled = 5,

        /// <summary>
        /// Booking was rejected
        /// </summary>
        Rejected = 6
    }
}
