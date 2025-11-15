using System;
using System.Collections.Generic;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace NouAssist.DocumentManagement.Domain.Shared.Events
{
    [EventName("NouAssist.DocumentManagement.DocumentOCRCompleted")]
    public class DocumentOCRCompletedEto : IMultiTenant
    {
        public Guid DocumentId { get; set; }
        public Guid? TenantId { get; set; }
        public string ExtractedText { get; set; }
        public Dictionary<string, string> ExtractedData { get; set; }
        public decimal ConfidenceScore { get; set; }
        public DateTime ProcessedAt { get; set; }
    }
}
