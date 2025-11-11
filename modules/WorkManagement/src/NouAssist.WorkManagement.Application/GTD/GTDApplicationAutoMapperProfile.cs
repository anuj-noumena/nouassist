using AutoMapper;
using NouAssist.Core.ValueObjects;
using NouAssist.GTD.Aggregates;
using NouAssist.GTD.Dtos;

namespace NouAssist.GTD
{
    public class GtdApplicationAutoMapperProfile : Profile
    {
        public GtdApplicationAutoMapperProfile()
        {
            // GtdHorizon
            CreateMap<GtdHorizon, GtdHorizonDto>()
                .ForMember(dest => dest.NeedsReview, opt => opt.MapFrom(src => src.NeedsReview))
                .ForMember(dest => dest.TotalRelatedEntities, opt => opt.MapFrom(src => src.TotalRelatedEntities));

            CreateMap<GtdHorizonAlignment, GtdHorizonAlignmentDto>()
                .ForMember(dest => dest.HasStrongAlignment, opt => opt.MapFrom(src => src.HasStrongAlignment))
                .ForMember(dest => dest.HasWeakAlignment, opt => opt.MapFrom(src => src.HasWeakAlignment));

            // TicklerItem
            CreateMap<TicklerItem, TicklerItemDto>()
                .ForMember(dest => dest.ImportanceScore, opt => opt.MapFrom(src => src.Importance.Value))
                .ForMember(dest => dest.UrgencyScore, opt => opt.MapFrom(src => src.Urgency.Value))
                .ForMember(dest => dest.IsDue, opt => opt.MapFrom(src => src.IsDue))
                .ForMember(dest => dest.RecurrencePattern, opt => opt.MapFrom(src => src.RecurrencePattern));

            CreateMap<RecurrenceRule, RecurrenceRuleDto>()
                .ForMember(dest => dest.Frequency, opt => opt.MapFrom(src => src.Frequency.ToString()));

            // SomedayMaybe
            CreateMap<SomedayMaybeItem, SomedayMaybeItemDto>()
                .ForMember(dest => dest.ImportanceScore, opt => opt.MapFrom(src => src.Importance.Value))
                .ForMember(dest => dest.UrgencyScore, opt => opt.MapFrom(src => src.Urgency.Value))
                .ForMember(dest => dest.AgeInDays, opt => opt.MapFrom(src => src.AgeInDays))
                .ForMember(dest => dest.NeedsReview, opt => opt.MapFrom(src => src.NeedsReview));

            // WaitingFor
            CreateMap<WaitingFor, WaitingForDto>()
                .ForMember(dest => dest.WaitingDays, opt => opt.MapFrom(src => src.WaitingDays))
                .ForMember(dest => dest.IsFollowUpDue, opt => opt.MapFrom(src => src.IsFollowUpDue));

            // Followup
            CreateMap<Followup, FollowupDto>()
                .ForMember(dest => dest.IsDue, opt => opt.MapFrom(src => src.IsDue));
        }
    }
}
