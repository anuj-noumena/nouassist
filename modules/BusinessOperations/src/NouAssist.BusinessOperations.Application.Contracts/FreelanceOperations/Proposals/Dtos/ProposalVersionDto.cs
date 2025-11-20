using System;
using System.Collections.Generic;
using NouAssist.BusinessOperations.FreelanceOperations.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.BusinessOperations.FreelanceOperations.Proposals.Dtos
{
    public class ProposalVersionDto : EntityDto<Guid>
    {
        public Guid ProposalId { get; set; }
        public string VersionNumber { get; set; }
        public string VersionName { get; set; }
        public string Summary { get; set; }
        public List<string> KeySections { get; set; }
        public BillingType BillingType { get; set; }
        public decimal? HourlyRate { get; set; }
        public string HourlyRateCurrency { get; set; }
        public decimal? FixedPrice { get; set; }
        public string Currency { get; set; }
        public int? EstimatedHours { get; set; }
        public string PaymentTerms { get; set; }
        public string DocumentId { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid CreatedBy { get; set; }
        public string CreatedByName { get; set; }
        public string Notes { get; set; }
        public string FormattedPrice { get; set; }
    }
}
