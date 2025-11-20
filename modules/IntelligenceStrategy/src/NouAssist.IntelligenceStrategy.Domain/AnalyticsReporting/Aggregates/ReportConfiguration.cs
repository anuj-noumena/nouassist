using System;
using System.Collections.Generic;
using System.Linq;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Entities;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Enums;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Events;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Exceptions;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.ValueObjects;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace NouAssist.IntelligenceStrategy.AnalyticsReporting.Aggregates
{
    /// <summary>
    /// Configuration for a report including parameters, data sources, and schedules
    /// </summary>
    public class ReportConfiguration : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        /// <summary>
        /// Optional tenant ID for multi-tenancy
        /// </summary>
        public Guid? TenantId { get; protected set; }
        
        /// <summary>
        /// Name of the report
        /// </summary>
        public string Name { get; private set; }
        
        /// <summary>
        /// Description of the report
        /// </summary>
        public string Description { get; private set; }
        
        /// <summary>
        /// Category of the report
        /// </summary>
        public string Category { get; private set; }
        
        /// <summary>
        /// Data sources used by this report
        /// </summary>
        public Guid[] DataSourceIds { get; private set; }
        
        /// <summary>
        /// SQL or query definition for the report
        /// </summary>
        public string QueryDefinition { get; private set; }
        
        /// <summary>
        /// Report format defaults
        /// </summary>
        public ReportFormat DefaultFormat { get; private set; }
        
        /// <summary>
        /// Export settings for the report
        /// </summary>
        public ExportSettings ExportSettings { get; private set; }
        
        /// <summary>
        /// Report template as JSON or HTML
        /// </summary>
        public string TemplateDefinition { get; private set; }
        
        /// <summary>
        /// Tags for organizing reports
        /// </summary>
        public string[] Tags { get; private set; }
        
        private readonly List<ReportParameter> _parameters;
        
        /// <summary>
        /// Parameters for the report
        /// </summary>
        public IReadOnlyCollection<ReportParameter> Parameters => _parameters.AsReadOnly();
        
        private readonly List<ReportSchedule> _schedules;
        
        /// <summary>
        /// Schedules for the report
        /// </summary>
        public IReadOnlyCollection<ReportSchedule> Schedules => _schedules.AsReadOnly();
        
        protected ReportConfiguration()
        {
            _parameters = new List<ReportParameter>();
            _schedules = new List<ReportSchedule>();
            DataSourceIds = Array.Empty<Guid>();
            Tags = Array.Empty<string>();
        }
        
        /// <summary>
        /// Creates a new report configuration
        /// </summary>
        public static ReportConfiguration Create(
            Guid id,
            string name,
            string description,
            string category,
            Guid[] dataSourceIds,
            string queryDefinition,
            ReportFormat defaultFormat,
            string templateDefinition = null,
            ExportSettings exportSettings = null,
            string[] tags = null,
            Guid? tenantId = null)
        {
            var report = new ReportConfiguration(
                id,
                name,
                description,
                category,
                dataSourceIds,
                queryDefinition,
                defaultFormat,
                templateDefinition,
                exportSettings,
                tags,
                tenantId
            );
            
            report.AddDistributedEvent(
                new ReportDefinedEto
                {
                    Id = report.Id,
                    Name = report.Name,
                    Description = report.Description,
                    CreatorId = report.CreatorId.GetValueOrDefault(),
                    IsScheduled = false,
                    TenantId = report.TenantId
                }
            );
            
            return report;
        }
        
        private ReportConfiguration(
            Guid id,
            string name,
            string description,
            string category,
            Guid[] dataSourceIds,
            string queryDefinition,
            ReportFormat defaultFormat,
            string templateDefinition = null,
            ExportSettings exportSettings = null,
            string[] tags = null,
            Guid? tenantId = null)
            : base(id)
        {
            SetName(name);
            SetDescription(description);
            Category = category;
            DataSourceIds = dataSourceIds ?? Array.Empty<Guid>();
            QueryDefinition = queryDefinition;
            DefaultFormat = defaultFormat;
            TemplateDefinition = templateDefinition;
            ExportSettings = exportSettings;
            Tags = tags ?? Array.Empty<string>();
            TenantId = tenantId;
            _parameters = new List<ReportParameter>();
            _schedules = new List<ReportSchedule>();
        }
        
        /// <summary>
        /// Sets the report name
        /// </summary>
        public void SetName(string name)
        {
            Name = Check.NotNullOrWhiteSpace(name, nameof(name), AnalyticsReportingConsts.MaxNameLength);
        }
        
        /// <summary>
        /// Sets the report description
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
        /// Sets the report category
        /// </summary>
        public void SetCategory(string category)
        {
            Category = category;
        }
        
        /// <summary>
        /// Updates the data sources for the report
        /// </summary>
        public void UpdateDataSources(Guid[] dataSourceIds)
        {
            DataSourceIds = dataSourceIds ?? Array.Empty<Guid>();
        }
        
        /// <summary>
        /// Updates the query definition for the report
        /// </summary>
        public void UpdateQueryDefinition(string queryDefinition)
        {
            QueryDefinition = Check.NotNullOrWhiteSpace(queryDefinition, nameof(queryDefinition));
        }
        
        /// <summary>
        /// Updates the template definition for the report
        /// </summary>
        public void UpdateTemplateDefinition(string templateDefinition)
        {
            TemplateDefinition = templateDefinition;
        }
        
        /// <summary>
        /// Sets the default format for the report
        /// </summary>
        public void SetDefaultFormat(ReportFormat defaultFormat)
        {
            DefaultFormat = defaultFormat;
        }
        
        /// <summary>
        /// Updates the export settings for the report
        /// </summary>
        public void UpdateExportSettings(ExportSettings exportSettings)
        {
            ExportSettings = exportSettings;
        }
        
        /// <summary>
        /// Sets tags for the report
        /// </summary>
        public void SetTags(string[] tags)
        {
            Tags = tags ?? Array.Empty<string>();
        }
        
        /// <summary>
        /// Adds a parameter to the report
        /// </summary>
        public ReportParameter AddParameter(
            string name,
            string dataType,
            string defaultValue = null,
            bool isRequired = false,
            string displayName = null,
            string description = null,
            int order = 0,
            string validationRules = null)
        {
            if (_parameters.Any(p => p.Name == name))
            {
                throw new DuplicateNameException("Parameter", name);
            }
            
            var parameter = new ReportParameter(
                Id,
                name,
                dataType,
                defaultValue,
                isRequired,
                displayName,
                description,
                order,
                validationRules
            );
            
            _parameters.Add(parameter);
            return parameter;
        }
        
        /// <summary>
        /// Updates a parameter in the report
        /// </summary>
        public void UpdateParameter(
            string name,
            string defaultValue = null,
            bool? isRequired = null,
            string displayName = null,
            string description = null,
            int? order = null,
            string validationRules = null)
        {
            var parameter = GetParameter(name);
            
            if (defaultValue != null)
            {
                parameter.SetDefaultValue(defaultValue);
            }
            
            if (isRequired.HasValue)
            {
                parameter.SetRequired(isRequired.Value);
            }
            
            if (displayName != null)
            {
                parameter.SetDisplayName(displayName);
            }
            
            if (description != null)
            {
                parameter.SetDescription(description);
            }
            
            if (order.HasValue)
            {
                parameter.SetOrder(order.Value);
            }
            
            if (validationRules != null)
            {
                parameter.SetValidationRules(validationRules);
            }
        }
        
        /// <summary>
        /// Removes a parameter from the report
        /// </summary>
        public void RemoveParameter(string name)
        {
            var parameter = GetParameter(name);
            _parameters.Remove(parameter);
        }
        
        /// <summary>
        /// Gets a parameter by name
        /// </summary>
        private ReportParameter GetParameter(string name)
        {
            var parameter = _parameters.FirstOrDefault(p => p.Name == name);
            if (parameter == null)
            {
                throw new EntityNotFoundException("Parameter", name);
            }
            return parameter;
        }
        
        /// <summary>
        /// Adds a schedule to the report
        /// </summary>
        public ReportSchedule AddSchedule(
            Guid scheduleId,
            ScheduleTiming timing,
            ReportFormat format,
            string[] recipients = null,
            bool attachToEmail = true,
            string emailSubject = null,
            string emailBody = null)
        {
            var schedule = new ReportSchedule(
                scheduleId,
                Id,
                timing,
                format,
                recipients,
                attachToEmail,
                emailSubject,
                emailBody
            );
            
            _schedules.Add(schedule);
            
            AddDistributedEvent(
                new ReportScheduleCreatedEto
                {
                    Id = schedule.Id,
                    ReportId = Id,
                    TenantId = TenantId
                }
            );
            
            return schedule;
        }
        
        /// <summary>
        /// Updates a schedule for the report
        /// </summary>
        public void UpdateSchedule(
            Guid scheduleId,
            ScheduleTiming timing = null,
            ReportFormat? format = null,
            string[] recipients = null,
            bool? attachToEmail = null,
            string emailSubject = null,
            string emailBody = null)
        {
            var schedule = GetSchedule(scheduleId);
            
            if (timing != null)
            {
                schedule.UpdateSchedule(timing);
            }
            
            if (format.HasValue)
            {
                schedule.SetFormat(format.Value);
            }
            
            if (recipients != null)
            {
                schedule.SetRecipients(recipients);
            }
            
            if (attachToEmail.HasValue || emailSubject != null || emailBody != null)
            {
                schedule.SetEmailSettings(
                    attachToEmail ?? schedule.AttachToEmail,
                    emailSubject ?? schedule.EmailSubject,
                    emailBody ?? schedule.EmailBody
                );
            }
            
            AddDistributedEvent(
                new ReportScheduleUpdatedEto
                {
                    Id = schedule.Id,
                    ReportId = Id,
                    TenantId = TenantId
                }
            );
        }
        
        /// <summary>
        /// Records the execution of a schedule
        /// </summary>
        public void RecordScheduleExecution(Guid scheduleId, DateTime executionTime, DateTime? nextScheduled = null)
        {
            var schedule = GetSchedule(scheduleId);
            schedule.RecordExecution(executionTime, nextScheduled);
        }
        
        /// <summary>
        /// Removes a schedule from the report
        /// </summary>
        public void RemoveSchedule(Guid scheduleId)
        {
            var schedule = GetSchedule(scheduleId);
            _schedules.Remove(schedule);
            
            AddDistributedEvent(
                new ReportScheduleDeletedEto
                {
                    Id = scheduleId,
                    ReportId = Id,
                    TenantId = TenantId
                }
            );
        }
        
        /// <summary>
        /// Gets a schedule by ID
        /// </summary>
        private ReportSchedule GetSchedule(Guid scheduleId)
        {
            var schedule = _schedules.FirstOrDefault(s => s.Id == scheduleId);
            if (schedule == null)
            {
                throw new EntityNotFoundException("Schedule", scheduleId);
            }
            return schedule;
        }
        
        /// <summary>
        /// Records the generation of a report
        /// </summary>
        public ReportGenerationLog CreateGenerationLog(
            Guid logId,
            DateTime startTime,
            ReportFormat format,
            Guid? scheduleId = null,
            Guid? userId = null,
            string parametersJson = null)
        {
            return new ReportGenerationLog(
                logId,
                Id,
                startTime,
                format,
                scheduleId,
                userId,
                parametersJson
            );
        }
    }
}
