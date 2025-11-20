using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace NouAssist.StartupEntrepreneurship.GrowthExperimentation.ConversionFunnels.Dtos
{
    public class ConversionFunnelDto : FullAuditedEntityDto<Guid>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public List<FunnelStageDto> Stages { get; set; }
        public double OverallConversionRate { get; set; }
    }
}
