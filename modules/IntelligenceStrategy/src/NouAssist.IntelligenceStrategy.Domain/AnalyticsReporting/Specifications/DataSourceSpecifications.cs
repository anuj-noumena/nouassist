using System;
using System.Linq;
using System.Linq.Expressions;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Aggregates;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Enums;
using Volo.Abp.Specifications;

namespace NouAssist.IntelligenceStrategy.AnalyticsReporting.Specifications
{
    /// <summary>
    /// Base specification for DataSource aggregate
    /// </summary>
    public abstract class DataSourceSpecification : Specification<DataSource>
    {
    }
    
    /// <summary>
    /// Specification for active data sources
    /// </summary>
    public class ActiveDataSourceSpecification : DataSourceSpecification
    {
        public override Expression<Func<DataSource, bool>> ToExpression()
        {
            return dataSource => dataSource.IsActive;
        }
    }
    
    /// <summary>
    /// Specification for data sources that need refresh
    /// </summary>
    public class DataSourceNeedsRefreshSpecification : DataSourceSpecification
    {
        private readonly DateTime _currentTime;
        
        public DataSourceNeedsRefreshSpecification(DateTime currentTime)
        {
            _currentTime = currentTime;
        }
        
        public override Expression<Func<DataSource, bool>> ToExpression()
        {
            return dataSource => 
                dataSource.IsActive && 
                (dataSource.LastRefreshed == null || 
                 _currentTime.Subtract(dataSource.LastRefreshed.Value).TotalSeconds > dataSource.RefreshInterval.ToSeconds());
        }
    }
    
    /// <summary>
    /// Specification for data sources with transformation steps
    /// </summary>
    public class DataSourceWithTransformationsSpecification : DataSourceSpecification
    {
        public override Expression<Func<DataSource, bool>> ToExpression()
        {
            return dataSource => dataSource.TransformationSteps.Any();
        }
    }
    
    /// <summary>
    /// Specification for data sources with valid credentials
    /// </summary>
    public class DataSourceWithValidCredentialsSpecification : DataSourceSpecification
    {
        public override Expression<Func<DataSource, bool>> ToExpression()
        {
            return dataSource => 
                dataSource.Credentials == null || 
                dataSource.Credentials.IsValid;
        }
    }
}
