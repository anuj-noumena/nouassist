using System.ComponentModel.DataAnnotations;

namespace NouAssist.CollaborationHub.Resources.Inventory.Dtos
{
    public class CheckInInventoryDto
    {
        [Range(1, 1000)]
        public int Quantity { get; set; } = 1;
    }
}
