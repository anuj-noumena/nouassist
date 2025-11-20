using System;
using System.Collections.Generic;
using NouAssist.StartupEntrepreneurship.GrowthExperimentation.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.StartupEntrepreneurship.GrowthExperimentation.MarketingChannels.Dtos
{
    public class MarketingChannelDto : FullAuditedEntityDto<Guid>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public MarketingChannelType Type { get; set; }
        public ChannelDetailsDto Details { get; set; }
        public bool IsActive { get; set; }
        public CostPerAcquisitionDto CostPerAcquisition { get; set; }
        public ReturnOnAdSpendDto ReturnOnAdSpend { get; set; }
        public List<ChannelMetricDto> Metrics { get; set; }
    }
}
