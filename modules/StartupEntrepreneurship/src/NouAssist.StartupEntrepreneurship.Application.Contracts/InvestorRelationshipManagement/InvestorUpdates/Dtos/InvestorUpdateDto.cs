using System;
using System.Collections.Generic;
using NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.InvestorUpdates.Dtos
{
    /// <summary>
    /// DTO representing an investor update for read operations
    /// </summary>
    public class InvestorUpdateDto : FullAuditedEntityDto<Guid>
    {
        public string Title { get; set; }
        public string Subject { get; set; }
        public UpdateStatus Status { get; set; }
        public DateTime? ScheduledDate { get; set; }
        public DateTime? SentDate { get; set; }
        public UpdateFrequency Frequency { get; set; }
        public string TemplateId { get; set; }

        // Statistics
        public int SectionCount { get; set; }
        public int MetricCount { get; set; }
        public int RecipientCount { get; set; }
        public int DeliveredCount { get; set; }
        public int ReadCount { get; set; }
        public int EngagementCount { get; set; }
        public decimal EngagementRate { get; set; }

        // Collections (populated based on request)
        public List<UpdateSectionDto> Sections { get; set; } = new();
        public List<MetricSnapshotDto> Metrics { get; set; } = new();
        public List<DistributionLogDto> DistributionLogs { get; set; } = new();
        public List<EngagementRecordDto> EngagementRecords { get; set; } = new();
    }
}
