using System;
using System.Collections.Generic;
using System.Linq;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Constants;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Entities;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Enums;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Events;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Exceptions;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Helpers;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.ValueObjects;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace NouAssist.IntelligenceStrategy.AnalyticsReporting.Aggregates
{
    /// <summary>
    /// Represents a data source for reports and dashboards
    /// </summary>
    public class DataSource : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        /// <summary>
        /// Optional tenant ID for multi-tenancy
        /// </summary>
        public Guid? TenantId { get; protected set; }
        
        /// <summary>
        /// Name of the data source
        /// </summary>
        public string Name { get; private set; }
        
        /// <summary>
        /// Description of the data source
        /// </summary>
        public string Description { get; private set; }
        
        /// <summary>
        /// Type of data source
        /// </summary>
        public DataSourceType Type { get; private set; }
        
        /// <summary>
        /// Connection string if applicable
        /// </summary>
        public DataConnectionString ConnectionString { get; private set; }
        
        /// <summary>
        /// Credentials for data source access
        /// </summary>
        public DataSourceCredentials Credentials { get; private set; }
        
        /// <summary>
        /// Configuration settings as JSON
        /// </summary>
        public string Configuration { get; private set; }
        
        /// <summary>
        /// When this data source was last refreshed
        /// </summary>
        public DateTime? LastRefreshed { get; private set; }
        
        /// <summary>
        /// Refresh interval setting
        /// </summary>
        public RefreshInterval RefreshInterval { get; private set; }
        
        /// <summary>
        /// Status of data validation
        /// </summary>
        public ValidationStatus ValidationStatus { get; private set; }
        
        /// <summary>
        /// Schema definition as JSON
        /// </summary>
        public string SchemaDefinition { get; private set; }
        
        /// <summary>
        /// Whether this data source is active
        /// </summary>
        public bool IsActive { get; private set; }
        
        /// <summary>
        /// Tags for organizing data sources
        /// </summary>
        public string[] Tags { get; private set; }
        
        private readonly List<DataTransformationStep> _transformationSteps;
        
        /// <summary>
        /// Transformation steps for this data source
        /// </summary>
        public IReadOnlyCollection<DataTransformationStep> TransformationSteps => _transformationSteps.AsReadOnly();
        
        protected DataSource()
        {
            _transformationSteps = new List<DataTransformationStep>();
            Tags = Array.Empty<string>();
        }
        
        /// <summary>
        /// Creates a new data source
        /// </summary>
        public static DataSource Create(
            Guid id,
            string name,
            string description,
            DataSourceType type,
            RefreshInterval refreshInterval,
            string configuration = null,
            DataConnectionString connectionString = null,
            DataSourceCredentials credentials = null,
            string schemaDefinition = null,
            bool isActive = true,
            string[] tags = null,
            Guid? tenantId = null)
        {
            var dataSource = new DataSource(
                id,
                name,
                description,
                type,
                refreshInterval,
                configuration,
                connectionString,
                credentials,
                schemaDefinition,
                isActive,
                tags,
                tenantId
            );
            
            dataSource.AddDistributedEvent(
                new DataSourceConnectedEto
                {
                    Id = dataSource.Id,
                    Name = dataSource.Name,
                    Type = (int)dataSource.Type,
                    CreatorId = dataSource.CreatorId.GetValueOrDefault(),
                    TenantId = dataSource.TenantId
                }
            );
            
            return dataSource;
        }
        
        private DataSource(
            Guid id,
            string name,
            string description,
            DataSourceType type,
            RefreshInterval refreshInterval,
            string configuration = null,
            DataConnectionString connectionString = null,
            DataSourceCredentials credentials = null,
            string schemaDefinition = null,
            bool isActive = true,
            string[] tags = null,
            Guid? tenantId = null)
            : base(id)
        {
            SetName(name);
            SetDescription(description);
            Type = type;
            RefreshInterval = refreshInterval;
            Configuration = configuration;
            ConnectionString = connectionString;
            Credentials = credentials;
            SchemaDefinition = schemaDefinition;
            ValidationStatus = ValidationStatus.NotValidated;
            IsActive = isActive;
            Tags = tags ?? Array.Empty<string>();
            TenantId = tenantId;
            _transformationSteps = new List<DataTransformationStep>();
        }
        
        /// <summary>
        /// Sets the data source name
        /// </summary>
        public void SetName(string name)
        {
            Name = Check.NotNullOrWhiteSpace(name, nameof(name), AnalyticsReportingConsts.MaxNameLength);
        }
        
        /// <summary>
        /// Sets the data source description
        /// </summary>
        public void SetDescription(string description)
        {
            Description = Check.NotNullOrWhiteSpace(
                description,
                nameof(description),
                AnalyticsReportingConsts.MaxDescriptionLength
            );
        }
        
        /// <summary>
        /// Updates the configuration for the data source
        /// </summary>
        public void UpdateConfiguration(string configuration)
        {
            Configuration = configuration;
            ValidationStatus = ValidationStatus.NotValidated;
        }
        
        /// <summary>
        /// Updates the connection string for the data source
        /// </summary>
        public void UpdateConnectionString(DataConnectionString connectionString)
        {
            ConnectionString = connectionString;
            ValidationStatus = ValidationStatus.NotValidated;
        }
        
        /// <summary>
        /// Updates the credentials for the data source
        /// </summary>
        public void UpdateCredentials(DataSourceCredentials credentials)
        {
            Credentials = credentials;
            ValidationStatus = ValidationStatus.NotValidated;
        }
        
        /// <summary>
        /// Updates the schema definition for the data source
        /// </summary>
        public void UpdateSchemaDefinition(string schemaDefinition)
        {
            SchemaDefinition = schemaDefinition;
        }
        
        /// <summary>
        /// Updates the refresh interval for the data source
        /// </summary>
        public void UpdateRefreshInterval(RefreshInterval refreshInterval)
        {
            RefreshInterval = refreshInterval;
        }
        
        /// <summary>
        /// Activates or deactivates the data source
        /// </summary>
        public void SetActive(bool isActive)
        {
            IsActive = isActive;
        }
        
        /// <summary>
        /// Sets tags for the data source
        /// </summary>
        public void SetTags(string[] tags)
        {
            Tags = tags ?? Array.Empty<string>();
        }
        
        /// <summary>
        /// Marks the data source as refreshed
        /// </summary>
        public void MarkAsRefreshed(DateTime refreshTime, ValidationStatus validationStatus)
        {
            LastRefreshed = refreshTime;
            ValidationStatus = validationStatus;
            
            AddDistributedEvent(
                new DataSourceRefreshedEto
                {
                    Id = Id,
                    RefreshTime = refreshTime,
                    ValidationStatus = (int)validationStatus,
                    TenantId = TenantId
                }
            );
        }
        
        /// <summary>
        /// Adds a transformation step to the data source
        /// </summary>
        public DataTransformationStep AddTransformationStep(
            Guid stepId,
            TransformationRule rule,
            int executionOrder,
            bool isEnabled = true)
        {
            var step = new DataTransformationStep(
                stepId,
                Id,
                rule,
                executionOrder,
                isEnabled
            );
            
            _transformationSteps.Add(step);
            
            AddDistributedEvent(
                new DataTransformationStepAddedEto
                {
                    Id = step.Id,
                    DataSourceId = Id,
                    TenantId = TenantId
                }
            );
            
            return step;
        }
        
        /// <summary>
        /// Updates a transformation step
        /// </summary>
        public void UpdateTransformationStep(
            Guid stepId,
            TransformationRule rule = null,
            int? executionOrder = null,
            bool? isEnabled = null)
        {
            var step = GetTransformationStep(stepId);
            
            if (rule != null)
            {
                step.UpdateRule(rule);
            }
            
            if (executionOrder.HasValue)
            {
                step.SetExecutionOrder(executionOrder.Value);
            }
            
            if (isEnabled.HasValue)
            {
                step.SetEnabled(isEnabled.Value);
            }
            
            AddDistributedEvent(
                new DataTransformationStepUpdatedEto
                {
                    Id = step.Id,
                    DataSourceId = Id,
                    TenantId = TenantId
                }
            );
        }
        
        /// <summary>
        /// Removes a transformation step
        /// </summary>
        public void RemoveTransformationStep(Guid stepId)
        {
            var step = GetTransformationStep(stepId);
            _transformationSteps.Remove(step);
            
            AddDistributedEvent(
                new DataTransformationStepRemovedEto
                {
                    Id = stepId,
                    DataSourceId = Id,
                    TenantId = TenantId
                }
            );
        }
        
        /// <summary>
        /// Gets a transformation step by ID
        /// </summary>
        private DataTransformationStep GetTransformationStep(Guid stepId)
        {
            var step = _transformationSteps.FirstOrDefault(s => s.Id == stepId);
            if (step == null)
            {
                throw new EntityNotFoundException("TransformationStep", stepId);
            }
            return step;
        }
        
        /// <summary>
        /// Validates the data source configuration
        /// </summary>
        public ValidationResult ValidateConfiguration()
        {
            var result = new ValidationResult();
            
            if (Type == DataSourceType.DatabaseView && ConnectionString == null)
            {
                result.AddError("Connection string is required for database views");
            }
            
            if (Type == DataSourceType.API && Credentials == null)
            {
                result.AddError("Credentials are required for API data sources");
            }
            
            return result;
        }
    }
}
