using System;
using System.ComponentModel.DataAnnotations;
using NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.Enums;

namespace NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.DueDiligenceDataRooms.Dtos
{
    /// <summary>
    /// DTO for generating a secure share link
    /// </summary>
    public class GenerateShareLinkDto
    {
        [Required]
        public Guid DocumentId { get; set; }

        [Required]
        public Guid InvestorId { get; set; }

        [Required]
        public DateTime ExpiryDate { get; set; }

        public AccessLevel AccessLevel { get; set; } = AccessLevel.Read;
    }
}
