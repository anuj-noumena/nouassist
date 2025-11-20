using System;
using System.Collections.Generic;
using NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Ideas.Dtos
{
    public class IdeaDto : FullAuditedEntityDto<Guid>
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public IdeaStatus Status { get; set; }
        public IdeaSourceDto Source { get; set; }
        public List<string> Tags { get; set; }
        public DateTime SubmittedDate { get; set; }
        public string ImpactStatement { get; set; }
        public int Priority { get; set; }
        public List<IdeaEvaluationDto> Evaluations { get; set; }
    }

    public class IdeaSourceDto
    {
        public string SourceName { get; set; }
        public string SourceDescription { get; set; }
        public DateTime SourcedDate { get; set; }
    }

    public class IdeaEvaluationDto
    {
        public Guid EvaluatorId { get; set; }
        public string EvaluatorName { get; set; }
        public DateTime EvaluationDate { get; set; }
        public string Comments { get; set; }
        public bool IsApproved { get; set; }
        public List<EvaluationScoreDto> Scores { get; set; }
    }

    public class EvaluationScoreDto
    {
        public string CriteriaName { get; set; }
        public int Score { get; set; }
        public string Justification { get; set; }
    }
}
