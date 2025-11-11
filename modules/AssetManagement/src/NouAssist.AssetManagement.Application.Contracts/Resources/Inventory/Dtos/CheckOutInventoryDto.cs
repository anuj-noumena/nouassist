using System;
using System.ComponentModel.DataAnnotations;

namespace NouAssist.AssetManagement.Resources.Inventory.Dtos
{
    public class CheckOutInventoryDto
    {
        [Required]
        public Guid UserId { get; set; }

        [Range(1, 1000)]
        public int Quantity { get; set; } = 1;

        public DateTime? DueDate { get; set; }
    }
}
