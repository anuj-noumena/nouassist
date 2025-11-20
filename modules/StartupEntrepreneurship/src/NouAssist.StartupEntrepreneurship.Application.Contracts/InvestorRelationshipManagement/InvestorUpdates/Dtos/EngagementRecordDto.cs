using System;
using NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.InvestorUpdates.Dtos
{
    /// <summary>
    /// DTO representing an engagement record
    /// </summary>
    public class EngagementRecordDto : EntityDto<Guid>
    {
        public Guid UpdateId { get; set; }
        public Guid InvestorId { get; set; }
        public string InvestorName { get; set; }
        public EngagementType Type { get; set; }
        public DateTime EngagedAt { get; set; }
        public string Details { get; set; }
        public string IpAddress { get; set; }
        public string UserAgent { get; set; }
    }
}
