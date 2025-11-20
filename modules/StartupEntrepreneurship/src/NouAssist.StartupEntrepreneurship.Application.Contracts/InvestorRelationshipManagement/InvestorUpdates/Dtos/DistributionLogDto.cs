using System;
using NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.InvestorUpdates.Dtos
{
    /// <summary>
    /// DTO representing a distribution log entry
    /// </summary>
    public class DistributionLogDto : EntityDto<Guid>
    {
        public Guid UpdateId { get; set; }
        public Guid InvestorId { get; set; }
        public string InvestorName { get; set; }
        public UpdateStatus Status { get; set; }
        public DateTime AttemptedAt { get; set; }
        public DateTime? DeliveredAt { get; set; }
        public string ErrorMessage { get; set; }
        public string Channel { get; set; }
    }
}
