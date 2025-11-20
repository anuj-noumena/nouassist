using System;
using Volo.Abp.Application.Dtos;

namespace NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.InvestorProfiles.Dtos
{
    /// <summary>
    /// DTO representing an investment record
    /// </summary>
    public class InvestmentRecordDto : FullAuditedEntityDto<Guid>
    {
        public Guid InvestorId { get; set; }
        public decimal InvestedAmount { get; set; }
        public decimal CommittedAmount { get; set; }
        public decimal OwnershipPercentage { get; set; }
        public string InvestmentRound { get; set; }
        public DateTime InvestmentDate { get; set; }
        public string DocumentReference { get; set; }
        public bool IsActive { get; set; }
    }
}
