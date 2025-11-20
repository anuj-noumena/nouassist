using System;
using NouAssist.BusinessOperations.ResourceInventoryManagement.Exceptions;

namespace NouAssist.BusinessOperations.ResourceInventoryManagement.Exceptions.ResourceInstance
{
    /// <summary>
    /// Exception thrown when a resource cannot be allocated
    /// </summary>
    public class ResourceAllocationException : RIMDomainException
    {
        /// <summary>
        /// ID of the resource instance
        /// </summary>
        public Guid ResourceInstanceId { get; }

        /// <summary>
        /// Creates a new instance of ResourceAllocationException
        /// </summary>
        /// <param name="resourceInstanceId">ID of the resource instance</param>
        /// <param name="reason">Reason for the allocation failure</param>
        public ResourceAllocationException(Guid resourceInstanceId, string reason)
            : base("RIM:ErrorMessage:ResourceAllocationFailed")
        {
            ResourceInstanceId = resourceInstanceId;
            WithData("resourceInstanceId", resourceInstanceId);
            WithData("reason", reason);
        }
    }
}
