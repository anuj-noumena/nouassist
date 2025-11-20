using System;
using Volo.Abp.Application.Dtos;

namespace NouAssist.BusinessOperations.FreelanceOperations.Projects.Dtos
{
    public class MilestoneDto : EntityDto<Guid>
    {
        public Guid ProjectId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string Currency { get; set; }
        public string AcceptanceCriteria { get; set; }
        public DateTime? Deadline { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime? CompletedDate { get; set; }
        public bool IsInvoiced { get; set; }
        public Guid? InvoiceId { get; set; }
        public bool IsOverdue { get; set; }
    }
}
