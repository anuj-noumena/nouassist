using System;
using NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.InvestorProfiles.Dtos
{
    /// <summary>
    /// DTO representing an investor interaction
    /// </summary>
    public class InvestorInteractionDto : FullAuditedEntityDto<Guid>
    {
        public Guid InvestorId { get; set; }
        public InteractionType Type { get; set; }
        public string Subject { get; set; }
        public string Notes { get; set; }
        public DateTime InteractionDate { get; set; }
        public string Outcome { get; set; }
        public Guid? RelatedEntityId { get; set; }
        public string RelatedEntityType { get; set; }
    }
}
