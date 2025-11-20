using System;
using System.Linq;
using System.Linq.Expressions;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Aggregates;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Enums;
using Volo.Abp.Specifications;

namespace NouAssist.IntelligenceStrategy.AnalyticsReporting.Specifications
{
    /// <summary>
    /// Base specification for ReportConfiguration aggregate
    /// </summary>
    public abstract class ReportSpecification : Specification<ReportConfiguration>
    {
    }
    
    /// <summary>
    /// Specification for reports that are scheduled
    /// </summary>
    public class ScheduledReportSpecification : ReportSpecification
    {
        public override Expression<Func<ReportConfiguration, bool>> ToExpression()
        {
            return report => report.Schedules.Any();
        }
    }
    
    /// <summary>
    /// Specification for reports using a specific data source
    /// </summary>
    public class ReportUsingDataSourceSpecification : ReportSpecification
    {
        private readonly Guid _dataSourceId;
        
        public ReportUsingDataSourceSpecification(Guid dataSourceId)
        {
            _dataSourceId = dataSourceId;
        }
        
        public override Expression<Func<ReportConfiguration, bool>> ToExpression()
        {
            return report => report.DataSourceIds.Contains(_dataSourceId);
        }
    }
    
    /// <summary>
    /// Specification for reports with a specific default format
    /// </summary>
    public class ReportWithFormatSpecification : ReportSpecification
    {
        private readonly ReportFormat _format;
        
        public ReportWithFormatSpecification(ReportFormat format)
        {
            _format = format;
        }
        
        public override Expression<Func<ReportConfiguration, bool>> ToExpression()
        {
            return report => report.DefaultFormat == _format;
        }
    }
}
