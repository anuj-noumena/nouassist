using System;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace NouAssist.FinancialManagement.DocumentManagement.Events
{
    [EventName("NouAssist.DocumentManagement.DocumentDeleted")]
    public class DocumentDeletedEto : IMultiTenant
    {
        public Guid DocumentId { get; set; }
        public Guid? TenantId { get; set; }
        public string FileName { get; set; }
        public Guid DeletedBy { get; set; }
        public DateTime DeletedAt { get; set; }
        public string Reason { get; set; }
    }
}
