using System;
using System.Linq.Expressions;
using NouAssist.BusinessOperations.CustomerRelationshipManagement.Aggregates;
using NouAssist.BusinessOperations.CustomerRelationshipManagement.Enums;

namespace NouAssist.BusinessOperations.CustomerRelationshipManagement.Specifications
{
    /// <summary>
    /// Base specification class for InteractionLog aggregate
    /// </summary>
    public abstract class InteractionLogSpecification : Specification<InteractionLog>
    {
    }

    /// <summary>
    /// Specification for interaction logs related to a specific entity
    /// </summary>
    public class EntityInteractionSpecification : InteractionLogSpecification
    {
        private readonly Guid _entityId;
        private readonly string _entityType;

        public EntityInteractionSpecification(Guid entityId, string entityType)
        {
            _entityId = entityId;
            _entityType = entityType;
        }

        public override Expression<Func<InteractionLog, bool>> ToExpression()
        {
            return log => 
                log.EntityId == _entityId && 
                log.EntityType == _entityType;
        }
    }

    /// <summary>
    /// Specification for interaction logs with pending follow-up tasks
    /// </summary>
    public class PendingFollowUpSpecification : InteractionLogSpecification
    {
        private readonly DateTime _now;

        public PendingFollowUpSpecification(DateTime now)
        {
            _now = now;
        }

        public override Expression<Func<InteractionLog, bool>> ToExpression()
        {
            return log => 
                log.FollowUpTasks.Any(t => 
                    t.Status == FollowUpStatus.Pending && 
                    t.DueDate <= _now);
        }
    }

    /// <summary>
    /// Specification for interaction logs with a specific type of communication
    /// </summary>
    public class InteractionWithCommunicationTypeSpecification : InteractionLogSpecification
    {
        private readonly InteractionType _type;

        public InteractionWithCommunicationTypeSpecification(InteractionType type)
        {
            _type = type;
        }

        public override Expression<Func<InteractionLog, bool>> ToExpression()
        {
            return log => 
                log.CommunicationRecords.Any(c => c.Type == _type);
        }
    }

    /// <summary>
    /// Specification for interactions in a specific date range
    /// </summary>
    public class InteractionInDateRangeSpecification : InteractionLogSpecification
    {
        private readonly DateTime _startDate;
        private readonly DateTime _endDate;

        public InteractionInDateRangeSpecification(DateTime startDate, DateTime endDate)
        {
            _startDate = startDate;
            _endDate = endDate;
        }

        public override Expression<Func<InteractionLog, bool>> ToExpression()
        {
            return log => 
                log.CommunicationRecords.Any(c => 
                    c.CommunicationTime >= _startDate && 
                    c.CommunicationTime <= _endDate);
        }
    }
}
