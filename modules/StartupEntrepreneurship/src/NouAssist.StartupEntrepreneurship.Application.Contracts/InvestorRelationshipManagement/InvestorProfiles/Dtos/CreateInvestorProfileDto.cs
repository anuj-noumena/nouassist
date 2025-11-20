using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.Enums;

namespace NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.InvestorProfiles.Dtos
{
    /// <summary>
    /// DTO for creating a new investor profile
    /// </summary>
    public class CreateInvestorProfileDto
    {
        [Required]
        [StringLength(200)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(200)]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(256)]
        public string Email { get; set; }

        [Required]
        public InvestorType Type { get; set; }

        [StringLength(300)]
        public string CompanyName { get; set; }

        [StringLength(200)]
        public string Title { get; set; }

        [StringLength(50)]
        public string Phone { get; set; }

        [StringLength(500)]
        public string LinkedIn { get; set; }

        [StringLength(500)]
        public string Website { get; set; }

        [StringLength(500)]
        public string Address { get; set; }

        public CommunicationChannel PreferredChannel { get; set; } = CommunicationChannel.Email;
        public UpdateFrequency UpdateFrequency { get; set; } = UpdateFrequency.Monthly;
        public bool AllowMarketing { get; set; } = true;
        public List<string> CommunicationTags { get; set; } = new();

        [StringLength(2000)]
        public string Notes { get; set; }
    }
}
