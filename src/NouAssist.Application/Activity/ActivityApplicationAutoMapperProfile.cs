using AutoMapper;
using NouAssist.Activity.ActivityFeeds.Dtos;
using NouAssist.Activity.Aggregates;
using NouAssist.Activity.AuditLogs.Dtos;

namespace NouAssist.Activity
{
    public class ActivityApplicationAutoMapperProfile : Profile
    {
        public ActivityApplicationAutoMapperProfile()
        {
            CreateMap<ActivityFeed, ActivityFeedDto>();
            CreateMap<AuditLog, AuditLogDto>();
            CreateMap<EntityHistory, EntityHistoryDto>();
            CreateMap<Aggregates.EntityHistory, EntityHistoryDto>();
        }
    }
}
