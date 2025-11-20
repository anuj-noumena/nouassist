using NouAssist.StartupEntrepreneurship.BusinessModelManagement.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.StartupEntrepreneurship.BusinessModelManagement.BusinessModelCanvas.Dtos
{
    public class GetBusinessModelCanvasListDto : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
        public Guid? OwnerId { get; set; }
        public ValidationStatus? ValidationStatus { get; set; }
        public DateTime? CreatedAfter { get; set; }
        public DateTime? CreatedBefore { get; set; }
    }
}
