using System.ComponentModel.DataAnnotations;
using NouAssist.Analytics.Constants;
using NouAssist.Analytics.Enums;

namespace NouAssist.Analytics.DTOs.DataSource
{
    /// <summary>
    /// DTO for creating a new data source
    /// </summary>
    public class CreateDataSourceDto
    {
        [Required]
        [StringLength(AnalyticsConsts.DataSource.MaxNameLength)]
        public string Name { get; set; }
        
        [StringLength(AnalyticsConsts.DataSource.MaxDescriptionLength)]
        public string Description { get; set; }
        
        public DataSourceType Type { get; set; }
        
        [Required]
        public DataConnectionStringDto ConnectionString { get; set; }
        
        public DataSourceCredentialsDto Credentials { get; set; }
    }
}
