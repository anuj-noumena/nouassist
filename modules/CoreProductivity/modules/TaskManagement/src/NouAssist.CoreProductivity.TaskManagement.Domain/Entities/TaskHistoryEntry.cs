using System;
using NouAssist.Tasks.Enums;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace NouAssist.Tasks.Entities
{
    /// <summary>
    /// Represents a historical record of changes made to a TaskInstance
    /// This is a child entity owned by TaskInstance aggregate
    /// </summary>
    public class TaskHistoryEntry : Entity<Guid>
    {
        /// <summary>
        /// The TaskInstance this history entry belongs to
        /// </summary>
        public Guid TaskInstanceId { get; private set; }

        /// <summary>
        /// Type of change that occurred
        /// </summary>
        public TaskHistoryChangeType ChangeType { get; private set; }

        /// <summary>
        /// User who made the change
        /// </summary>
        public Guid? ChangedBy { get; private set; }

        /// <summary>
        /// When the change occurred
        /// </summary>
        public DateTime ChangedAt { get; private set; }

        /// <summary>
        /// Description of the change
        /// </summary>
        public string Description { get; private set; }

        /// <summary>
        /// Name of the field that changed (if applicable)
        /// </summary>
        public string FieldName { get; private set; }

        /// <summary>
        /// Previous value before the change (serialized as JSON if complex)
        /// </summary>
        public string OldValue { get; private set; }

        /// <summary>
        /// New value after the change (serialized as JSON if complex)
        /// </summary>
        public string NewValue { get; private set; }

        /// <summary>
        /// Additional metadata about the change (JSON)
        /// </summary>
        public string Metadata { get; private set; }

        protected TaskHistoryEntry() { }

        public TaskHistoryEntry(
            Guid id,
            Guid taskInstanceId,
            TaskHistoryChangeType changeType,
            string description,
            Guid? changedBy = null,
            string fieldName = null,
            string oldValue = null,
            string newValue = null,
            string metadata = null)
        {
            Id = id;
            TaskInstanceId = Check.NotDefaultOrNull<Guid>(taskInstanceId, nameof(taskInstanceId));
            ChangeType = changeType;
            Description = Check.NotNullOrWhiteSpace(description, nameof(description));
            ChangedBy = changedBy;
            ChangedAt = DateTime.UtcNow;
            FieldName = fieldName;
            OldValue = oldValue;
            NewValue = newValue;
            Metadata = metadata;
        }

        /// <summary>
        /// Factory method for simple change tracking
        /// </summary>
        public static TaskHistoryEntry Create(
            Guid taskInstanceId,
            TaskHistoryChangeType changeType,
            string description,
            Guid? changedBy = null)
        {
            return new TaskHistoryEntry(
                Guid.NewGuid(),
                taskInstanceId,
                changeType,
                description,
                changedBy);
        }

        /// <summary>
        /// Factory method for field change tracking
        /// </summary>
        public static TaskHistoryEntry CreateFieldChange(
            Guid taskInstanceId,
            TaskHistoryChangeType changeType,
            string fieldName,
            string oldValue,
            string newValue,
            Guid? changedBy = null,
            string description = null)
        {
            var desc = description ?? $"Changed {fieldName} from '{oldValue}' to '{newValue}'";
            return new TaskHistoryEntry(
                Guid.NewGuid(),
                taskInstanceId,
                changeType,
                desc,
                changedBy,
                fieldName,
                oldValue,
                newValue);
        }
    }
}
