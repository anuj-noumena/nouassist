using System;

namespace NouAssist.WorkManagement.TaskManagement.Events
{
    [Serializable]
    public class TaskAcceptanceCriterionUnmetEto
    {
        public Guid TaskInstanceId { get; set; }
        public Guid? TenantId { get; set; }
        public Guid CriterionId { get; set; }
        public DateTime UnmetAt { get; set; }
    }
}
