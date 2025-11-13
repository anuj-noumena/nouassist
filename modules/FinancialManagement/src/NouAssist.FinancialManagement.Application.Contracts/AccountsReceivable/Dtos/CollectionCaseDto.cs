using System;
using System.Collections.Generic;
using NouAssist.FinancialManagement.AccountsReceivable.Enums;
using NouAssist.FinancialManagement.Shared.ValueObjects;
using Volo.Abp.Application.Dtos;

namespace NouAssist.FinancialManagement.AccountsReceivable.Dtos
{
    public class CollectionCaseDto : FullAuditedEntityDto<Guid>
    {
        public Guid CustomerId { get; set; }
        public string CustomerName { get; set; }
        public MonetaryValue TotalOverdueAmount { get; set; }
        public CollectionStatus Status { get; set; }
        public Guid AssignedTo { get; set; }
        public string AssignedToName { get; set; }
        public DateTime? LastContactDate { get; set; }
        public List<CollectionActivityDto> Activities { get; set; }
        public int DaysSinceLastContact => LastContactDate.HasValue 
            ? (DateTime.Today - LastContactDate.Value).Days 
            : 0;
    }

    public class CollectionActivityDto
    {
        public string ActivityType { get; set; }
        public string Description { get; set; }
        public DateTime ActivityDate { get; set; }
        public string ContactedPerson { get; set; }
        public DateTime? FollowUpDate { get; set; }
        public Guid LoggedBy { get; set; }
    }

    public class CreateCollectionCaseDto
    {
        public Guid CustomerId { get; set; }
        public Guid AssignedTo { get; set; }
    }

    public class AddCollectionActivityDto
    {
        public string ActivityType { get; set; }
        public string Description { get; set; }
        public DateTime ActivityDate { get; set; }
        public string ContactedPerson { get; set; }
        public DateTime? FollowUpDate { get; set; }
    }

    public class UpdateCollectionStatusDto
    {
        public CollectionStatus NewStatus { get; set; }
    }
}
