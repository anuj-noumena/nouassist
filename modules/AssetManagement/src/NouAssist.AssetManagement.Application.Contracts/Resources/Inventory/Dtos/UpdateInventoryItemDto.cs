using System.ComponentModel.DataAnnotations;
using NouAssist.Resources.Enums;

namespace NouAssist.Resources.Inventory.Dtos
{
    public class UpdateInventoryItemDto
    {
        [Required]
        [MaxLength(200)]
        public string Name { get; set; }

        public string Description { get; set; }
        public string Manufacturer { get; set; }
        public string Model { get; set; }
        public InventoryCondition Condition { get; set; }
        public string Location { get; set; }
        public string Building { get; set; }
        public string Zone { get; set; }
        public decimal? CurrentValue { get; set; }
    }
}
