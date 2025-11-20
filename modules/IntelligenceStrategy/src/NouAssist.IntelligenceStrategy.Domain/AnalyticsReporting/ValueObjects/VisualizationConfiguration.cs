using System.Collections.Generic;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Enums;
using Volo.Abp.Domain.Values;

namespace NouAssist.IntelligenceStrategy.AnalyticsReporting.ValueObjects
{
    /// <summary>
    /// Configuration for data visualization
    /// </summary>
    public class VisualizationConfiguration : ValueObject
    {
        /// <summary>
        /// Type of chart for visualization
        /// </summary>
        public ChartType ChartType { get; private set; }
        
        /// <summary>
        /// JSON configuration for the chart
        /// </summary>
        public string ChartConfig { get; private set; }
        
        /// <summary>
        /// The field name to use for X-axis
        /// </summary>
        public string XAxisField { get; private set; }
        
        /// <summary>
        /// The field name(s) to use for Y-axis
        /// </summary>
        public List<string> YAxisFields { get; private set; }
        
        /// <summary>
        /// Whether to show legends
        /// </summary>
        public bool ShowLegend { get; private set; }
        
        /// <summary>
        /// Custom colors to use for the visualization
        /// </summary>
        public List<string> CustomColors { get; private set; }
        
        private VisualizationConfiguration()
        {
            YAxisFields = new List<string>();
            CustomColors = new List<string>();
        }
        
        public VisualizationConfiguration(
            ChartType chartType,
            string chartConfig,
            string xAxisField,
            List<string> yAxisFields,
            bool showLegend = true,
            List<string> customColors = null)
        {
            ChartType = chartType;
            ChartConfig = chartConfig;
            XAxisField = xAxisField;
            YAxisFields = yAxisFields ?? new List<string>();
            ShowLegend = showLegend;
            CustomColors = customColors ?? new List<string>();
        }
        
        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return ChartType;
            yield return ChartConfig ?? string.Empty;
            yield return XAxisField ?? string.Empty;
            
            foreach (var field in YAxisFields)
            {
                yield return field;
            }
            
            yield return ShowLegend;
            
            foreach (var color in CustomColors)
            {
                yield return color;
            }
        }
    }
}
