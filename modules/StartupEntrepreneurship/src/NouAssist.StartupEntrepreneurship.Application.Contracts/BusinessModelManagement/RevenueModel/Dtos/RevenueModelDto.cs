using System;
using System.Collections.Generic;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.StartupEntrepreneurship.BusinessModelManagement.RevenueModel.Dtos
{
    public class RevenueModelDto : FullAuditedEntityDto<Guid>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public RevenueModelType Type { get; set; }
        public Guid OwnerId { get; set; }
        public bool IsActive { get; set; }
        public List<PricingModelDto> PricingModels { get; set; }
        public List<RevenueScenarioDto> Scenarios { get; set; }
        public List<GoToMarketStrategyDto> GtmStrategies { get; set; }

        public RevenueModelDto()
        {
            PricingModels = new List<PricingModelDto>();
            Scenarios = new List<RevenueScenarioDto>();
            GtmStrategies = new List<GoToMarketStrategyDto>();
        }
    }
}
