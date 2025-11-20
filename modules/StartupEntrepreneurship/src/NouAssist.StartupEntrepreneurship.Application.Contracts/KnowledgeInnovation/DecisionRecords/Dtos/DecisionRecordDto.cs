using System;
using System.Collections.Generic;
using NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.StartupEntrepreneurship.KnowledgeInnovation.DecisionRecords.Dtos
{
    public class DecisionRecordDto : FullAuditedEntityDto<Guid>
    {
        public string Title { get; set; }
        public DecisionStatus Status { get; set; }
        public DateTime DecisionDate { get; set; }
        public List<string> Tags { get; set; }
        public DecisionContextDto Context { get; set; }
        public DecisionRationaleDto Rationale { get; set; }
        public List<DecisionAlternativeDto> Alternatives { get; set; }
        public DecisionOutcomeDto Outcome { get; set; }
    }

    public class DecisionContextDto
    {
        public string Background { get; set; }
        public string ProblemStatement { get; set; }
        public List<string> Stakeholders { get; set; }
        public List<string> Constraints { get; set; }
        public DateTime ContextCapturedAt { get; set; }
    }

    public class DecisionRationaleDto
    {
        public string Reasoning { get; set; }
        public List<string> KeyFactors { get; set; }
        public List<string> RisksIdentified { get; set; }
        public List<string> AssumptionsMade { get; set; }
    }

    public class DecisionAlternativeDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string ProsCons { get; set; }
        public bool IsChosen { get; set; }
    }

    public class DecisionOutcomeDto
    {
        public string Description { get; set; }
        public DateTime? ImplementationDate { get; set; }
        public string LessonsLearned { get; set; }
        public List<OutcomeImpactDto> Impacts { get; set; }
    }

    public class OutcomeImpactDto
    {
        public string Area { get; set; }
        public string Description { get; set; }
        public int Magnitude { get; set; }
    }
}
