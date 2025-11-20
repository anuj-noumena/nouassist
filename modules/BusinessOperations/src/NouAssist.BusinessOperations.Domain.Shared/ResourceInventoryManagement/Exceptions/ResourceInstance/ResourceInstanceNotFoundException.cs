using System;
using NouAssist.BusinessOperations.ResourceInventoryManagement.Exceptions;

namespace NouAssist.BusinessOperations.ResourceInventoryManagement.Exceptions.ResourceInstance
{
    /// <summary>
    /// Exception thrown when a resource instance cannot be found
    /// </summary>
    public class ResourceInstanceNotFoundException : RIMDomainException
    {
        /// <summary>
        /// ID of the resource instance that was not found
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        /// Creates a new instance of ResourceInstanceNotFoundException
        /// </summary>
        /// <param name="id">ID of the resource instance that was not found</param>
        public ResourceInstanceNotFoundException(Guid id)
            : base("RIM:ErrorMessage:ResourceInstanceNotFound")
        {
            Id = id;
            WithData("id", id);
        }

        /// <summary>
        /// Creates a new instance of ResourceInstanceNotFoundException for a resource instance identifier
        /// </summary>
        /// <param name="identifier">Identifier of the resource instance that was not found</param>
        public ResourceInstanceNotFoundException(string identifier)
            : base("RIM:ErrorMessage:ResourceInstanceNotFoundByIdentifier")
        {
            WithData("identifier", identifier);
        }
    }
}
