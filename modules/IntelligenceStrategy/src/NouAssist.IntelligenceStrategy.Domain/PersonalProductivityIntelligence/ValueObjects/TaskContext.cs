using System;
using System.Collections.Generic;
using System.Linq;
using Volo.Abp.Domain.Values;

namespace NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.ValueObjects
{
    /// <summary>
    /// Value object representing the context of a task
    /// </summary>
    public class TaskContext : ValueObject
    {
        /// <summary>
        /// Project or context ID
        /// </summary>
        public Guid? ProjectId { get; private set; }
        
        /// <summary>
        /// Team or group ID
        /// </summary>
        public Guid? TeamId { get; private set; }
        
        /// <summary>
        /// Tags associated with the task
        /// </summary>
        public IReadOnlyList<string> Tags { get; private set; }
        
        /// <summary>
        /// Location context (if relevant)
        /// </summary>
        public string Location { get; private set; }
        
        /// <summary>
        /// Energy level required (High, Medium, Low)
        /// </summary>
        public string EnergyLevel { get; private set; }
        
        /// <summary>
        /// Focus level required (High, Medium, Low)
        /// </summary>
        public string FocusLevel { get; private set; }

        protected TaskContext()
        {
            Tags = new List<string>();
        }

        /// <summary>
        /// Creates a new task context
        /// </summary>
        public TaskContext(
            Guid? projectId,
            Guid? teamId,
            IEnumerable<string> tags,
            string location,
            string energyLevel,
            string focusLevel)
        {
            ProjectId = projectId;
            TeamId = teamId;
            Tags = tags?.ToList().AsReadOnly() ?? new List<string>().AsReadOnly();
            Location = location;
            EnergyLevel = energyLevel;
            FocusLevel = focusLevel;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return ProjectId ?? Guid.Empty;
            yield return TeamId ?? Guid.Empty;
            yield return string.Join(",", Tags);
            yield return Location ?? string.Empty;
            yield return EnergyLevel ?? string.Empty;
            yield return FocusLevel ?? string.Empty;
        }
    }
}
