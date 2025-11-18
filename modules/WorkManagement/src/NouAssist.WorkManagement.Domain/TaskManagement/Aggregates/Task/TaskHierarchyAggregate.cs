using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace NouAssist.WorkManagement.TaskManagement.Aggregates.Task
{
    /// <summary>
    /// Manages the hierarchical relationship between task blueprints (parent/child).
    /// </summary>
    public class TaskHierarchyAggregate : FullAuditedAggregateRoot<Guid>
    {
        public Guid TaskId { get; private set; }

        // === Properties from the TaskHierarchy Value Object are now inlined ===
        public Guid? ParentId { get; private set; }
        public int Level { get; private set; }
        public string Path { get; private set; } // e.g., "root/parent/child"

        protected TaskHierarchyAggregate() { }

        public TaskHierarchyAggregate(Guid id, Guid taskId) : base(id)
        {
            TaskId = taskId;
            
            // Logic from TaskHierarchy.Root() is now directly in the constructor
            ParentId = null;
            Level = 0;
            Path = "/";
        }

        // === HIERARCHY MANAGEMENT ===

        /// <summary>
        /// Updates the entire hierarchy state with explicit values.
        /// This replaces the original UpdateHierarchy which took a Value Object.
        /// </summary>
        public void UpdateHierarchy(Guid? parentId, int level, string path)
        {
            ParentId = parentId;
            Level = level;
            Path = path;
            AddDistributedEvent(new TaskHierarchyUpdatedEto { TaskHierarchyId = Id, ParentId = ParentId });
        }

        /// <summary>
        /// Sets the parent for this task.
        /// NOTE: The signature has changed. Since the Value Object's factory logic is now inside the aggregate,
        /// we need the parent's hierarchy information (Level and Path) to correctly calculate this task's new state.
        /// In a real application, you might retrieve the parent aggregate first to get this information.
        /// </summary>
        /// <param name="parentTaskId">The ID of the new parent task.</param>
        /// <param name="parentLevel">The hierarchy level of the parent task.</param>
        /// <param name="parentPath">The full path of the parent task.</param>
        public void SetParent(Guid parentTaskId, int parentLevel, string parentPath)
        {
            // Logic from TaskHierarchy.Child() is now directly in this method
            ParentId = parentTaskId;
            Level = parentLevel + 1;
            // The path is constructed from the parent's path and the current task's ID.
            // This is a more logical implementation for a path. The original Value Object's
            // Path implementation was slightly ambiguous. This version is more explicit.
            Path = $"{parentPath.TrimEnd('/')}/{Id}";

            AddDistributedEvent(new TaskHierarchyUpdatedEto { TaskHierarchyId = Id, ParentId = ParentId });
        }
        
        // === Helper Properties from the Value Object ===
        public bool IsRoot => !ParentId.HasValue;
        public bool IsChild => ParentId.HasValue;
    }
}