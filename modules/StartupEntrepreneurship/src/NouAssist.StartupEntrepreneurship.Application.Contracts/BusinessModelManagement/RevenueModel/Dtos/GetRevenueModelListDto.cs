using NouAssist.StartupEntrepreneurship.BusinessModelManagement.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.StartupEntrepreneurship.BusinessModelManagement.RevenueModel.Dtos
{
    public class GetRevenueModelListDto : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
        public Guid? OwnerId { get; set; }
        public RevenueModelType? Type { get; set; }
        public bool? IsActive { get; set; }
    }
}
