using System;
using System.Collections.Generic;
using System.Linq;
using NouAssist.Analytics.Constants;
using NouAssist.Analytics.Entities;
using NouAssist.Analytics.Enums;
using NouAssist.Analytics.Exceptions;
using NouAssist.Analytics.ValueObjects;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace NouAssist.Analytics.Aggregates
{
    /// <summary>
    /// Represents a data source configuration for retrieving and transforming data
    /// </summary>
    public class DataSource : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Guid? TenantId { get; protected set; }
        
        public string Name { get; private set; }
        
        public string Description { get; private set; }
        
        public DataSourceType Type { get; private set; }
        
        public bool IsActive { get; private set; }
        
        public DataConnectionString ConnectionString { get; private set; }
        
        public DataSourceCredentials Credentials { get; private set; }
        
        private readonly List<DataTransformationStep> _transformationSteps;
        public IReadOnlyCollection<DataTransformationStep> TransformationSteps => _transformationSteps.AsReadOnly();
        
        private readonly List<AggregatedDataView> _aggregatedDataViews;
        public IReadOnlyCollection<AggregatedDataView> AggregatedDataViews => _aggregatedDataViews.AsReadOnly();
        
        protected DataSource()
        {
            _transformationSteps = new List<DataTransformationStep>();
            _aggregatedDataViews = new List<AggregatedDataView>();
        }
        
        public DataSource(
            Guid id,
            string name,
            string description,
            DataSourceType type,
            DataConnectionString connectionString,
            DataSourceCredentials credentials,
            Guid? tenantId
        ) : base(id)
        {
            TenantId = tenantId;
            SetName(name);
            SetDescription(description);
            Type = type;
            ConnectionString = connectionString;
            Credentials = credentials;
            IsActive = true;
            _transformationSteps = new List<DataTransformationStep>();
            _aggregatedDataViews = new List<AggregatedDataView>();
        }
        
        public void SetName(string name)
        {
            Name = Check.NotNullOrWhiteSpace(
                name,
                nameof(name),
                maxLength: AnalyticsConsts.DataSource.MaxNameLength
            );
        }
        
        public void SetDescription(string description)
        {
            Description = Check.Length(
                description,
                nameof(description),
                maxLength: AnalyticsConsts.DataSource.MaxDescriptionLength
            );
        }
        
        public void SetActive(bool isActive)
        {
            IsActive = isActive;
        }
        
        public void UpdateConnectionString(DataConnectionString connectionString)
        {
            Check.NotNull(connectionString, nameof(connectionString));
            ConnectionString = connectionString;
        }
        
        public void UpdateCredentials(DataSourceCredentials credentials)
        {
            Credentials = credentials;
        }
        
        public void AddTransformationStep(DataTransformationStep step)
        {
            Check.NotNull(step, nameof(step));
            
            if (_transformationSteps.Count >= AnalyticsConsts.DataSource.MaxTransformationStepsPerDataSource)
            {
                throw new AnalyticsDomainException("Analytics:ErrorMessage:MaxTransformationStepsExceeded")
                    .WithData("maxSteps", AnalyticsConsts.DataSource.MaxTransformationStepsPerDataSource);
            }
            
            _transformationSteps.Add(step);
        }
        
        public void RemoveTransformationStep(Guid stepId)
        {
            var step = _transformationSteps.FirstOrDefault(s => s.Id == stepId);
            if (step == null)
            {
                return;
            }
            
            _transformationSteps.Remove(step);
        }
        
        public void ReorderTransformationSteps(Guid stepId, int newOrder)
        {
            var step = _transformationSteps.FirstOrDefault(s => s.Id == stepId);
            if (step == null)
            {
                return;
            }
            
            if (newOrder < 0 || newOrder >= _transformationSteps.Count)
            {
                throw new AnalyticsDomainException("Analytics:ErrorMessage:InvalidValue")
                    .WithData("property", "newOrder");
            }
            
            _transformationSteps.Remove(step);
            _transformationSteps.Insert(newOrder, step);
            
            // Update order values for all steps
            for (int i = 0; i < _transformationSteps.Count; i++)
            {
                _transformationSteps[i].UpdateOrder(i);
            }
        }
        
        public void AddAggregatedDataView(AggregatedDataView view)
        {
            Check.NotNull(view, nameof(view));
            _aggregatedDataViews.Add(view);
        }
        
        public void RemoveAggregatedDataView(Guid viewId)
        {
            var view = _aggregatedDataViews.FirstOrDefault(v => v.Id == viewId);
            if (view == null)
            {
                return;
            }
            
            _aggregatedDataViews.Remove(view);
        }
    }
}
