namespace NouAssist.IntelligenceStrategy.StrategicPlanning.Specifications
{
    /// <summary>
    /// Base specification class for Strategic Goal aggregate
    /// </summary>
    public abstract class StrategicGoalSpecification : Specification<StrategicGoalDefinition>
    {
    }

    /// <summary>
    /// Specification for active strategic goals
    /// </summary>
    public class ActiveStrategicGoalsSpecification : StrategicGoalSpecification
    {
        public override Expression<Func<StrategicGoalDefinition, bool>> ToExpression()
        {
            return goal => goal.Status == GoalStatus.Active;
        }
    }

    /// <summary>
    /// Specification for strategic goals by priority
    /// </summary>
    public class GoalsByPrioritySpecification : StrategicGoalSpecification
    {
        private readonly GoalPriority _priority;

        public GoalsByPrioritySpecification(GoalPriority priority)
        {
            _priority = priority;
        }

        public override Expression<Func<StrategicGoalDefinition, bool>> ToExpression()
        {
            return goal => goal.Priority == _priority;
        }
    }

    /// <summary>
    /// Specification for strategic goals by planning horizon
    /// </summary>
    public class GoalsByHorizonSpecification : StrategicGoalSpecification
    {
        private readonly PlanningHorizon _horizon;

        public GoalsByHorizonSpecification(PlanningHorizon horizon)
        {
            _horizon = horizon;
        }

        public override Expression<Func<StrategicGoalDefinition, bool>> ToExpression()
        {
            return goal => goal.Horizon == _horizon;
        }
    }

    /// <summary>
    /// Specification for strategic goals that are behind schedule
    /// </summary>
    public class GoalsBehindScheduleSpecification : StrategicGoalSpecification
    {
        public override Expression<Func<StrategicGoalDefinition, bool>> ToExpression()
        {
            var now = DateTime.UtcNow;
            return goal => goal.Status == GoalStatus.Active && 
                          goal.TargetDate > now &&
                          !goal.IsOnTrack;
        }
    }

    /// <summary>
    /// Specification for strategic goals due within a specific timeframe
    /// </summary>
    public class GoalsDueWithinTimeframeSpecification : StrategicGoalSpecification
    {
        private readonly DateTime _fromDate;
        private readonly DateTime _toDate;

        public GoalsDueWithinTimeframeSpecification(DateTime fromDate, DateTime toDate)
        {
            _fromDate = fromDate;
            _toDate = toDate;
        }

        public override Expression<Func<StrategicGoalDefinition, bool>> ToExpression()
        {
            return goal => goal.TargetDate >= _fromDate && goal.TargetDate <= _toDate;
        }
    }

    /// <summary>
    /// Specification for strategic goals assigned to a specific owner
    /// </summary>
    public class GoalsByOwnerSpecification : StrategicGoalSpecification
    {
        private readonly Guid _ownerId;

        public GoalsByOwnerSpecification(Guid ownerId)
        {
            _ownerId = ownerId;
        }

        public override Expression<Func<StrategicGoalDefinition, bool>> ToExpression()
        {
            return goal => goal.OwnerId == _ownerId;
        }
    }
}
