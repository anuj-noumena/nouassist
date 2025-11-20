using System;
using System.Collections.Generic;
using Volo.Abp;
using Volo.Abp.Domain.Values;

namespace NouAssist.BusinessOperations.ResourceInventoryManagement.ValueObjects
{
    /// <summary>
    /// Represents a user identifier linked to a human resource
    /// </summary>
    public class UserId : ValueObject
    {
        /// <summary>
        /// The user's identity ID
        /// </summary>
        public Guid Value { get; private set; }

        private UserId()
        {
        }

        /// <summary>
        /// Creates a new user ID value object
        /// </summary>
        /// <param name="id">User's identity ID</param>
        public UserId(Guid id)
        {
            Value = id;
        }

        /// <summary>
        /// Gets the atomic values for value comparison
        /// </summary>
        /// <returns>Collection of atomic values</returns>
        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }
    }
}
