using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace NouAssist.StartupEntrepreneurship.BusinessModelManagement.RevenueModel.Dtos
{
    public class GoToMarketStrategyDto : EntityDto<Guid>
    {
        public Guid RevenueModelId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string TargetMarket { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsActive { get; set; }
        public List<GtmPlanElementDto> PlanElements { get; set; }

        public GoToMarketStrategyDto()
        {
            PlanElements = new List<GtmPlanElementDto>();
        }
    }
}
