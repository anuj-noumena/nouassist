using System;
using System.Collections.Generic;
using System.Linq;
using NouAssist.WorkManagement.TaskManagement.Enums;
using NouAssist.WorkManagement.TaskManagement.Events;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace NouAssist.WorkManagement.TaskManagement.Aggregates
{
    /// <summary>
    /// TaskDependencyAggregate - Manages dependencies for a task instance
    /// Promoted to aggregate root for better lifecycle management and independent scaling
    /// </summary>
    public class TaskDependencyAggregate : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        // === IDENTITY ===
        public Guid TaskInstanceId { get; private set; }
        public Guid? TenantId { get; private set; }

        // === DEPENDENCIES ===
        private readonly List<DependencyItem> _dependencies = new();
        public IReadOnlyCollection<DependencyItem> Dependencies => _dependencies.AsReadOnly();

        protected TaskDependencyAggregate() { }

        public TaskDependencyAggregate(
            Guid id,
            Guid taskInstanceId,
            Guid? tenantId = null) : base(id)
        {
            TaskInstanceId = taskInstanceId;
            TenantId = tenantId;

            AddDistributedEvent(new TaskDependencyAggregateCreatedEto
            {
                TaskDependencyAggregateId = Id,
                TaskInstanceId = TaskInstanceId,
                TenantId = TenantId,
                CreatedAt = DateTime.UtcNow
            });
        }

        // === DEPENDENCY MANAGEMENT ===

        /// <summary>
        /// Adds a dependency on another task instance
        /// </summary>
        public void AddDependency(
            Guid dependsOnTaskInstanceId,
            DependencyType type,
            int lagMinutes = 0,
            bool isBlocking = true)
        {
            if (dependsOnTaskInstanceId == TaskInstanceId)
            {
                throw new BusinessException("Tasks:CannotDependOnSelf",
                    "A task cannot depend on itself.");
            }

            if (_dependencies.Any(d => d.DependsOnTaskInstanceId == dependsOnTaskInstanceId))
            {
                throw new BusinessException("Tasks:DependencyAlreadyExists",
                    "This dependency already exists.");
            }

            var dependency = new DependencyItem(
                dependsOnTaskInstanceId,
                type,
                lagMinutes,
                isBlocking);

            _dependencies.Add(dependency);

            AddDistributedEvent(new TaskDependencyAddedEto
            {
                TaskInstanceId = TaskInstanceId,
                TenantId = TenantId,
                DependsOnTaskInstanceId = dependsOnTaskInstanceId,
                DependencyType = type.ToString(),
                LagMinutes = lagMinutes,
                IsBlocking = isBlocking,
                AddedAt = DateTime.UtcNow
            });
        }

        /// <summary>
        /// Removes a dependency
        /// </summary>
        public void RemoveDependency(Guid dependsOnTaskInstanceId)
        {
            var dependency = _dependencies.FirstOrDefault(d =>
                d.DependsOnTaskInstanceId == dependsOnTaskInstanceId);

            if (dependency != null)
            {
                _dependencies.Remove(dependency);

                AddDistributedEvent(new TaskDependencyRemovedEto
                {
                    TaskInstanceId = TaskInstanceId,
                    TenantId = TenantId,
                    DependsOnTaskInstanceId = dependsOnTaskInstanceId,
                    RemovedAt = DateTime.UtcNow
                });
            }
        }

        /// <summary>
        /// Updates a dependency's properties
        /// </summary>
        public void UpdateDependency(
            Guid dependsOnTaskInstanceId,
            DependencyType? type = null,
            int? lagMinutes = null,
            bool? isBlocking = null)
        {
            var dependency = _dependencies.FirstOrDefault(d =>
                d.DependsOnTaskInstanceId == dependsOnTaskInstanceId);

            if (dependency == null)
            {
                throw new BusinessException("Tasks:DependencyNotFound");
            }

            dependency.Update(type, lagMinutes, isBlocking);
        }

        /// <summary>
        /// Removes all dependencies
        /// </summary>
        public void ClearAllDependencies()
        {
            _dependencies.Clear();

            AddDistributedEvent(new TaskDependenciesClearedEto
            {
                TaskInstanceId = TaskInstanceId,
                TenantId = TenantId,
                ClearedAt = DateTime.UtcNow
            });
        }

        // === COMPUTED PROPERTIES ===
        public bool HasDependencies => _dependencies.Any();
        public bool HasBlockingDependencies => _dependencies.Any(d => d.IsBlocking);
        public int DependencyCount => _dependencies.Count;
        public int BlockingDependencyCount => _dependencies.Count(d => d.IsBlocking);

        /// <summary>
        /// DependencyItem - Value object representing a single dependency
        /// </summary>
        public class DependencyItem
        {
            public Guid DependsOnTaskInstanceId { get; private set; }
            public DependencyType DependencyType { get; private set; }
            public int LagMinutes { get; private set; }
            public bool IsBlocking { get; private set; }

            protected DependencyItem() { }

            public DependencyItem(
                Guid dependsOnTaskInstanceId,
                DependencyType dependencyType,
                int lagMinutes,
                bool isBlocking)
            {
                DependsOnTaskInstanceId = dependsOnTaskInstanceId;
                DependencyType = dependencyType;
                LagMinutes = lagMinutes;
                IsBlocking = isBlocking;
            }

            public void Update(
                DependencyType? type = null,
                int? lagMinutes = null,
                bool? isBlocking = null)
            {
                if (type.HasValue)
                    DependencyType = type.Value;

                if (lagMinutes.HasValue)
                    LagMinutes = lagMinutes.Value;

                if (isBlocking.HasValue)
                    IsBlocking = isBlocking.Value;
            }

            public TimeSpan LagDuration => TimeSpan.FromMinutes(LagMinutes);
        }
    }
}
