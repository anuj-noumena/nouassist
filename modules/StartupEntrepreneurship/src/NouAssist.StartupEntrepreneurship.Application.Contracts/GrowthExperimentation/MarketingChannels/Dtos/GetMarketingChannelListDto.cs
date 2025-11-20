using NouAssist.StartupEntrepreneurship.GrowthExperimentation.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.StartupEntrepreneurship.GrowthExperimentation.MarketingChannels.Dtos
{
    public class GetMarketingChannelListDto : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
        public MarketingChannelType? Type { get; set; }
        public bool? IsActive { get; set; }
    }
}
