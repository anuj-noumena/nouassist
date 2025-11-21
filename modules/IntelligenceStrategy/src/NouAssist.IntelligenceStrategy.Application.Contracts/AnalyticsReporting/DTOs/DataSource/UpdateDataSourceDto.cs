using System.ComponentModel.DataAnnotations;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Constants;

namespace NouAssist.Analytics.DTOs.DataSource
{
    /// <summary>
    /// DTO for updating an existing data source
    /// </summary>
    public class UpdateDataSourceDto
    {
        [Required]
        [StringLength(AnalyticsConsts.DataSource.MaxNameLength)]
        public string Name { get; set; }
        
        [StringLength(AnalyticsConsts.DataSource.MaxDescriptionLength)]
        public string Description { get; set; }
        
        [Required]
        public DataConnectionStringDto ConnectionString { get; set; }
        
        public DataSourceCredentialsDto Credentials { get; set; }
    }
}
