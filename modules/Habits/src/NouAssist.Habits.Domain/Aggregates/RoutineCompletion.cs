using System;
using System.Collections.Generic;
using System.Linq;
using NouAssist.Habits.Entities;
using NouAssist.Habits.Enums;
using Volo.Abp;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace NouAssist.Habits.Aggregates
{
    public class RoutineCompletion : AggregateRoot<Guid>, IMultiTenant
    {
        public Guid OwnerId { get; private set; }
        public Guid RoutineId { get; private set; }
        public DateTime LogDate { get; private set; }
        public RoutineCompletionStatus Status { get; private set; }
        public string Notes { get; set; }
        public int Completed { get; private set; }
        public int? TotalSteps { get; private set; }
        public Guid? TenantId { get; private set; }

        private readonly List<RoutineCompletedStep> _completedSteps = new();
        public IReadOnlyCollection<RoutineCompletedStep> CompletedSteps => _completedSteps.AsReadOnly();

        protected RoutineCompletion() { /* ORM */ }

        public RoutineCompletion(
            Guid id,
            Guid ownerId,
            Guid routineId,
            DateTime logDate,
            int? totalSteps = null,
            Guid? tenantId = null
        ) : base(id)
        {
            OwnerId = Check.NotNull(ownerId, nameof(ownerId));
            RoutineId = routineId;
            LogDate = new DateTime(logDate.Year, logDate.Month, logDate.Day);
            Status = RoutineCompletionStatus.Skipped;
            Completed = 0;
            TotalSteps = totalSteps;
            TenantId = tenantId;
        }

        public bool IsCompleted => Status == RoutineCompletionStatus.Completed;
        public bool IsPartiallyCompleted => Status == RoutineCompletionStatus.PartiallyCompleted;
        public bool IsSkipped => Status == RoutineCompletionStatus.Skipped;

        public double CompletionRate
        {
            get
            {
                if (!TotalSteps.HasValue || TotalSteps.Value == 0) return 0.0;
                return (double)Completed / TotalSteps.Value;
            }
        }

        public void CompleteStep(Guid stepId)
        {
            if (_completedSteps.Any(s => s.StepId == stepId))
            {
                return; // Already completed
            }

            var completedStep = new RoutineCompletedStep(Guid.NewGuid(), Id, stepId);
            _completedSteps.Add(completedStep);
            Completed = _completedSteps.Count;

            UpdateStatus();
        }

        public void MarkAsCompleted()
        {
            Status = RoutineCompletionStatus.Completed;
            Completed = TotalSteps ?? Completed;
        }

        public void MarkAsPartiallyCompleted(int stepsCompleted)
        {
            if (stepsCompleted < 0 || (TotalSteps.HasValue && stepsCompleted > TotalSteps.Value))
            {
                throw new BusinessException("Habits:InvalidCompletedSteps", 
                    "Invalid number of completed steps");
            }

            Status = RoutineCompletionStatus.PartiallyCompleted;
            Completed = stepsCompleted;
        }

        public void AddNotes(string notes)
        {
            Notes = notes;
        }

        private void UpdateStatus()
        {
            if (TotalSteps.HasValue)
            {
                if (Completed >= TotalSteps.Value)
                {
                    Status = RoutineCompletionStatus.Completed;
                }
                else if (Completed > 0)
                {
                    Status = RoutineCompletionStatus.PartiallyCompleted;
                }
                else
                {
                    Status = RoutineCompletionStatus.Skipped;
                }
            }
            else if (Completed > 0)
            {
                Status = RoutineCompletionStatus.PartiallyCompleted;
            }
        }
    }
}
