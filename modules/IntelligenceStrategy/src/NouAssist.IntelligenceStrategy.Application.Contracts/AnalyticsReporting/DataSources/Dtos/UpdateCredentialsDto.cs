using System;
using System.ComponentModel.DataAnnotations;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Constants;

namespace NouAssist.IntelligenceStrategy.AnalyticsReporting.DataSources.Dtos
{
    /// <summary>
    /// DTO for updating credentials
    /// </summary>
    public class UpdateCredentialsDto
    {
        /// <summary>
        /// The credentials to be encrypted
        /// </summary>
        [Required]
        public string Credentials { get; set; }
    }
}
