using System;
using NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.ValueObjects;
using Volo.Abp.Domain.Entities;

namespace NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Entities
{
    /// <summary>
    /// Represents a record of a completed task
    /// </summary>
    public class TaskCompletionRecord : Entity<Guid>
    {
        /// <summary>
        /// Name of the task
        /// </summary>
        public string TaskName { get; protected set; }
        
        /// <summary>
        /// Estimated duration in minutes
        /// </summary>
        public int EstimatedDuration { get; protected set; }
        
        /// <summary>
        /// Actual duration in minutes
        /// </summary>
        public int ActualDuration { get; protected set; }
        
        /// <summary>
        /// When the task was completed
        /// </summary>
        public DateTime CompletedAt { get; protected set; }
        
        /// <summary>
        /// Context of the task
        /// </summary>
        public TaskContext TaskContext { get; protected set; }

        protected TaskCompletionRecord()
        {
        }

        /// <summary>
        /// Creates a new task completion record
        /// </summary>
        public TaskCompletionRecord(
            Guid id,
            string taskName,
            int estimatedDuration,
            int actualDuration,
            DateTime completedAt,
            TaskContext taskContext = null) : base(id)
        {
            TaskName = taskName;
            EstimatedDuration = estimatedDuration;
            ActualDuration = actualDuration;
            CompletedAt = completedAt;
            TaskContext = taskContext;
        }

        /// <summary>
        /// Gets the estimation accuracy
        ///
