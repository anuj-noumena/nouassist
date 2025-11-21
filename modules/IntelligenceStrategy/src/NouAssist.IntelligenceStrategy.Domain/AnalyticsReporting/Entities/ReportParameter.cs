using System;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Constants;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace NouAssist.IntelligenceStrategy.AnalyticsReporting.Entities
{
    /// <summary>
    /// Parameter for a report
    /// </summary>
    public class ReportParameter : Entity
    {
        /// <summary>
        /// Report configuration this parameter belongs to
        /// </summary>
        public Guid ReportConfigurationId { get; private set; }
        
        /// <summary>
        /// Parameter name
        /// </summary>
        public string Name { get; private set; }
        
        /// <summary>
        /// Parameter data type
        /// </summary>
        public string DataType { get; private set; }
        
        /// <summary>
        /// Default value
        /// </summary>
        public string DefaultValue { get; private set; }
        
        /// <summary>
        /// Whether the parameter is required
        /// </summary>
        public bool IsRequired { get; private set; }
        
        /// <summary>
        /// Display name for the parameter
        /// </summary>
        public string DisplayName { get; private set; }
        
        /// <summary>
        /// Description of the parameter
        /// </summary>
        public string Description { get; private set; }
        
        /// <summary>
        /// Order for display
        /// </summary>
        public int Order { get; private set; }
        
        /// <summary>
        /// Validation rules as JSON
        /// </summary>
        public string ValidationRules { get; private set; }
        
        protected ReportParameter()
        {
        }
        
        internal ReportParameter(
            Guid reportConfigurationId,
            string name,
            string dataType,
            string defaultValue = null,
            bool isRequired = false,
            string displayName = null,
            string description = null,
            int order = 0,
            string validationRules = null)
        {
            ReportConfigurationId = reportConfigurationId;
            Name = Check.NotNullOrWhiteSpace(name, nameof(name), AnalyticsReportingConsts.MaxNameLength);
            DataType = Check.NotNullOrWhiteSpace(dataType, nameof(dataType));
            DefaultValue = defaultValue;
            IsRequired = isRequired;
            DisplayName = displayName ?? name;
            Description = description;
            Order = order;
            ValidationRules = validationRules;
        }
        
        internal void SetDefaultValue(string defaultValue)
        {
            DefaultValue = defaultValue;
        }
        
        internal void SetRequired(bool isRequired)
        {
            IsRequired = isRequired;
        }
        
        internal void SetDisplayName(string displayName)
        {
            DisplayName = displayName;
        }
        
        internal void SetDescription(string description)
        {
            Description = description;
        }
        
        internal void SetOrder(int order)
        {
            Order = order;
        }
        
        internal void SetValidationRules(string validationRules)
        {
            ValidationRules = validationRules;
        }
        
        public override object[] GetKeys()
        {
            return new object[] { ReportConfigurationId, Name };
        }
    }
}
