using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace NouAssist.WorkManagement.TaskManagement.Entities
{
    public class TaskActionStep : FullAuditedEntity<Guid>
    {
        public Guid TaskId { get; private set; }
        public string Title { get; private set; }
        public string Notes { get; private set; }
        public bool IsRequired { get; private set; }
        public int Sequence { get; private set; }

        protected TaskActionStep() { }

        public TaskActionStep(
            Guid actionStepId,
            Guid taskId,
            string title,
            string notes = null,
            bool isRequired = false,
            int sequence = 0)
        {
            Id = actionStepId;
            TaskId = taskId;
            Title = Check.NotNullOrWhiteSpace(title, nameof(title), maxLength: 500);
            Notes = notes;
            IsRequired = isRequired;
            Sequence = sequence;
        }

        public void Update(string title, string notes, bool isRequired)
        {
            Title = Check.NotNullOrWhiteSpace(title, nameof(title), maxLength: 500);
            Notes = notes;
            IsRequired = isRequired;
        }

        public void UpdateSequence(int newSequence)
        {
            Sequence = newSequence;
        }
    }
}
