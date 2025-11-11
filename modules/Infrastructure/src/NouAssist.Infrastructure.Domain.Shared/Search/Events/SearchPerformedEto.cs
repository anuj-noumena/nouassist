using System;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace NouAssist.Infrastructure.Search.Events
{
    [EventName("NouAssist.Search.Performed")]
    public class SearchPerformedEto : IMultiTenant
    {
        public Guid UserId { get; set; }
        public string Query { get; set; }
        public int ResultCount { get; set; }
        public int SearchDurationMs { get; set; }
        public double QualityScore { get; set; }
        public Guid? TenantId { get; set; }
        public DateTime SearchedAt { get; set; }
    }
}
