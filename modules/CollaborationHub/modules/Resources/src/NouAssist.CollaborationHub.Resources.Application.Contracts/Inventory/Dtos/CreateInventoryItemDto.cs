using System.ComponentModel.DataAnnotations;
using NouAssist.CollaborationHub.Resources.Enums;

namespace NouAssist.CollaborationHub.Resources.Inventory.Dtos
{
    public class CreateInventoryItemDto
    {
        [Required]
        public InventoryItemKind Kind { get; set; }

        [Required]
        [MaxLength(200)]
        public string Name { get; set; }

        public string Description { get; set; }
        public string Manufacturer { get; set; }
        public string Model { get; set; }
        public string Barcode { get; set; }
        public string SerialNumber { get; set; }
        public string Sku { get; set; }

        [Range(1, 10000)]
        public int Quantity { get; set; } = 1;

        public InventoryCondition Condition { get; set; } = InventoryCondition.Good;
        public string Location { get; set; }
        public string Building { get; set; }
        public string Zone { get; set; }
        public decimal? PurchasePrice { get; set; }
        public DateTime? PurchaseDate { get; set; }
    }
}
