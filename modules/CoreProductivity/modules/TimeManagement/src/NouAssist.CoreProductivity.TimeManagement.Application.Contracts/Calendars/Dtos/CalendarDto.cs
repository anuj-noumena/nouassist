using System;
using Volo.Abp.Application.Dtos;

namespace NouAssist.CoreProductivity.TimeManagement.Calendars.Dtos
{
    public class CalendarDto : FullAuditedEntityDto<Guid>
    {
        public Guid OwnerId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Color { get; set; }
        public string Timezone { get; set; }
        public bool IsActive { get; set; }
        public bool IsDefault { get; set; }
        public bool IsShared { get; set; }
    }

    public class CreateCalendarDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Color { get; set; }
        public string Timezone { get; set; }
        public bool IsDefault { get; set; }
    }

    public class UpdateCalendarDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Color { get; set; }
        public string Timezone { get; set; }
    }

    public class GetCalendarListDto : PagedAndSortedResultRequestDto
    {
        public bool? IsActive { get; set; }
        public bool? IsShared { get; set; }
    }

    public class ShareCalendarDto
    {
        public Guid SharedWithUserId { get; set; }
        public string PermissionLevel { get; set; } // "Read" or "Write"
    }
}
