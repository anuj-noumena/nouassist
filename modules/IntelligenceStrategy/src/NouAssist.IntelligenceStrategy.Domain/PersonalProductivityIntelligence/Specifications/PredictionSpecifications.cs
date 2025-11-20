using System;
using System.Linq.Expressions;
using NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Aggregates;
using NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Enums;
using Volo.Abp.Specifications;

namespace NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Specifications
{
    /// <summary>
    /// Base specification for predictions
    /// </summary>
    public abstract class PredictionSpecification : Specification<PredictionInstance>
    {
    }

    /// <summary>
    /// Specification for high confidence predictions
    /// </summary>
    public class HighConfidencePredictionSpecification : PredictionSpecification
    {
        public override Expression<Func<PredictionInstance, bool>> ToExpression()
        {
            return p => p.ConfidenceLevel >= PredictionConfidenceLevel.High;
        }
    }

    /// <summary>
    /// Specification for predictions of a specific type
    /// </summary>
    public class PredictionTypeSpecification : PredictionSpecification
    {
        private readonly string _predictionType;

        public PredictionTypeSpecification(string predictionType)
        {
            _predictionType = predictionType;
        }

        public override Expression<Func<PredictionInstance, bool>> ToExpression()
        {
            return p => p.PredictionType == _predictionType;
        }
    }

    /// <summary>
    /// Specification for recent predictions
    /// </summary>
    public class RecentPredictionSpecification : PredictionSpecification
    {
        private readonly int _hoursThreshold;

        public RecentPredictionSpecification(int hoursThreshold = 24)
        {
            _hoursThreshold = hoursThreshold;
        }

        public override Expression<Func<PredictionInstance, bool>> ToExpression()
        {
            var threshold = DateTime.UtcNow.AddHours(-_hoursThreshold);
            return p => p.PredictionTime >= threshold;
        }
    }

    /// <summary>
    /// Specification for predictions about a specific entity
    /// </summary>
    public class EntityPredictionSpecification : PredictionSpecification
    {
        private readonly string _targetEntityType;
        private readonly Guid _targetEntityId;

        public EntityPredictionSpecification(string targetEntityType, Guid targetEntityId)
        {
            _targetEntityType = targetEntityType;
            _targetEntityId = targetEntityId;
        }

        public override Expression<Func<PredictionInstance, bool>> ToExpression()
        {
            return p => p.TargetEntityType == _targetEntityType && p.TargetEntityId == _targetEntityId;
        }
    }
}
