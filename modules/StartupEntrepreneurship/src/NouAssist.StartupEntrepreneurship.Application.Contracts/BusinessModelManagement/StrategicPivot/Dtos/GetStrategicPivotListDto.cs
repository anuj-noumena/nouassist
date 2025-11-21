using System;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.StartupEntrepreneurship.BusinessModelManagement.StrategicPivot.Dtos
{
    public class GetStrategicPivotListDto : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
        public PivotType? Type { get; set; }
        public PivotStatus? Status { get; set; }
        public Guid? InitiatorId { get; set; }
        public DateTime? InitiatedAfter { get; set; }
        public DateTime? InitiatedBefore { get; set; }
        public bool? IncludeOverdue { get; set; }
    }
}
