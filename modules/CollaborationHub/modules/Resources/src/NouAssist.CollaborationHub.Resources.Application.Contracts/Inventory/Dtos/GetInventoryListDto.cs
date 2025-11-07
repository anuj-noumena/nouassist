using NouAssist.CollaborationHub.Resources.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.CollaborationHub.Resources.Inventory.Dtos
{
    public class GetInventoryListDto : PagedAndSortedResultRequestDto
    {
        public InventoryItemKind? Kind { get; set; }
        public InventoryStatus? Status { get; set; }
        public string Location { get; set; }
        public bool? IsAvailable { get; set; }
        public bool? IsOverdue { get; set; }
        public string SearchTerm { get; set; }
    }
}
