using System;
using System.Collections.Generic;
using System.Linq;
using NouAssist.WorkManagement.TaskManagement.Entities;
using NouAssist.WorkManagement.TaskManagement.Enums;
using NouAssist.WorkManagement.TaskManagement.Events;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace NouAssist.WorkManagement.TaskManagement.Aggregates
{
    /// <summary>
    /// Task Dependencies - Manages task dependency graph relationships for a TaskInstance
    /// </summary>
    public class TaskDependencies : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        // === IDENTITY ===
        public Guid TaskInstanceId { get; private set; }
        public Guid? TenantId { get; private set; }
        public int Version { get; private set; }

        // === DEPENDENCIES ===
        private readonly List<TaskDependency> _dependencies = new();
        public IReadOnlyCollection<TaskDependency> Dependencies => _dependencies.AsReadOnly();

        protected TaskDependencies() { }

        public TaskDependencies(
            Guid id,
            Guid taskInstanceId,
            Guid? tenantId = null) : base(id)
        {
            TaskInstanceId = taskInstanceId;
            TenantId = tenantId;
            Version = 1;
        }

        // === DEPENDENCY MANAGEMENT ===
        public void AddDependency(
            Guid dependsOnTaskInstanceId,
            DependencyType type,
            int lagMinutes = 0,
            bool isBlocking = true)
        {
            if (dependsOnTaskInstanceId == TaskInstanceId)
            {
                throw new BusinessException("Tasks:CannotDependOnSelf");
            }

            if (_dependencies.Any(d => d.DependsOnTaskInstanceId == dependsOnTaskInstanceId))
            {
                throw new BusinessException("Tasks:DependencyAlreadyExists");
            }

            var dependency = TaskDependency.CreateInstanceDependency(
                TaskInstanceId, dependsOnTaskInstanceId, type, lagMinutes, isBlocking);

            _dependencies.Add(dependency);
            Version++;

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

        public void RemoveDependency(Guid dependsOnTaskInstanceId)
        {
            var dependency = _dependencies.FirstOrDefault(d =>
                d.DependsOnTaskInstanceId == dependsOnTaskInstanceId);

            if (dependency != null)
            {
                _dependencies.Remove(dependency);
                Version++;

                AddDistributedEvent(new TaskDependencyRemovedEto
                {
                    TaskInstanceId = TaskInstanceId,
                    TenantId = TenantId,
                    DependsOnTaskInstanceId = dependsOnTaskInstanceId,
                    RemovedAt = DateTime.UtcNow
                });
            }
        }

        // === COMPUTED PROPERTIES ===
        public bool HasDependencies => _dependencies.Any();
        public bool HasBlockingDependencies => _dependencies.Any(d => d.IsBlocking);
        public int TotalDependencies => _dependencies.Count;
        public int BlockingDependenciesCount => _dependencies.Count(d => d.IsBlocking);
    }
}
