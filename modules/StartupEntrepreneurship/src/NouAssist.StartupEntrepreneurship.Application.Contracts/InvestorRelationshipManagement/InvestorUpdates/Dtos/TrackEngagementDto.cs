using System;
using System.ComponentModel.DataAnnotations;
using NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.Enums;

namespace NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.InvestorUpdates.Dtos
{
    /// <summary>
    /// DTO for tracking investor engagement
    /// </summary>
    public class TrackEngagementDto
    {
        [Required]
        public Guid InvestorId { get; set; }

        [Required]
        public EngagementType Type { get; set; }

        [StringLength(1000)]
        public string Details { get; set; }

        public string IpAddress { get; set; }

        public string UserAgent { get; set; }
    }
}
