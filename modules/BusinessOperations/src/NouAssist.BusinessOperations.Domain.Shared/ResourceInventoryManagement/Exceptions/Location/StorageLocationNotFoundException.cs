using System;
using NouAssist.BusinessOperations.ResourceInventoryManagement.Exceptions;

namespace NouAssist.BusinessOperations.ResourceInventoryManagement.Exceptions.Location
{
    /// <summary>
    /// Exception thrown when a storage location cannot be found
    /// </summary>
    public class StorageLocationNotFoundException : RIMDomainException
    {
        /// <summary>
        /// ID of the storage location that was not found
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        /// Creates a new instance of StorageLocationNotFoundException
        /// </summary>
        /// <param name="id">ID of the storage location that was not found</param>
        public StorageLocationNotFoundException(Guid id)
            : base("RIM:ErrorMessage:StorageLocationNotFound")
        {
            Id = id;
            WithData("id", id);
        }

        /// <summary>
        /// Creates a new instance of StorageLocationNotFoundException for a location code
        /// </summary>
        /// <param name="code">Code of the storage location that was not found</param>
        public StorageLocationNotFoundException(string code)
            : base("RIM:ErrorMessage:StorageLocationNotFoundByCode")
        {
            WithData("code", code);
        }
    }
}
