using System;
using NouAssist.BusinessOperations.ResourceInventoryManagement.Exceptions;

namespace NouAssist.BusinessOperations.ResourceInventoryManagement.Exceptions.ResourceType
{
    /// <summary>
    /// Exception thrown when a resource type cannot be found
    /// </summary>
    public class ResourceTypeNotFoundException : RIMDomainException
    {
        /// <summary>
        /// ID of the resource type that was not found
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        /// Creates a new instance of ResourceTypeNotFoundException
        /// </summary>
        /// <param name="id">ID of the resource type that was not found</param>
        public ResourceTypeNotFoundException(Guid id)
            : base("RIM:ErrorMessage:ResourceTypeNotFound")
        {
            Id = id;
            WithData("id", id);
        }

        /// <summary>
        /// Creates a new instance of ResourceTypeNotFoundException for a resource type code
        /// </summary>
        /// <param name="code">Code of the resource type that was not found</param>
        public ResourceTypeNotFoundException(string code)
            : base("RIM:ErrorMessage:ResourceTypeNotFoundByCode")
        {
            WithData("code", code);
        }
    }
}
