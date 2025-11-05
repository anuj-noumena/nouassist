using System;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;

namespace NouAssist.Tasks.Entities
{
    public class AcceptanceCriterionCompletion : FullAuditedEntity
    {
        public Guid TaskInstanceId { get; private set; }
        public Guid CriterionId { get; private set; }
        public bool IsMet { get; private set; }
        public DateTime? MetAt { get; private set; }
        public Guid? MetBy { get; private set; }

        protected AcceptanceCriterionCompletion() { }

        public AcceptanceCriterionCompletion(Guid taskInstanceId, Guid criterionId)
        {
            TaskInstanceId = taskInstanceId;
            CriterionId = criterionId;
            IsMet = false;
        }

        public void MarkAsMet(Guid metBy)
        {
            IsMet = true;
            MetAt = DateTime.UtcNow;
            MetBy = metBy;
        }

        public void UnmarkAsMet()
        {
            IsMet = false;
            MetAt = null;
            MetBy = null;
        }

        public override object[] GetKeys()
        {
            return new object[] { TaskInstanceId, CriterionId };
        }
    }
}
