using System;

namespace NouAssist.DecisionSupport.Exceptions
{
    /// <summary>
    /// Exception thrown when an entity is not found
    /// </summary>
    public class EntityNotFoundException : DecisionSupportDomainException
    {
        /// <summary>
        /// The ID of the entity that was not found
        /// </summary>
        public Guid EntityId { get; }
        
        /// <summary>
        /// The type of entity that was not found
        /// </summary>
        public string EntityType { get; }

        /// <summary>
        /// Creates a new instance of <see cref="EntityNotFoundException"/>
        /// </summary>
        /// <param name="entityType">The type of entity</param>
        /// <param name="id">The ID of the entity</param>
        public EntityNotFoundException(string entityType, Guid id)
            : base("DecisionSupport:ErrorMessage:EntityNotFound")
        {
            EntityId = id;
            EntityType = entityType;
            WithData("entityType", entityType);
            WithData("id", id);
        }
    }
}
