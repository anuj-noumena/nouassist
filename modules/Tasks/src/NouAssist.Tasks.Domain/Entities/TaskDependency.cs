using System;
using NouAssist.Tasks.Enums;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace NouAssist.Tasks.Entities
{
    public class TaskDependency : Entity
    {
        public Guid? TaskId { get; private set; }
        public Guid? TaskInstanceId { get; private set; }
        public Guid? DependsOnTaskId { get; private set; }
        public Guid? DependsOnTaskInstanceId { get; private set; }
        public DependencyType DependencyType { get; private set; }
        public int LagMinutes { get; private set; }
        public bool IsBlocking { get; private set; }

        protected TaskDependency() { }

        public static TaskDependency CreateTaskDependency(
            Guid taskId,
            Guid dependsOnTaskId,
            DependencyType dependencyType,
            int lagMinutes = 0,
            bool isBlocking = true)
        {
            if (taskId == dependsOnTaskId)
            {
                throw new BusinessException("Tasks:CannotDependOnSelf");
            }

            return new TaskDependency
            {
                TaskId = taskId,
                DependsOnTaskId = dependsOnTaskId,
                DependencyType = dependencyType,
                LagMinutes = lagMinutes,
                IsBlocking = isBlocking
            };
        }

        public static TaskDependency CreateInstanceDependency(
            Guid taskInstanceId,
            Guid dependsOnTaskInstanceId,
            DependencyType dependencyType,
            int lagMinutes = 0,
            bool isBlocking = true)
        {
            if (taskInstanceId == dependsOnTaskInstanceId)
            {
                throw new BusinessException("Tasks:CannotDependOnSelf");
            }

            return new TaskDependency
            {
                TaskInstanceId = taskInstanceId,
                DependsOnTaskInstanceId = dependsOnTaskInstanceId,
                DependencyType = dependencyType,
                LagMinutes = lagMinutes,
                IsBlocking = isBlocking
            };
        }

        public bool IsTaskDependency => TaskId.HasValue;
        public bool IsInstanceDependency => TaskInstanceId.HasValue;
        public TimeSpan LagDuration => TimeSpan.FromMinutes(LagMinutes);

        public override object[] GetKeys()
        {
            if (IsTaskDependency)
            {
                return new object[] { TaskId, DependsOnTaskId };
            }

            return new object[] { TaskInstanceId, DependsOnTaskInstanceId };
        }
    }
}
