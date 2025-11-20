using System;
using NouAssist.BusinessOperations.ResourceInventoryManagement.Exceptions;

namespace NouAssist.BusinessOperations.ResourceInventoryManagement.Exceptions.Booking
{
    /// <summary>
    /// Exception thrown when a resource booking operation fails
    /// </summary>
    public class ResourceBookingException : RIMDomainException
    {
        /// <summary>
        /// ID of the resource instance
        /// </summary>
        public Guid ResourceInstanceId { get; }

        /// <summary>
        /// Creates a new instance of ResourceBookingException
        /// </summary>
        /// <param name="resourceInstanceId">ID of the resource instance</param>
        /// <param name="reason">Reason for the booking failure</param>
        public ResourceBookingException(Guid resourceInstanceId, string reason)
            : base("RIM:ErrorMessage:ResourceBookingFailed")
        {
            ResourceInstanceId = resourceInstanceId;
            WithData("resourceInstanceId", resourceInstanceId);
            WithData("reason", reason);
        }
    }
}
