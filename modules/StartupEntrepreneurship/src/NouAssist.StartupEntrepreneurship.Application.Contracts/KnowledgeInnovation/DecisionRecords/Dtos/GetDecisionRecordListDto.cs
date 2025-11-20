using System;
using NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.StartupEntrepreneurship.KnowledgeInnovation.DecisionRecords.Dtos
{
    public class GetDecisionRecordListDto : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
        public DecisionStatus? Status { get; set; }
        public string Tag { get; set; }
        public DateTime? SinceDate { get; set; }
        public bool? CreatedByCurrentUser { get; set; }
    }
}
