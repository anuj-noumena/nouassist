using System;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Constants;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Enums;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.ValueObjects;
using Volo.Abp;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace NouAssist.IntelligenceStrategy.AnalyticsReporting.Entities
{
    /// <summary>
    /// A pre-aggregated view of data for reporting
    /// </summary>
    public class AggregatedDataView : Entity<Guid>, IMultiTenant
    {
        /// <summary>
        /// Optional tenant ID for multi-tenancy
        /// </summary>
        public Guid? TenantId { get; protected set; }
        
        /// <summary>
        /// Name of the aggregated view
        /// </summary>
        public string Name { get; private set; }
        
        /// <summary>
        /// Description of what this view represents
        /// </summary>
        public string Description { get; private set; }
        
        /// <summary>
        /// Source data sources for this view
        /// </summary>
        public Guid[] SourceDataSourceIds { get; private set; }
        
        /// <summary>
        /// When this view was last refreshed
        /// </summary>
        public DateTime LastRefreshed { get; private set; }
        
        /// <summary>
        /// Refresh interval setting
        /// </summary>
        public RefreshInterval RefreshInterval { get; private set; }
        
        /// <summary>
        /// Query or aggregation definition as JSON
        /// </summary>
        public string AggregationDefinition { get; private set; }
        
        /// <summary>
        /// Status of data validation
        /// </summary>
        public ValidationStatus ValidationStatus { get; private set; }
        
        protected AggregatedDataView()
        {
            SourceDataSourceIds = Array.Empty<Guid>();
        }
        
        public AggregatedDataView(
            Guid id,
            string name,
            string description,
            Guid[] sourceDataSourceIds,
            string aggregationDefinition,
            RefreshInterval refreshInterval,
            DateTime lastRefreshed,
            ValidationStatus validationStatus = ValidationStatus.NotValidated,
            Guid? tenantId = null)
            : base(id)
        {
            SetName(name);
            SetDescription(description);
            SourceDataSourceIds = sourceDataSourceIds ?? Array.Empty<Guid>();
            AggregationDefinition = aggregationDefinition;
            RefreshInterval = refreshInterval;
            LastRefreshed = lastRefreshed;
            ValidationStatus = validationStatus;
            TenantId = tenantId;
        }
        
        public void SetName(string name)
        {
            Name = Check.NotNullOrWhiteSpace(name, nameof(name), AnalyticsReportingConsts.MaxNameLength);
        }
        
        public void SetDescription(string description)
        {
            Description = Check.NotNullOrWhiteSpace(
                description,
                nameof(description),
                AnalyticsReportingConsts.MaxDescriptionLength
            );
        }
        
        public void SetSourceDataSources(Guid[] sourceDataSourceIds)
        {
            SourceDataSourceIds = sourceDataSourceIds ?? Array.Empty<Guid>();
        }
        
        public void UpdateAggregationDefinition(string aggregationDefinition)
        {
            AggregationDefinition = Check.NotNullOrWhiteSpace(aggregationDefinition, nameof(aggregationDefinition));
            ValidationStatus = ValidationStatus.NotValidated;
        }
        
        public void SetRefreshInterval(RefreshInterval refreshInterval)
        {
            RefreshInterval = refreshInterval;
        }
        
        public void MarkAsRefreshed(DateTime refreshTime, ValidationStatus validationStatus)
        {
            LastRefreshed = refreshTime;
            ValidationStatus = validationStatus;
        }
    }
}
