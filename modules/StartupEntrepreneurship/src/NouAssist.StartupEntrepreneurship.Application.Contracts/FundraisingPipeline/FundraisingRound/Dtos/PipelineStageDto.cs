using System;
using Volo.Abp.Application.Dtos;

namespace NouAssist.StartupEntrepreneurship.FundraisingPipeline.FundraisingRound.Dtos
{
    public class PipelineStageDto : EntityDto<Guid>
    {
        public Guid FundraisingRoundId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Order { get; set; }
        public int ProspectCount { get; set; }
    }
}
