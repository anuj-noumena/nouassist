using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace NouAssist.WorkManagement.TaskManagement.Entities
{
    public class TaskActionStepCompletion : FullAuditedEntity
    {
        public Guid TaskInstanceId { get; private set; }
        public Guid TaskActionStepId { get; private set; }
        public bool IsDone { get; private set; }
        public DateTime? CompletedAt { get; private set; }
        public Guid? CompletedBy { get; private set; }

        protected TaskActionStepCompletion() { }

        public TaskActionStepCompletion(Guid taskInstanceId, Guid taskActionStepId)
        {
            TaskInstanceId = taskInstanceId;
            TaskActionStepId = taskActionStepId;
            IsDone = false;
        }

        public void Complete(Guid completedBy)
        {
            IsDone = true;
            CompletedAt = DateTime.UtcNow;
            CompletedBy = completedBy;
        }

        public void Uncomplete()
        {
            IsDone = false;
            CompletedAt = null;
            CompletedBy = null;
        }

        public override object[] GetKeys()
        {
            return new object[] { TaskInstanceId, TaskActionStepId };
        }
    }
}
