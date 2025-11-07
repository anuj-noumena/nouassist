using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace NouAssist.CoreProductivity.TimeManagement.TimeEntries.Dtos
{
    public class TimeEntryDto : FullAuditedEntityDto<Guid>
    {
        public Guid OwnerId { get; set; }
        public Guid? TaskInstanceId { get; set; }
        public Guid? ProjectId { get; set; }
        public string Description { get; set; }
        public DateTime StartedAt { get; set; }
        public DateTime? EndedAt { get; set; }
        public int? DurationMinutes { get; set; }
        public string Timezone { get; set; }
        public bool IsBillable { get; set; }
        public string Notes { get; set; }
        public List<string> Tags { get; set; }
        public bool IsRunning { get; set; }
        public int CalculatedDurationMinutes { get; set; }
        public double DurationHours { get; set; }
    }

    public class CreateTimeEntryDto
    {
        public Guid? TaskInstanceId { get; set; }
        public Guid? ProjectId { get; set; }
        public string Description { get; set; }
        public DateTime StartedAt { get; set; }
        public DateTime? EndedAt { get; set; }
        public string Timezone { get; set; }
    }

    public class UpdateTimeEntryDto
    {
        public string Description { get; set; }
        public int? DurationMinutes { get; set; }
        public bool IsBillable { get; set; }
        public string Notes { get; set; }
    }

    public class GetTimeEntryListDto : PagedAndSortedResultRequestDto
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public Guid? ProjectId { get; set; }
        public bool? IsBillable { get; set; }
    }
}
