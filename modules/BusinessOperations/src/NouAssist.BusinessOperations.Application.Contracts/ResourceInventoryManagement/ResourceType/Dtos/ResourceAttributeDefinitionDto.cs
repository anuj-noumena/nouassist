using NouAssist.BusinessOperations.ResourceInventoryManagement.Enums;

namespace NouAssist.BusinessOperations.ResourceInventoryManagement.ResourceType.Dtos
{
    /// <summary>
    /// DTO for resource attribute definition
    /// </summary>
    public class ResourceAttributeDefinitionDto
    {
        /// <summary>
        /// Name of the attribute
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// Description of the attribute
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Data type of the attribute
        /// </summary>
        public AttributeDataType DataType { get; set; }

        /// <summary>
        /// Whether the attribute is required
        /// </summary>
        public bool IsRequired { get; set; }

        /// <summary>
        /// Default value for the attribute (if any)
        /// </summary>
        public string? DefaultValue { get; set; }

        /// <summary>
        /// Possible values for enum type attributes (comma-separated)
        /// </summary>
        public string? PossibleValues { get; set; }

        /// <summary>
        /// Display order for UI presentation
        /// </summary>
        public int DisplayOrder { get; set; }
    }
}
