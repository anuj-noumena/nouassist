using NouAssist.BusinessOperations.ResourceInventoryManagement.Enums;

namespace NouAssist.BusinessOperations.ResourceInventoryManagement.ResourceInstance.Dtos
{
    /// <summary>
    /// DTO for resource attribute value
    /// </summary>
    public class ResourceAttributeValueDto
    {
        /// <summary>
        /// The name of the attribute
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// The data type of the attribute
        /// </summary>
        public AttributeDataType DataType { get; set; }

        /// <summary>
        /// The string representation of the value
        /// </summary>
        public string? Value { get; set; }
    }
}
