using System;

namespace NouAssist.WorkManagement.TaskManagement.Events
{
    [Serializable]
    public class TaskAcceptanceCriterionAddedEto
    {
        public Guid TaskId { get; set; }
        public Guid? TenantId { get; set; }
        public Guid CriterionId { get; set; }
        public string Description { get; set; }
        public int? Priority { get; set; }
        public int Version { get; set; }
        public DateTime AddedAt { get; set; }
    }
}
