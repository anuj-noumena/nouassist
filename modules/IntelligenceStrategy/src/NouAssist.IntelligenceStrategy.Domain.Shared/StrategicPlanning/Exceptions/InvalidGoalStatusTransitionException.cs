using NouAssist.IntelligenceStrategy.StrategicPlanning.Enums;

namespace NouAssist.IntelligenceStrategy.StrategicPlanning.Exceptions
{
    /// <summary>
    /// Exception thrown when an invalid goal status transition is attempted
    /// </summary>
    public class InvalidGoalStatusTransitionException : StrategicPlanningDomainException
    {
        public InvalidGoalStatusTransitionException(GoalStatus currentStatus, GoalStatus targetStatus)
            : base("StrategicPlanning:ErrorMessage:InvalidStatusTransition")
        {
            WithData("currentStatus", currentStatus);
            WithData("targetStatus", targetStatus);
        }
    }
}
