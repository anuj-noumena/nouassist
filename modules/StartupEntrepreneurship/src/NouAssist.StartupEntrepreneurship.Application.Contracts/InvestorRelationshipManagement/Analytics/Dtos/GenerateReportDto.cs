using System;
using System.ComponentModel.DataAnnotations;

namespace NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.Analytics.Dtos
{
    /// <summary>
    /// DTO for generating analytics reports
    /// </summary>
    public class GenerateReportDto
    {
        [Required]
        public DateTime FromDate { get; set; }

        [Required]
        public DateTime ToDate { get; set; }

        public List<Guid> InvestorIds { get; set; } = new();
    }
}
