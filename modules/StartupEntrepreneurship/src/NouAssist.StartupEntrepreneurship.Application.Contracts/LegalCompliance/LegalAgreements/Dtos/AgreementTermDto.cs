using System;
using Volo.Abp.Application.Dtos;

namespace NouAssist.StartupEntrepreneurship.LegalCompliance.LegalAgreements.Dtos
{
    public class AgreementTermDto : EntityDto<Guid>
    {
        public Guid AgreementId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsCritical { get; set; }
        public DateTime? DeadlineDate { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime? CompletedDate { get; set; }
    }
