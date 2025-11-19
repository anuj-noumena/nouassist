using System;
using System.Collections.Generic;
using System.Linq;
using NouAssist.PersonalDevelopment.Habits.Entities;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace NouAssist.PersonalDevelopment.Habits.Aggregates
{
    public class Routine : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Guid OwnerId { get; private set; }
        public string Title { get; private set; }
        public string Description { get; set; }
        public string TriggerDescription { get; set; }
        public string TimeOfDay { get; set; }
        public bool IsActive { get; private set; }
        public Guid? TenantId { get; private set; }

        private readonly List<int> _scheduledDays = new();
        public IReadOnlyCollection<int> ScheduledDays => _scheduledDays.AsReadOnly();

        private readonly List<RoutineStep> _steps = new();
        public IReadOnlyCollection<RoutineStep> Steps => _steps.AsReadOnly();

        protected Routine() { /* ORM */ }

        public Routine(
            Guid id,
            Guid ownerId,
            string title,
            Guid? tenantId = null
        ) : base(id)
        {
            OwnerId = Check.NotNull(ownerId, nameof(ownerId));
            Title = Check.NotNullOrWhiteSpace(title, nameof(title), maxLength: 200);
            TenantId = tenantId;
            IsActive = true;
        }

        public bool HasSchedule => _scheduledDays.Any();
        public bool IsEveryday => _scheduledDays.Count == 7;
        public int TotalSteps => _steps.Count;

        public void AddStep(Guid stepId, int stepNumber, Guid? habitId = null, string adhocTaskDescription = null, int? estimatedDurationMinutes = null)
        {
            if (habitId == null && string.IsNullOrWhiteSpace(adhocTaskDescription))
            {
                throw new BusinessException("Habits:InvalidRoutineStep", 
                    "Step must have either a habit or adhoc task description");
            }

            if (habitId != null && !string.IsNullOrWhiteSpace(adhocTaskDescription))
            {
                throw new BusinessException("Habits:InvalidRoutineStep", 
                    "Step cannot have both habit and adhoc task");
            }

            if (_steps.Any(s => s.StepNumber == stepNumber))
            {
                throw new BusinessException("Habits:DuplicateStepNumber", 
                    $"Step number {stepNumber} already exists");
            }

            var step = new RoutineStep(stepId, Id, stepNumber, habitId, adhocTaskDescription, estimatedDurationMinutes);
            _steps.Add(step);
        }

        public void RemoveStep(Guid stepId)
        {
            var step = _steps.FirstOrDefault(s => s.Id == stepId);
            if (step == null)
            {
                throw new BusinessException("Habits:StepNotFound", 
                    "Step not found");
            }
            _steps.Remove(step);
        }

        public void ReorderSteps(Dictionary<Guid, int> newOrder)
        {
            foreach (var kvp in newOrder)
            {
                var step = _steps.FirstOrDefault(s => s.Id == kvp.Key);
                if (step != null)
                {
                    step.UpdateStepNumber(kvp.Value);
                }
            }
        }

        public void UpdateSchedule(List<int> scheduledDays)
        {
            if (scheduledDays.Any(d => d < 1 || d > 7))
            {
                throw new BusinessException("Habits:InvalidSchedule", 
                    "Day numbers must be between 1 and 7");
            }

            _scheduledDays.Clear();
            _scheduledDays.AddRange(scheduledDays.Distinct());
        }

        public void Activate()
        {
            if (IsActive)
            {
                return;
            }

            IsActive = true;
        }

        public void Deactivate()
        {
            if (!IsActive)
            {
                return;
            }

            IsActive = false;
        }

        public void UpdateTitle(string newTitle)
        {
            Title = Check.NotNullOrWhiteSpace(newTitle, nameof(newTitle), maxLength: 200);
        }
    }
}
