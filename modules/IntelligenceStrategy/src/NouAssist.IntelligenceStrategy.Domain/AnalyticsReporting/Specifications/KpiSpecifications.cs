using System;
using System.Linq;
using System.Linq.Expressions;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Aggregates;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Enums;
using Volo.Abp.Specifications;

namespace NouAssist.IntelligenceStrategy.AnalyticsReporting.Specifications
{
    /// <summary>
    /// Base specification for KpiDefinition aggregate
    /// </summary>
    public abstract class KpiSpecification : Specification<KpiDefinition>
    {
    }
    
    /// <summary>
    /// Specification for KPIs with alerts
    /// </summary>
    public class KpiWithAlertsSpecification : KpiSpecification
    {
        public override Expression<Func<KpiDefinition, bool>> ToExpression()
        {
            return kpi => kpi.Thresholds.Any();
        }
    }
    
    /// <summary>
    /// Specification for KPIs with critical status
    /// </summary>
    public class CriticalKpiSpecification : KpiSpecification
    {
        public override Expression<Func<KpiDefinition, bool>> ToExpression()
        {
            return kpi => 
                kpi.IsActive && 
                kpi.CurrentValue != null && 
                kpi.CurrentValue.Status == KpiStatus.Critical;
        }
    }
    
    /// <summary>
    /// Specification for KPIs using a specific data source
    /// </summary>
    public class KpiUsingDataSourceSpecification : KpiSpecification
    {
        private readonly Guid _dataSourceId;
        
        public KpiUsingDataSourceSpecification(Guid dataSourceId)
        {
            _dataSourceId = dataSourceId;
        }
        
        public override Expression<Func<KpiDefinition, bool>> ToExpression()
        {
            return kpi => kpi.DataSourceIds.Contains(_dataSourceId);
        }
    }
    
    /// <summary>
    /// Specification for KPIs that require refresh
    /// </summary>
    public class KpiNeedsRefreshSpecification : KpiSpecification
    {
        private readonly DateTime _currentTime;
        
        public KpiNeedsRefreshSpecification(DateTime currentTime)
        {
            _currentTime = currentTime;
        }
        
        public override Expression<Func<KpiDefinition, bool>> ToExpression()
        {
            return kpi => 
                kpi.IsActive && 
                (kpi.CurrentValue == null || 
                 _currentTime.Subtract(kpi.CurrentValue.Timestamp).TotalSeconds > kpi.RefreshInterval.ToSeconds());
        }
    }
}
