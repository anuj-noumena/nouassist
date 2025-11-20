using System.Collections.Generic;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Constants;
using Volo.Abp;
using Volo.Abp.Domain.Values;

namespace NouAssist.IntelligenceStrategy.AnalyticsReporting.ValueObjects
{
    /// <summary>
    /// Settings for a dashboard widget
    /// </summary>
    public class WidgetSettings : ValueObject
    {
        /// <summary>
        /// Widget title to display
        /// </summary>
        public string Title { get; private set; }
        
        /// <summary>
        /// Widget subtitle to display
        /// </summary>
        public string Subtitle { get; private set; }
        
        /// <summary>
        /// Widget refresh interval in seconds
        /// </summary>
        public int RefreshIntervalSeconds { get; private set; }
        
        /// <summary>
        /// Widget configuration as serialized JSON
        /// </summary>
        public string Configuration { get; private set; }
        
        /// <summary>
        /// Whether to show the widget title
        /// </summary>
        public bool ShowTitle { get; private set; }
        
        /// <summary>
        /// Whether to allow maximizing the widget
        /// </summary>
        public bool AllowMaximize { get; private set; }
        
        private WidgetSettings()
        {
        }
        
        public WidgetSettings(
            string title,
            string subtitle,
            int refreshIntervalSeconds,
            string configuration,
            bool showTitle = true,
            bool allowMaximize = true)
        {
            Title = Check.NotNullOrWhiteSpace(title, nameof(title), AnalyticsReportingConsts.MaxNameLength);
            Subtitle = subtitle;
            
            if (refreshIntervalSeconds < 0)
            {
                throw new BusinessException("AnalyticsReporting:Validation:InvalidValue")
                    .WithData("propertyName", "refreshIntervalSeconds");
            }
            
            RefreshIntervalSeconds = refreshIntervalSeconds;
            Configuration = configuration;
            ShowTitle = showTitle;
            AllowMaximize = allowMaximize;
        }
        
        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Title;
            yield return Subtitle ?? string.Empty;
            yield return RefreshIntervalSeconds;
            yield return Configuration ?? string.Empty;
            yield return ShowTitle;
            yield return AllowMaximize;
        }
    }
}
