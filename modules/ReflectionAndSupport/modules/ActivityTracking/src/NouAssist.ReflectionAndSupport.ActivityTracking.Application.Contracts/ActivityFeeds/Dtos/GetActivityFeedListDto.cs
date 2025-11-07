using System;
using Volo.Abp.Application.Dtos;

namespace NouAssist.ReflectionAndSupport.ActivityTracking.ActivityFeeds.Dtos
{
    public class GetActivityFeedListDto : PagedAndSortedResultRequestDto
    {
        public Guid? ActorId { get; set; }
        public string EntityType { get; set; }
        public Guid? EntityId { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public bool? IsRead { get; set; }
    }
}
