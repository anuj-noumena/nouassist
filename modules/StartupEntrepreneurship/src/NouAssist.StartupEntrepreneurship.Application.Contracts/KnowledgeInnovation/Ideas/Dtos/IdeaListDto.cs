using System;
using System.Collections.Generic;
using NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Ideas.Dtos
{
    public class IdeaListDto : AuditedEntityDto<Guid>
    {
        public string Title { get; set; }
        public IdeaStatus Status { get; set; }
        public string SourceName { get; set; }
        public List<string> Tags { get; set; }
        public DateTime SubmittedDate { get; set; }
        public int Priority { get; set; }
        public int EvaluationCount { get; set; }
    }
}
