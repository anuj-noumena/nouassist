using System;
using System.Linq.Expressions;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Aggregates;
using Volo.Abp.Specifications;

namespace NouAssist.IntelligenceStrategy.AnalyticsReporting.Specifications
{
    /// <summary>
    /// Base specification for Dashboard aggregate
    /// </summary>
    public abstract class DashboardSpecification : Specification<Dashboard>
    {
    }
    
    /// <summary>
    /// Specification for dashboards that can be shared
    /// </summary>
    public class ShareableDashboardSpecification : DashboardSpecification
    {
        private readonly Guid _userId;
        
        public ShareableDashboardSpecification(Guid userId)
        {
            _userId = userId;
        }
        
        public override Expression<Func<Dashboard, bool>> ToExpression()
        {
            return dashboard => 
                !dashboard.IsPrivate || 
                dashboard.CreatorId == _userId;
        }
    }
    
    /// <summary>
    /// Specification for dashboards with specific widget types
    /// </summary>
    public class DashboardWithWidgetTypeSpecification : DashboardSpecification
    {
        private readonly WidgetType _widgetType;
        
        public DashboardWithWidgetTypeSpecification(WidgetType widgetType)
        {
            _widgetType = widgetType;
        }
        
        public override Expression<Func<Dashboard, bool>> ToExpression()
        {
            return dashboard => 
                dashboard.Widgets.Any(w => w.Type == _widgetType);
        }
    }
    
    /// <summary>
    /// Specification for dashboards using a specific data source
    /// </summary>
    public class DashboardUsingDataSourceSpecification : DashboardSpecification
    {
        private readonly Guid _dataSourceId;
        
        public DashboardUsingDataSourceSpecification(Guid dataSourceId)
        {
            _dataSourceId = dataSourceId;
        }
        
        public override Expression<Func<Dashboard, bool>> ToExpression()
        {
            return dashboard => 
                dashboard.Widgets.Any(w => w.DataSourceId == _dataSourceId);
        }
    }
}
