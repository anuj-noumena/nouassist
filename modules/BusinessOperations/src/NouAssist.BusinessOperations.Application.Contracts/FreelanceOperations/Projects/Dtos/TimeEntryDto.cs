using System;
using Volo.Abp.Application.Dtos;

namespace NouAssist.BusinessOperations.FreelanceOperations.Projects.Dtos
{
    public class TimeEntryDto : EntityDto
    {
        public Guid ProjectId { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan StartTime { get; set; }
        public int DurationMinutes { get; set; }
        public string Description { get; set; }
        public bool IsBillable { get; set; }
        public bool IsInvoiced { get; set; }
        public Guid? InvoiceId { get; set; }
        public string FormattedDuration { get; set; }
        public decimal DurationInHours { get; set; }
    }
}
