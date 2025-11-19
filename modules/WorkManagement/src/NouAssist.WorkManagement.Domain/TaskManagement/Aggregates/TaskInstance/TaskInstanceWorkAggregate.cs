using System;
using NouAssist.WorkManagement.TaskManagement.Enums;
using NouAssist.WorkManagement.TaskManagement.ValueObjects;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace NouAssist.WorkManagement.TaskManagement.Aggregates.TaskInstance
{
    /// <summary>
    /// Manages the execution details of a task: schedule, time tracking, and priority.
    /// </summary>
    public class TaskInstanceWorkAggregate : FullAuditedAggregateRoot<Guid>
    {
        public Guid TaskInstanceId { get; private set; }
        public TaskWorkTracking WorkTracking { get; private set; }
        public TaskSchedule Schedule { get; private set; }
        public ImportanceUrgencyMatrix Priority { get; private set; }
        public ComplexityEstimation ComplexityEstimation { get; private set; }
        public CognitiveLoad? CognitiveLoad { get; private set; }

        protected TaskInstanceWorkAggregate() { }

        public TaskInstanceWorkAggregate(Guid id, Guid taskInstanceId) : base(id)
        {
            TaskInstanceId = taskInstanceId;
            WorkTracking = TaskWorkTracking.NotStarted();
            Schedule = TaskSchedule.Empty();
            Priority = new ImportanceUrgencyMatrix(0, 0); // Default
            ComplexityEstimation = ComplexityEstimation.Unknown();
        }

        // === Work Execution Methods ===
        public void Start()
        {
            if (WorkTracking.HasStarted) throw new BusinessException("TaskManagement:WorkAlreadyStarted");
            WorkTracking = WorkTracking.Start();
            Schedule = Schedule.MarkStarted(DateTime.UtcNow);
            AddDistributedEvent(new TaskWorkStartedEto { TaskWorkAggregateId = Id, TaskInstanceId = TaskInstanceId });
        }

        public void LogTime(int minutes)
        {
            WorkTracking = WorkTracking.LogTime(minutes);
        }

        public void UpdateProgress(double progressPercent)
        {
            WorkTracking = WorkTracking.UpdateProgress(progressPercent);
        }

        // === Scheduling & Estimation Methods ===
        public void SetPlannedDates(DateTime? start, DateTime? due)
        {
            Schedule = Schedule.WithPlannedDates(start, due);
        }

        public void SetEstimate(int minutes)
        {
            WorkTracking = WorkTracking.WithEstimate(minutes);
        }

        // === Priority & Complexity Methods ===
        public void UpdatePriority(double importance, double urgency)
        {
            Priority = new ImportanceUrgencyMatrix(importance, urgency);
        }

        public void UpdateComplexity(ComplexityEstimation complexity)
        {
            ComplexityEstimation = complexity;
        }

        public void SetCognitiveLoad(CognitiveLoad cognitiveLoad)
        {
            CognitiveLoad = cognitiveLoad;
        }

        // === Event Handlers (called by Application Service after events from TaskAggregate) ===
        public void HandleTaskCompleted()
        {
            WorkTracking = WorkTracking.Complete();
            Schedule = Schedule.MarkCompleted(DateTime.UtcNow);
        }
    }
}