using System;
using Volo.Abp.Application.Dtos;

namespace NouAssist.StartupEntrepreneurship.LegalCompliance.IntellectualProperty.Dtos
{
    public class IPDeadlineDto : EntityDto<Guid>
    {
        public Guid AssetId { get; set; }
        public string Title { get; set; }
        public DateTime Deadline { get; set; }
        public string Description { get; set; }
        public bool IsRecurring { get; set; }
        public string RecurrencePattern { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime? CompletedDate { get; set; }
    }
