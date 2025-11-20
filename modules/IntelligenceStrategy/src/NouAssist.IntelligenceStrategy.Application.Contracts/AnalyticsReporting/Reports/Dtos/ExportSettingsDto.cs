using NouAssist.IntelligenceStrategy.AnalyticsReporting.Enums;

namespace NouAssist.IntelligenceStrategy.AnalyticsReporting.Reports.Dtos
{
    /// <summary>
    /// DTO for export settings
    /// </summary>
    public class ExportSettingsDto
    {
        /// <summary>
        /// Format of the export
        /// </summary>
        public ReportFormat Format { get; set; }
        
        /// <summary>
        /// Whether to include charts in the export
        /// </summary>
        public bool IncludeCharts { get; set; }
        
        /// <summary>
        /// Whether to include raw data in the export
        /// </summary>
        public bool IncludeRawData { get; set; }
        
        /// <summary>
        /// Whether to include timestamps in the filename
        /// </summary>
        public bool IncludeTimestampInFilename { get; set; }
        
        /// <summary>
        /// Custom filename template
        /// </summary>
        public string FilenameTemplate { get; set; }
    }
    
    /// <summary>
    /// DTO for creating export settings
    /// </summary>
    public class CreateExportSettingsDto
    {
        /// <summary>
        /// Format of the export
        /// </summary>
        public ReportFormat Format { get; set; } = ReportFormat.PDF;
        
        /// <summary>
        /// Whether to include charts in the export
        /// </summary>
        public bool IncludeCharts { get; set; } = true;
        
        /// <summary>
        /// Whether to include raw data in the export
        /// </summary>
        public bool IncludeRawData { get; set; } = true;
        
        /// <summary>
        /// Whether to include timestamps in the filename
        /// </summary>
        public bool IncludeTimestampInFilename { get; set; } = true;
        
        /// <summary>
        /// Custom filename template
        /// </summary>
        public string FilenameTemplate { get; set; }
    }
    
    /// <summary>
    /// DTO for updating export settings
    /// </summary>
    public class UpdateExportSettingsDto
    {
        /// <summary>
        /// Format of the export
        /// </summary>
        public ReportFormat Format { get; set; }
        
        /// <summary>
        /// Whether to include charts in the export
        /// </summary>
        public bool IncludeCharts { get; set; }
        
        /// <summary>
        /// Whether to include raw data in the export
        /// </summary>
        public bool IncludeRawData { get; set; }
        
        /// <summary>
        /// Whether to include timestamps in the filename
        /// </summary>
        public bool IncludeTimestampInFilename { get; set; }
        
        /// <summary>
        /// Custom filename template
        /// </summary>
        public string FilenameTemplate { get; set; }
    }
}
