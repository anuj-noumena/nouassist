using System;
using Volo.Abp.Domain.Entities;

namespace NouAssist.PersonalDevelopment.Habits.Entities
{
    public class RoutineCompletedStep : Entity<Guid>
    {
        public Guid CompletionId { get; private set; }
        public Guid StepId { get; private set; }
        public DateTime CompletedAt { get; private set; }

        protected RoutineCompletedStep() { /* ORM */ }

        public RoutineCompletedStep(Guid id, Guid completionId, Guid stepId)
        {
            Id = id;
            CompletionId = completionId;
            StepId = stepId;
            CompletedAt = DateTime.UtcNow;
        }
    }
}
