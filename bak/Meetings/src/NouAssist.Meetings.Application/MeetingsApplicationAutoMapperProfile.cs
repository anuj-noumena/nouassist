using AutoMapper;
using NouAssist.Meetings.Aggregates;
using NouAssist.Meetings.Meetings.Dtos;
using NouAssist.Meetings.ValueObjects;

namespace NouAssist.Meetings
{
    public class MeetingsApplicationAutoMapperProfile : Profile
    {
        public MeetingsApplicationAutoMapperProfile()
        {
            CreateMap<Meeting, MeetingDto>()
                .ForMember(dest => dest.AttendeeCount, opt => opt.MapFrom(src => src.GetAttendeeCount()))
                .ForMember(dest => dest.ConfirmedCount, opt => opt.MapFrom(src => src.GetConfirmedCount()))
                .ForMember(dest => dest.DeclinedCount, opt => opt.MapFrom(src => src.GetDeclinedCount()))
                .ForMember(dest => dest.AcceptanceRate, opt => opt.MapFrom(src => 
                    src.GetAttendeeCount() > 0 ? (double)src.GetConfirmedCount() / src.GetAttendeeCount() * 100 : 0))
                .ForMember(dest => dest.DurationMinutes, opt => opt.MapFrom(src => src.Duration.Minutes))
                .ForMember(dest => dest.IsUpcoming, opt => opt.MapFrom(src => src.IsUpcoming))
                .ForMember(dest => dest.IsPast, opt => opt.MapFrom(src => src.IsPast))
                .ForMember(dest => dest.IsHappeningNow, opt => opt.MapFrom(src => src.IsHappeningNow))
                .ForMember(dest => dest.HasRecording, opt => opt.MapFrom(src => src.HasRecording))
                .ForMember(dest => dest.OwnerName, opt => opt.Ignore())
                .ForMember(dest => dest.OrganizerName, opt => opt.Ignore());

            CreateMap<MeetingAttendee, MeetingAttendeeDto>();
            CreateMap<MeetingAgendaItem, MeetingAgendaItemDto>()
                .ForMember(dest => dest.PresenterName, opt => opt.Ignore());
            CreateMap<MeetingActionItem, MeetingActionItemDto>()
                .ForMember(dest => dest.IsOverdue, opt => opt.MapFrom(src => src.IsOverdue))
                .ForMember(dest => dest.AssignedToName, opt => opt.Ignore());
        }
    }
}
