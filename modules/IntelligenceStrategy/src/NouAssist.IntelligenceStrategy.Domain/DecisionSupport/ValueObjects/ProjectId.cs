using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Values;

namespace NouAssist.IntelligenceStrategy.DecisionSupport.ValueObjects
{
    /// <summary>
    /// Value object representing a reference to a Project in the Project BC
    /// </summary>
    public class ProjectId : ValueObject
    {
        /// <summary>
        /// The project identifier
        /// </summary>
        public Guid Value { get; private set; }

        private ProjectId() { }

        /// <summary>
        /// Creates a new instance of <see cref="ProjectId"/>
        /// </summary>
        /// <param name="value">The project identifier</param>
        public ProjectId(Guid value)
        {
            if (value == Guid.Empty)
            {
                throw new ArgumentException("Project ID cannot be empty", nameof(value));
            }
            
            Value = value;
        }

        /// <inheritdoc/>
        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }
    }
}
