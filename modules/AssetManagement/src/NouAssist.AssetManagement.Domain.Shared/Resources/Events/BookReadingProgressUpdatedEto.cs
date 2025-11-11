using System;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace NouAssist.Resources.Events
{
    [EventName("NouAssist.Resources.BookProgressUpdated")]
    public class BookReadingProgressUpdatedEto : IMultiTenant
    {
        public Guid BookId { get; set; }
        public Guid OwnerId { get; set; }
        public string ReadingStatus { get; set; }
        public double? ProgressPercentage { get; set; }
        public int? CurrentPage { get; set; }
        public Guid? TenantId { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
