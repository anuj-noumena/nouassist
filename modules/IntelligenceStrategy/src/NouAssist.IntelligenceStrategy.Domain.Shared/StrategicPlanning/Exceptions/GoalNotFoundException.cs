using System;

namespace NouAssist.IntelligenceStrategy.StrategicPlanning.Exceptions
{
    /// <summary>
    /// Exception thrown when a strategic goal is not found
    /// </summary>
    public class GoalNotFoundException : StrategicPlanningDomainException
    {
        public GoalNotFoundException(Guid goalId)
            : base("StrategicPlanning:ErrorMessage:GoalNotFound")
        {
            WithData("goalId", goalId);
        }
    }
}
