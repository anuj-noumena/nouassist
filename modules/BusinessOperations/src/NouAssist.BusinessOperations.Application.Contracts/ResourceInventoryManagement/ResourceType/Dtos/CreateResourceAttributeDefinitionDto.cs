using System.ComponentModel.DataAnnotations;
using NouAssist.BusinessOperations.ResourceInventoryManagement.Constants;
using NouAssist.BusinessOperations.ResourceInventoryManagement.Enums;

namespace NouAssist.BusinessOperations.ResourceInventoryManagement.ResourceType.Dtos
{
    /// <summary>
    /// DTO for creating a new resource attribute definition
    /// </summary>
    public class CreateResourceAttributeDefinitionDto
    {
        /// <summary>
        /// Name of the attribute
        /// </summary>
        [Required]
        [StringLength(RIMConstants.Attributes.MaxNameLength)]
        public string Name { get; set; } = null!;

        /// <summary>
        /// Description of the attribute
        /// </summary>
        [StringLength(RIMConstants.Attributes.MaxDescriptionLength)]
        public string? Description { get; set; }

        /// <summary>
        /// Data type of the attribute
        /// </summary>
        [Required]
        public AttributeDataType DataType { get; set; }

        /// <summary>
        /// Whether the attribute is required
        /// </summary>
        public bool IsRequired { get; set; }

        /// <summary>
        /// Default value for the attribute (if any)
        /// </summary>
        [StringLength(RIMConstants.Attributes.MaxValueLength)]
        public string? DefaultValue { get; set; }

        /// <summary>
        /// Possible values for enum type attributes (comma-separated or list)
        /// </summary>
        public List<string>? PossibleValues { get; set; }

        /// <summary>
        /// Display order for UI presentation
        /// </summary>
        public int DisplayOrder { get; set; }
    }
}
