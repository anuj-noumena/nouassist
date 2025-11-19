using System;
using Volo.Abp;

namespace NouAssist.PersonalDevelopment.Goals.Exceptions
{
    public class GoalDomainException : BusinessException
    {
        public GoalDomainException(string code, string? message = null, string? details = null, Exception? innerException = null)
            : base(code, message, details, innerException)
        {
        }

        public static GoalDomainException InvalidProgress()
        {
            return new GoalDomainException(
                "Goals:InvalidProgress",
                "Progress must be between 0 and 100.");
        }

        public static GoalDomainException AlreadyAchieved()
        {
            return new GoalDomainException(
                "Goals:AlreadyAchieved",
                "Goal is already achieved.");
        }

        public static GoalDomainException CanOnlyPauseActive()
        {
            return new GoalDomainException(
                "Goals:CanOnlyPauseActive",
                "Can only pause active goals.");
        }

        public static GoalDomainException CanOnlyResumeOnHold()
        {
            return new GoalDomainException(
                "Goals:CanOnlyResumeOnHold",
                "Can only resume on-hold goals.");
        }

        public static GoalDomainException DuplicateMilestone()
        {
            return new GoalDomainException(
                "Goals:DuplicateMilestone",
                "A milestone with this title already exists.");
        }
    }
}
