using System;
using System.Linq;
using System.Linq.Expressions;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.Aggregates;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.Enums;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.Specifications.Base;

namespace NouAssist.StartupEntrepreneurship.BusinessModelManagement.Specifications
{
    /// <summary>
    /// Specification for active strategic pivots
    /// </summary>
    public class ActivePivotSpecification : BusinessModelManagementSpecification<StrategicPivot>
    {
        public override Expression<Func<StrategicPivot, bool>> ToExpression()
        {
            return pivot => pivot.Status == PivotStatus.InProgress || 
                          pivot.Status == PivotStatus.Planned;
        }
    }

    /// <summary>
    /// Specification for overdue pivots
    /// </summary>
    public class OverduePivotSpecification : BusinessModelManagementSpecification<StrategicPivot>
    {
        private readonly DateTime _currentDate;

        public OverduePivotSpecification(DateTime currentDate)
        {
            _currentDate = currentDate;
        }

        public override Expression<Func<StrategicPivot, bool>> ToExpression()
        {
            return pivot => pivot.Status != PivotStatus.Completed && 
                          pivot.Status != PivotStatus.Abandoned && 
                          pivot.TargetCompletionDate < _currentDate;
        }
    }

    /// <summary>
    /// Specification for successful pivots
    /// </summary>
    public class SuccessfulPivotSpecification : BusinessModelManagementSpecification<StrategicPivot>
    {
        public override Expression<Func<StrategicPivot, bool>> ToExpression()
        {
            return pivot => pivot.Status == PivotStatus.Completed && 
                          pivot.WasSuccessful;
        }
    }

    /// <summary>
    /// Specification for pivots of a specific type
    /// </summary>
    public class PivotTypeSpecification : BusinessModelManagementSpecification<StrategicPivot>
    {
        private readonly PivotType _type;

        public PivotTypeSpecification(PivotType type)
        {
            _type = type;
        }

        public override Expression<Func<StrategicPivot, bool>> ToExpression()
        {
            return pivot => pivot.Type == _type;
        }
    }

    /// <summary>
    /// Specification for pivots initiated by a specific user
    /// </summary>
    public class PivotInitiatedByUserSpecification : BusinessModelManagementSpecification<StrategicPivot>
    {
        private readonly Guid _userId;

        public PivotInitiatedByUserSpecification(Guid userId)
        {
            _userId = userId;
        }

        public override Expression<Func<StrategicPivot, bool>> ToExpression()
        {
            return pivot => pivot.InitiatedBy == _userId;
        }
    }

    /// <summary>
    /// Specification for pivots with measurable impact
    /// </summary>
    public class PivotWithMeasurableImpactSpecification : BusinessModelManagementSpecification<StrategicPivot>
    {
        public override Expression<Func<StrategicPivot, bool>> ToExpression()
        {
            return pivot => pivot.ImpactMetrics.Any() && 
                          pivot.ImpactMetrics.Any(im => im.Measurements.Any());
        }
    }
}
