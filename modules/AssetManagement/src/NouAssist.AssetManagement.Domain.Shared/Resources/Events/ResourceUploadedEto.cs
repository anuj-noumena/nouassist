using System;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace NouAssist.AssetManagement.Resources.Events
{
    [EventName("NouAssist.Resources.Uploaded")]
    public class ResourceUploadedEto : IMultiTenant
    {
        public Guid ResourceId { get; set; }
        public Guid OwnerId { get; set; }
        public string ResourceKind { get; set; }
        public string MimeType { get; set; }
        public long SizeBytes { get; set; }
        public Guid? TenantId { get; set; }
        public DateTime UploadedAt { get; set; }
    }
}
