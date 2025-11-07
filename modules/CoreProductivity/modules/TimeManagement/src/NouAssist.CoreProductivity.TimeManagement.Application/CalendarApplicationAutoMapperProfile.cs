using AutoMapper;
using NouAssist.CoreProductivity.TimeManagement.Aggregates;
using NouAssist.CoreProductivity.TimeManagement.Calendars.Dtos;
using NouAssist.CoreProductivity.TimeManagement.CalendarEvents.Dtos;
using NouAssist.CoreProductivity.TimeManagement.Entities;
using NouAssist.CoreProductivity.TimeManagement.SchedulingRequests.Dtos;
using NouAssist.CoreProductivity.TimeManagement.Timeblocks.Dtos;
using NouAssist.CoreProductivity.TimeManagement.TimeEntries.Dtos;
using NouAssist.CoreProductivity.TimeManagement.ValueObjects;

namespace NouAssist.CoreProductivity.TimeManagement
{
    public class CalendarApplicationAutoMapperProfile : Profile
    {
        public CalendarApplicationAutoMapperProfile()
        {
            // Calendar
            CreateMap<Aggregates.Calendar, CalendarDto>();
            CreateMap<CreateCalendarDto, Aggregates.Calendar>();

            // Calendar Event
            CreateMap<CalendarEvent, CalendarEventDto>()
                .ForMember(dest => dest.DurationInMinutes, opt => opt.MapFrom(src => src.DurationInMinutes))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.LinkedEntities, opt => opt.MapFrom(src => src.LinkedEntities));

            CreateMap<RecurrenceRule, RecurrenceRuleDto>().ReverseMap();
            CreateMap<LinkedEntity, LinkedEntityDto>().ReverseMap();

            // Timeblock
            CreateMap<Timeblock, TimeblockDto>()
                .ForMember(dest => dest.DurationInMinutes, opt => opt.MapFrom(src => src.DurationInMinutes))
                .ForMember(dest => dest.Tasks, opt => opt.MapFrom(src => src.Tasks));

            CreateMap<TimeblockTask, TimeblockTaskDto>();

            // Scheduling Request
            CreateMap<SchedulingRequest, SchedulingRequestDto>()
                .ForMember(dest => dest.ParticipantIds, opt => opt.MapFrom(src => src.ParticipantIds))
                .ForMember(dest => dest.Slots, opt => opt.MapFrom(src => src.Slots));

            CreateMap<SchedulingSlot, SchedulingSlotDto>()
                .ForMember(dest => dest.DurationMinutes, opt => opt.MapFrom(src => src.DurationMinutes))
                .ForMember(dest => dest.AvailableCount, opt => opt.MapFrom(src => src.AvailableCount))
                .ForMember(dest => dest.AvailableParticipants, opt => opt.MapFrom(src => src.AvailableParticipants));

            // Time Entry
            CreateMap<TimeEntry, TimeEntryDto>()
                .ForMember(dest => dest.IsRunning, opt => opt.MapFrom(src => src.IsRunning))
                .ForMember(dest => dest.CalculatedDurationMinutes, opt => opt.MapFrom(src => src.CalculatedDurationMinutes))
                .ForMember(dest => dest.DurationHours, opt => opt.MapFrom(src => src.DurationHours))
                .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.Tags));
        }
    }
}
