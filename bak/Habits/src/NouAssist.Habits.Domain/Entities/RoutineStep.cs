using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace NouAssist.Habits.Entities
{
    public class RoutineStep : Entity<Guid>
    {
        public Guid RoutineId { get; private set; }
        public int StepNumber { get; private set; }
        public Guid? HabitId { get; private set; }
        public string AdhocTaskDescription { get; private set; }
        public int? EstimatedDurationMinutes { get; set; }

        protected RoutineStep() { /* ORM */ }

        public RoutineStep(
            Guid id,
            Guid routineId,
            int stepNumber,
            Guid? habitId,
            string adhocTaskDescription,
            int? estimatedDurationMinutes
        )
        {
            Id = id;
            RoutineId = routineId;
            StepNumber = stepNumber;
            HabitId = habitId;
            AdhocTaskDescription = adhocTaskDescription;
            EstimatedDurationMinutes = estimatedDurationMinutes;
        }

        public bool IsHabitStep => HabitId.HasValue;
        public bool IsAdhocStep => !string.IsNullOrWhiteSpace(AdhocTaskDescription);

        public void UpdateStepNumber(int newStepNumber)
        {
            if (newStepNumber < 1)
            {
                throw new BusinessException("Habits:InvalidStepNumber", 
                    "Step number must be positive");
            }
            StepNumber = newStepNumber;
        }

        public void UpdateDuration(int minutes)
        {
            if (minutes < 0)
            {
                throw new BusinessException("Habits:InvalidDuration", 
                    "Duration cannot be negative");
            }
            EstimatedDurationMinutes = minutes;
        }
    }
}
