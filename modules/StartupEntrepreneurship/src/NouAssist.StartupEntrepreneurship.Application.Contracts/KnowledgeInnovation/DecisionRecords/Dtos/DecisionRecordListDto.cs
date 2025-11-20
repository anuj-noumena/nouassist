using System;
using System.Collections.Generic;
using NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.StartupEntrepreneurship.KnowledgeInnovation.DecisionRecords.Dtos
{
    public class DecisionRecordListDto : AuditedEntityDto<Guid>
    {
        public string Title { get; set; }
        public DecisionStatus Status { get; set; }
        public DateTime DecisionDate { get; set; }
        public List<string> Tags { get; set; }
        public int AlternativesCount { get; set; }
        public bool HasOutcome { get; set; }
    }
}
