using System;
using NouAssist.BusinessOperations.ResourceInventoryManagement.Events.Base;
using Volo.Abp.EventBus;

namespace NouAssist.BusinessOperations.ResourceInventoryManagement.Events.ResourceBooking
{
    /// <summary>
    /// Event raised when a resource is booked
    /// </summary>
    [EventName("NouAssist.BusinessOperations.ResourceInventoryManagement.ResourceBooked")]
    public class ResourceBookedEto : RIMEtoBase
    {
        /// <summary>
        /// ID of the booking
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// ID of the resource instance
        /// </summary>
        public Guid ResourceInstanceId { get; set; }

        /// <summary>
        /// ID of the user who booked the resource
        /// </summary>
        public Guid BookedByUserId { get; set; }

        /// <summary>
        /// Start time of the booking
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// End time of the booking
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// Status of the booking
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Purpose of the booking
        /// </summary>
        public string Purpose { get; set; }
    }
}
