using System.Collections.Generic;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Enums;
using Volo.Abp.Domain.Values;

namespace NouAssist.IntelligenceStrategy.AnalyticsReporting.ValueObjects
{
    /// <summary>
    /// Settings for exporting reports
    /// </summary>
    public class ExportSettings : ValueObject
    {
        /// <summary>
        /// Format of the export
        /// </summary>
        public ReportFormat Format { get; private set; }
        
        /// <summary>
        /// Whether to include charts in the export
        /// </summary>
        public bool IncludeCharts { get; private set; }
        
        /// <summary>
        /// Whether to include raw data in the export
        /// </summary>
        public bool IncludeRawData { get; private set; }
        
        /// <summary>
        /// Whether to include timestamps in the filename
        /// </summary>
        public bool IncludeTimestampInFilename { get; private set; }
        
        /// <summary>
        /// Custom filename template
        /// </summary>
        public string FilenameTemplate { get; private set; }
        
        private ExportSettings()
        {
        }
        
        public ExportSettings(
            ReportFormat format,
            bool includeCharts = true,
            bool includeRawData = true,
            bool includeTimestampInFilename = true,
            string filenameTemplate = null)
        {
            Format = format;
            IncludeCharts = includeCharts;
            IncludeRawData = includeRawData;
            IncludeTimestampInFilename = includeTimestampInFilename;
            FilenameTemplate = filenameTemplate;
        }
        
        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Format;
            yield return IncludeCharts;
            yield return IncludeRawData;
            yield return IncludeTimestampInFilename;
            yield return FilenameTemplate ?? string.Empty;
        }
    }
}
