using System;
using System.Collections.Generic;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.StartupEntrepreneurship.BusinessModelManagement.RevenueModel.Dtos
{
    public class PricingModelDto : EntityDto<Guid>
    {
        public Guid RevenueModelId { get; set; }
        public string Name { get; set; }
        public PricingStrategy Strategy { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public List<PricingTierDto> PricingTiers { get; set; }

        public PricingModelDto()
        {
            PricingTiers = new List<PricingTierDto>();
        }
    }
}
