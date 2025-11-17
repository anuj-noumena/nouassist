using System;

namespace NouAssist.WorkManagement.TaskManagement.Events
{
    [Serializable]
    public class TaskAcceptanceCriterionMetEto
    {
        public Guid TaskInstanceId { get; set; }
        public Guid? TenantId { get; set; }
        public Guid CriterionId { get; set; }
        public Guid MetBy { get; set; }
        public DateTime MetAt { get; set; }
    }
}
