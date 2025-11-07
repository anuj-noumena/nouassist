using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Values;

namespace NouAssist.Tasks.ValueObjects
{
    public class TaskHierarchy : ValueObject
    {
        public int OrderInParent { get; private set; }
        public Guid? ParentInstanceId { get; private set; }
        public int SubtasksCount { get; private set; }
        public int SubtasksDoneCount { get; private set; }

        private TaskHierarchy() { }

        public TaskHierarchy(
            int orderInParent,
            Guid? parentInstanceId,
            int subtasksCount = 0,
            int subtasksDoneCount = 0)
        {
            if (orderInParent < 0)
            {
                throw new ArgumentException("Order in parent cannot be negative.", nameof(orderInParent));
            }

            if (subtasksCount < 0)
            {
                throw new ArgumentException("Subtask count cannot be negative.", nameof(subtasksCount));
            }

            if (subtasksDoneCount < 0 || subtasksDoneCount > subtasksCount)
            {
                throw new ArgumentException("Invalid subtask completion count.", nameof(subtasksDoneCount));
            }

            OrderInParent = orderInParent;
            ParentInstanceId = parentInstanceId;
            SubtasksCount = subtasksCount;
            SubtasksDoneCount = subtasksDoneCount;
        }

        // Immutable update methods
        public TaskHierarchy WithOrder(int newOrder)
        {
            return new TaskHierarchy(newOrder, ParentInstanceId, SubtasksCount, SubtasksDoneCount);
        }

        public TaskHierarchy WithParent(Guid? newParentId)
        {
            return new TaskHierarchy(OrderInParent, newParentId, SubtasksCount, SubtasksDoneCount);
        }

        public TaskHierarchy WithSubtaskCounts(int subtasksCount, int subtasksDoneCount)
        {
            return new TaskHierarchy(OrderInParent, ParentInstanceId, subtasksCount, subtasksDoneCount);
        }

        public TaskHierarchy IncrementSubtasksDone()
        {
            if (SubtasksDoneCount >= SubtasksCount)
            {
                return this;
            }

            return new TaskHierarchy(OrderInParent, ParentInstanceId, SubtasksCount, SubtasksDoneCount + 1);
        }

        public bool IsParent => ParentInstanceId == null;
        public bool HasSubtasks => SubtasksCount > 0;
        public bool AllSubtasksDone => SubtasksCount > 0 && SubtasksDoneCount == SubtasksCount;
        public double SubtaskCompletionRate => SubtasksCount == 0
            ? 0.0
            : (double)SubtasksDoneCount / SubtasksCount;

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return OrderInParent;
            yield return ParentInstanceId;
            yield return SubtasksCount;
            yield return SubtasksDoneCount;
        }
    }
}
