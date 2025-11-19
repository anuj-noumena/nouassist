using System;
using Volo.Abp;

namespace NouAssist.PersonalDevelopment.Habits.Exceptions
{
    public class HabitDomainException : BusinessException
    {
        public HabitDomainException(string code, string? message = null, string? details = null, Exception? innerException = null)
            : base(code, message, details, innerException)
        {
        }

        public static HabitDomainException HabitAlreadyCompleted(DateTime date)
        {
            return new HabitDomainException(
                "Habits:AlreadyCompleted",
                $"Habit already completed for date {date:yyyy-MM-dd}.");
        }

        public static HabitDomainException InvalidFrequency()
        {
            return new HabitDomainException(
                "Habits:InvalidFrequency",
                "Invalid habit frequency configuration.");
        }

        public static HabitDomainException RoutineStepNotFound(Guid stepId)
        {
            return new HabitDomainException(
                "Habits:RoutineStepNotFound",
                $"Routine step with ID {stepId} not found.");
        }

        public static HabitDomainException StreakCalculationError()
        {
            return new HabitDomainException(
                "Habits:StreakCalculationError",
                "Error calculating habit streak.");
        }
    }
}
