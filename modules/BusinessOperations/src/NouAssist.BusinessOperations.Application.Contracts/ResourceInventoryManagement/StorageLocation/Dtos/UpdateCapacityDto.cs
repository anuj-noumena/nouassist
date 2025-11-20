using System.ComponentModel.DataAnnotations;

namespace NouAssist.BusinessOperations.ResourceInventoryManagement.StorageLocation.Dtos
{
    /// <summary>
    /// DTO for updating location capacity
    /// </summary>
    public class UpdateCapacityDto
    {
        /// <summary>
        /// Maximum capacity of the location
        /// </summary>
        [Required]
        [Range(0, double.MaxValue)]
        public decimal MaxCapacity { get; set; }

        /// <summary>
        /// Unit of measure for the capacity
        /// </summary>
        [Required]
        [StringLength(50)]
        public string CapacityUnitOfMeasure { get; set; } = null!;
    }
}
