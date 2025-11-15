using System;
using NouAssist.CreditScoreMonitoring.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.CreditScoreMonitoring.CreditDisputes.Dtos
{
    public class CreditDisputeDto : FullAuditedEntityDto<Guid>
    {
        public Guid UserId { get; set; }
        public CreditBureau Bureau { get; set; }
        public string ItemType { get; set; }
        public string ItemDescription { get; set; }
        public string DisputeReason { get; set; }
        public string SupportingDocumentation { get; set; }
        public DisputeStatus Status { get; set; }
        public DateTime SubmittedDate { get; set; }
        public DateTime? ResolutionDate { get; set; }
        public string ResolutionNotes { get; set; }
        public string ConfirmationNumber { get; set; }
    }

    public class CreateCreditDisputeDto
    {
        public CreditBureau Bureau { get; set; }
        public string ItemType { get; set; }
        public string ItemDescription { get; set; }
        public string DisputeReason { get; set; }
        public string SupportingDocumentation { get; set; }
    }

    public class UpdateDisputeStatusDto
    {
        public DisputeStatus Status { get; set; }
        public string ResolutionNotes { get; set; }
    }

    public class GetCreditDisputeListDto : PagedAndSortedResultRequestDto
    {
        public DisputeStatus? Status { get; set; }
        public CreditBureau? Bureau { get; set; }
    }
}
