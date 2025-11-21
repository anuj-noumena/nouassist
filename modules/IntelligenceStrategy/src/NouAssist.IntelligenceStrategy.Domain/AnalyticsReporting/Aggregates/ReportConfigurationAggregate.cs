using System;
using System.Collections.Generic;
using System.Linq;
using NouAssist.Analytics.Constants;
using NouAssist.Analytics.Entities;
using NouAssist.Analytics.Enums;
using NouAssist.Analytics.Exceptions;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Entities;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Enums;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace NouAssist.Analytics.Aggregates
{
    /// <summary>
    /// Represents a report configuration with parameters, data sources, and scheduling info
    /// </summary>
    public class ReportConfiguration : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Guid? TenantId { get; protected set; }
        
        public string Name { get; private set; }
        
        public string Description { get; private set; }
        
        public ReportFormat DefaultFormat { get; private set; }
        
        private readonly List<ReportParameter> _parameters;
        public IReadOnlyCollection<ReportParameter> Parameters => _parameters.AsReadOnly();
        
        private readonly List<Guid> _dataSources;
        public IReadOnlyCollection<Guid> DataSources => _dataSources.AsReadOnly();
        
        private ReportSchedule _schedule;
        public ReportSchedule Schedule => _schedule;
        
        private readonly List<ReportGenerationLog> _generationLogs;
        public IReadOnlyCollection<ReportGenerationLog> GenerationLogs => _generationLogs.AsReadOnly();
        
        protected ReportConfiguration()
        {
            _parameters = new List<ReportParameter>();
            _dataSources = new List<Guid>();
            _generationLogs = new List<ReportGenerationLog>();
        }
        
        public ReportConfiguration(
            Guid id,
            string name,
            string description,
            ReportFormat defaultFormat,
            ReportSchedule schedule,
            Guid? tenantId
        ) : base(id)
        {
            TenantId = tenantId;
            SetName(name);
            SetDescription(description);
            DefaultFormat = defaultFormat;
            _schedule = schedule;
            _parameters = new List<ReportParameter>();
            _dataSources = new List<Guid>();
            _generationLogs = new List<ReportGenerationLog>();
        }
        
        public void SetName(string name)
        {
            Name = Check.NotNullOrWhiteSpace(
                name,
                nameof(name),
                maxLength: AnalyticsConsts.Report.MaxNameLength
            );
        }
        
        public void SetDescription(string description)
        {
            Description = Check.Length(
                description,
                nameof(description),
                maxLength: AnalyticsConsts.Report.MaxDescriptionLength
            );
        }
        
        public void SetDefaultFormat(ReportFormat format)
        {
            DefaultFormat = format;
        }
        
        public void UpdateSchedule(ReportSchedule schedule)
        {
            Check.NotNull(schedule, nameof(schedule));
            _schedule = schedule;
        }
        
        public void AddParameter(ReportParameter parameter)
        {
            Check.NotNull(parameter, nameof(parameter));
            
            if (_parameters.Count >= AnalyticsConsts.Report.MaxParametersPerReport)
            {
                throw new InvalidReportParameterException(
                    Id, 
                    parameter.Name, 
                    $"Maximum allowed parameters ({AnalyticsConsts.Report.MaxParametersPerReport}) exceeded"
                );
            }
            
            if (_parameters.Any(p => p.Name == parameter.Name))
            {
                throw new InvalidReportParameterException(
                    Id,
                    parameter.Name,
                    "Parameter with the same name already exists"
                );
            }
            
            _parameters.Add(parameter);
        }
        
        public void RemoveParameter(string parameterName)
        {
            var parameter = _parameters.FirstOrDefault(p => p.Name == parameterName);
            if (parameter == null)
            {
                return;
            }
            
            _parameters.Remove(parameter);
        }
        
        public void AddDataSource(Guid dataSourceId)
        {
            if (!_dataSources.Contains(dataSourceId))
            {
                _dataSources.Add(dataSourceId);
            }
        }
        
        public void RemoveDataSource(Guid dataSourceId)
        {
            _dataSources.Remove(dataSourceId);
        }
        
        public void AddGenerationLog(ReportGenerationLog generationLog)
        {
            Check.NotNull(generationLog, nameof(generationLog));
            _generationLogs.Add(generationLog);
            
            // Keep only the last 10 logs
            while (_generationLogs.Count > 10)
            {
                _generationLogs.RemoveAt(0);
            }
        }
    }
}
