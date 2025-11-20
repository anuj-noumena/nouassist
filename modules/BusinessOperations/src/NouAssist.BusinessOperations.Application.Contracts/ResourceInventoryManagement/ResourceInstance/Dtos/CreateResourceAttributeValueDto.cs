using System.ComponentModel.DataAnnotations;
using NouAssist.BusinessOperations.ResourceInventoryManagement.Constants;

namespace NouAssist.BusinessOperations.ResourceInventoryManagement.ResourceInstance.Dtos
{
    /// <summary>
    /// DTO for creating a resource attribute value
    /// </summary>
    public class CreateResourceAttributeValueDto
    {
        /// <summary>
        /// The name of the attribute
        /// </summary>
        [Required]
        [StringLength(RIMConstants.Attributes.MaxNameLength)]
        public string Name { get; set; } = null!;

        /// <summary>
        /// The value of the attribute
        /// </summary>
        [StringLength(RIMConstants.Attributes.MaxValueLength)]
        public string? Value { get; set; }
    }
}
