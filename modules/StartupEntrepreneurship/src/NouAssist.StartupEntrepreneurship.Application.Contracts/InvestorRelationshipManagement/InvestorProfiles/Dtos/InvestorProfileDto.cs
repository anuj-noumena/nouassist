using System;
using System.Collections.Generic;
using NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.InvestorProfiles.Dtos
{
    /// <summary>
    /// DTO representing an investor profile for read operations
    /// </summary>
    public class InvestorProfileDto : FullAuditedEntityDto<Guid>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string CompanyName { get; set; }
        public string Title { get; set; }
        public InvestorType Type { get; set; }
        public InvestorStatus Status { get; set; }
        public string Notes { get; set; }
        public DateTime? LastInteractionDate { get; set; }

        // Contact Details
        public string Email { get; set; }
        public string Phone { get; set; }
        public string LinkedIn { get; set; }
        public string Website { get; set; }
        public string Address { get; set; }

        // Communication Preferences
        public CommunicationChannel PreferredChannel { get; set; }
        public UpdateFrequency UpdateFrequency { get; set; }
        public bool AllowMarketing { get; set; }
        public List<string> CommunicationTags { get; set; } = new();

        // Relationship Score
        public int RelationshipScore { get; set; }
        public DateTime RelationshipScoreCalculatedAt { get; set; }
        public string RelationshipScoreNotes { get; set; }

        // Investment Summary
        public decimal TotalInvestedAmount { get; set; }
        public decimal TotalCommittedAmount { get; set; }
        public decimal TotalOwnershipPercentage { get; set; }
        public int ActiveInvestmentCount { get; set; }

        // Activity Summary
        public int InteractionCount { get; set; }
        public int RecentInteractionCount { get; set; } // Last 90 days
    }
}
