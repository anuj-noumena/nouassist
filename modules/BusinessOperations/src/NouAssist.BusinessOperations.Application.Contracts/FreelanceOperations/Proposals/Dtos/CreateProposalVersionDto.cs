using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using NouAssist.BusinessOperations.FreelanceOperations.Constants;
using NouAssist.BusinessOperations.FreelanceOperations.Enums;

namespace NouAssist.BusinessOperations.FreelanceOperations.Proposals.Dtos
{
    public class CreateProposalVersionDto
    {
        [Required]
        public string VersionNumber { get; set; }

        [Required]
        [StringLength(FreelanceOperationsConsts.Proposal.MaxVersionNameLength)]
        public string VersionName { get; set; }

        [Required]
        [StringLength(FreelanceOperationsConsts.Proposal.MaxSummaryLength)]
        public string Summary { get; set; }

        public List<string> KeySections { get; set; }

        [Required]
        public BillingType BillingType { get; set; }

        // Hourly billing fields
        public decimal? HourlyRate { get; set; }
        public string HourlyRateCurrency { get; set; }
        public int? EstimatedHours { get; set; }

        // Fixed price billing fields
        public decimal? FixedPrice { get; set; }
        public string FixedPriceCurrency { get; set; }

        public string PaymentTerms { get; set; }

        [Required]
        public string DocumentId { get; set; }

        public string Notes { get; set; }
    }
}
