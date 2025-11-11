using System;
using System.Collections.Generic;
using NouAssist.WorkManagement.TaskManagement.Enums;
using Volo.Abp.Domain.Values;

namespace NouAssist.WorkManagement.TaskManagement.ValueObjects
{
    public class TaskProgress : ValueObject
    {
        public bool IsNextAction { get; private set; }
        public Guid WorkflowStatusId { get; private set; }
        public WorkflowStatusCategory TaskStatus { get; private set; }
        public double ProgressPercent { get; private set; }

        private TaskProgress() { }

        public TaskProgress(
            Guid workflowStatusId,
            WorkflowStatusCategory taskStatus,
            double progressPercent,
            bool isNextAction = false)
        {
            if (workflowStatusId == Guid.Empty)
            {
                throw new ArgumentException("WorkflowStatusId cannot be empty.", nameof(workflowStatusId));
            }

            if (progressPercent < 0 || progressPercent > 100)
            {
                throw new ArgumentOutOfRangeException(nameof(progressPercent), "Progress percent must be between 0 and 100.");
            }

            WorkflowStatusId = workflowStatusId;
            TaskStatus = taskStatus;
            ProgressPercent = progressPercent;
            IsNextAction = isNextAction;
        }

        // ------------------------------
        // Factory methods
        // ------------------------------

        public static TaskProgress NotStarted(Guid workflowStatusId)
            => new TaskProgress(workflowStatusId, WorkflowStatusCategory.Todo, 0, false);

        public static TaskProgress InProgress(Guid workflowStatusId, double progressPercent)
            => new TaskProgress(workflowStatusId, WorkflowStatusCategory.InProgress, progressPercent, false);

        public static TaskProgress Completed(Guid workflowStatusId)
            => new TaskProgress(workflowStatusId, WorkflowStatusCategory.Done, 100, false);

        // ------------------------------
        // Domain behavior (immutability-safe updates)
        // ------------------------------

        public TaskProgress UpdateProgress(double percent)
        {
            if (percent < 0 || percent > 100)
            {
                throw new ArgumentOutOfRangeException(nameof(percent), "Progress percent must be between 0 and 100.");
            }

            return new TaskProgress(WorkflowStatusId, TaskStatus, percent, IsNextAction);
        }

        public TaskProgress MarkAsNextAction(bool isNext = true)
        {
            return new TaskProgress(WorkflowStatusId, TaskStatus, ProgressPercent, isNext);
        }

        public TaskProgress ChangeStatus(Guid newWorkflowStatusId, WorkflowStatusCategory newStatus)
        {
            return new TaskProgress(newWorkflowStatusId, newStatus, ProgressPercent, IsNextAction);
        }

        // ------------------------------
        // Equality
        // ------------------------------
        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return WorkflowStatusId;
            yield return TaskStatus;
            yield return ProgressPercent;
            yield return IsNextAction;
        }
        
        // ------------------------------
        // Convenience flags
        // ------------------------------

        public bool IsCompleted => TaskStatus == WorkflowStatusCategory.Done || ProgressPercent >= 100;
        public bool IsInProgress => TaskStatus == WorkflowStatusCategory.InProgress && ProgressPercent < 100;
        public bool IsNotStarted => TaskStatus == WorkflowStatusCategory.Todo && ProgressPercent == 0;
        
    }
}
