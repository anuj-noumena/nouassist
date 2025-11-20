using System;
using System.ComponentModel.DataAnnotations;
using NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.Enums;

namespace NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.InvestorProfiles.Dtos
{
    /// <summary>
    /// DTO for creating a new investor interaction
    /// </summary>
    public class CreateInvestorInteractionDto
    {
        [Required]
        public InteractionType Type { get; set; }

        [Required]
        [StringLength(500)]
        public string Subject { get; set; }

        [StringLength(2000)]
        public string Notes { get; set; }

        [Required]
        public DateTime InteractionDate { get; set; }

        [StringLength(1000)]
        public string Outcome { get; set; }

        public Guid? RelatedEntityId { get; set; }

        [StringLength(100)]
        public string RelatedEntityType { get; set; }
    }
}
