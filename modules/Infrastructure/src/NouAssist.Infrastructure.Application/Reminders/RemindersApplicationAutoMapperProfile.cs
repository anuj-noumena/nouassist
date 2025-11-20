using System.Linq;
using AutoMapper;
using NouAssist.Infrastructure.Reminders.Aggregates;
using NouAssist.Infrastructure.Reminders.ValueObjects;
using NouAssist.Notification.Reminders.Reminders.Dtos;

namespace NouAssist.Infrastructure.Reminders
{
    public class RemindersApplicationAutoMapperProfile : Profile
    {
        public RemindersApplicationAutoMapperProfile()
        {
            CreateMap<Reminder, ReminderDto>()
                .ForMember(dest => dest.IsPending, opt => opt.MapFrom(src => src.IsPending))
                .ForMember(dest => dest.IsOverdue, opt => opt.MapFrom(src => src.IsOverdue))
                .ForMember(dest => dest.IsDue, opt => opt.MapFrom(src => src.IsDue))
                .ForMember(dest => dest.IsRecurring, opt => opt.MapFrom(src => src.IsRecurring))
                .ForMember(dest => dest.HoursUntilReminder, opt => opt.MapFrom(src => src.GetHoursUntilReminder()))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.Priority, opt => opt.MapFrom(src => src.Priority.ToString()))
                .ForMember(dest => dest.RefEntityType, opt => opt.MapFrom(src => src.RefEntityType != null ? src.RefEntityType.ToString() : null));

            CreateMap<ReminderSchedule, ReminderScheduleDto>()
                .ForMember(dest => dest.RecurrencePattern, opt => opt.MapFrom(src => src.RecurrencePattern.ToString()))
                .ForMember(dest => dest.RecurrenceDaysOfWeek, opt => opt.MapFrom(src => 
                    src.RecurrenceDaysOfWeek.Select(d => d.ToString()).ToArray()));

            CreateMap<SnoozeInfo, SnoozeInfoDto>();
        }
    }
}
