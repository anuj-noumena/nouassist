using System;
using NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.Enums;

namespace NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.DueDiligenceDataRooms.Dtos
{
    /// <summary>
    /// DTO representing a secure share link
    /// </summary>
    public class SecureShareLinkDto
    {
        public string Token { get; set; }
        public string Url { get; set; }
        public DateTime ExpiryDate { get; set; }
        public AccessLevel AccessLevel { get; set; }
        public bool RequiresPassword { get; set; }
    }
}
