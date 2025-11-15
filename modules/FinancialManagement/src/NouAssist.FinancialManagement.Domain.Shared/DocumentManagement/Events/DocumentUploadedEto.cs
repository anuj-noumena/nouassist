using System;
using NouAssist.DocumentManagement.Enums;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace NouAssist.DocumentManagement.Domain.Shared.Events
{
    [EventName("NouAssist.DocumentManagement.DocumentUploaded")]
    public class DocumentUploadedEto : IMultiTenant
    {
        public Guid DocumentId { get; set; }
        public Guid? TenantId { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public long FileSize { get; set; }
        public DocumentType DocumentType { get; set; }
        public Guid UploadedBy { get; set; }
        public DateTime UploadedAt { get; set; }
        public string EntityType { get; set; }
        public string EntityId { get; set; }
    }
}
