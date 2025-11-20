using System;
using System.Collections.Generic;
using NouAssist.StartupEntrepreneurship.FundraisingPipeline.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.StartupEntrepreneurship.FundraisingPipeline.FundraisingRound.Dtos
{
    public class FundraisingRoundDto : AuditedEntityDto<Guid>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public RoundType Type { get; set; }
        public string TypeName => Type.ToString();
        public RoundStage Stage { get; set; }
        public string StageName => Stage.ToString();
        public decimal TargetAmount { get; set; }
        public decimal CommittedAmount { get; set; }
        public decimal ProgressPercentage => TargetAmount > 0 ? (CommittedAmount / TargetAmount) * 100 : 0;
        public DateTime StartDate { get; set; }
        public DateTime? TargetCloseDate { get; set; }
        public DateTime? ActualCloseDate { get; set; }
        public List<PipelineStageDto> PipelineStages { get; set; }
        public int ProspectCount { get; set; }
    }
}
