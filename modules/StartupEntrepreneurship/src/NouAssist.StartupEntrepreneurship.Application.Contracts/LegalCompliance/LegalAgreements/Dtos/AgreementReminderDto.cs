using System;
using Volo.Abp.Application.Dtos;

namespace NouAssist.StartupEntrepreneurship.LegalCompliance.LegalAgreements.Dtos
{
    public class AgreementReminderDto : EntityDto<Guid>
    {
        public Guid AgreementId { get; set; }
        public string Title { get; set; }
        public DateTime ReminderDate { get; set; }
        public string Description { get; set; }
        public int? DaysBeforeEvent { get; set; }
        public bool IsRecurring { get; set; }
        public string RecurrencePattern { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime? CompletedDate { get; set; }
    }
