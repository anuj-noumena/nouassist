namespace NouAssist.Analytics.DTOs.Report
{
    /// <summary>
    /// DTO for report parameter information
    /// </summary>
    public class ReportParameterDto
    {
        public string Name { get; set; }
        
        public string DisplayName { get; set; }
        
        public string DataType { get; set; }
        
        public bool IsRequired { get; set; }
        
        public string DefaultValue { get; set; }
        
        public int Order { get; set; }
        
        public string ValidationRegex { get; set; }
    }
}
