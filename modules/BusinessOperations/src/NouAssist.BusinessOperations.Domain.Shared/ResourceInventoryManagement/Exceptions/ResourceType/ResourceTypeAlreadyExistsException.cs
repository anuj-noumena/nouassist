using System;
using NouAssist.BusinessOperations.ResourceInventoryManagement.Exceptions;

namespace NouAssist.BusinessOperations.ResourceInventoryManagement.Exceptions.ResourceType
{
    /// <summary>
    /// Exception thrown when attempting to create a resource type with a code that already exists
    /// </summary>
    public class ResourceTypeAlreadyExistsException : RIMDomainException
    {
        /// <summary>
        /// The code that already exists
        /// </summary>
        public string Code { get; }

        /// <summary>
        /// Creates a new instance of ResourceTypeAlreadyExistsException
        /// </summary>
        /// <param name="code">The duplicate code</param>
        public ResourceTypeAlreadyExistsException(string code)
            : base("RIM:ErrorMessage:ResourceTypeAlreadyExists")
        {
            Code = code;
            WithData("code", code);
        }
    }
}
