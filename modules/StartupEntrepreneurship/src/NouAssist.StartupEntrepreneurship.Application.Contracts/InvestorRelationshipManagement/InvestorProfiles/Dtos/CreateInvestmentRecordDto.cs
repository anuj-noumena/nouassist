using System;
using System.ComponentModel.DataAnnotations;

namespace NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.InvestorProfiles.Dtos
{
    /// <summary>
    /// DTO for creating a new investment record
    /// </summary>
    public class CreateInvestmentRecordDto
    {
        [Required]
        [Range(0, double.MaxValue)]
        public decimal InvestedAmount { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal CommittedAmount { get; set; }

        [Required]
        [Range(0, 100)]
        public decimal OwnershipPercentage { get; set; }

        [Required]
        [StringLength(100)]
        public string InvestmentRound { get; set; }

        [Required]
        public DateTime InvestmentDate { get; set; }

        [StringLength(200)]
        public string DocumentReference { get; set; }
    }
}
