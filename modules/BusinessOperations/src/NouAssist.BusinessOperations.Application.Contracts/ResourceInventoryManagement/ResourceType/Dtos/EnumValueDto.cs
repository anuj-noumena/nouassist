namespace NouAssist.BusinessOperations.ResourceInventoryManagement.ResourceType.Dtos
{
    /// <summary>
    /// DTO for representing enum values
    /// </summary>
    public class EnumValueDto
    {
        /// <summary>
        /// Enum value
        /// </summary>
        public int Value { get; set; }
        
        /// <summary>
        /// Enum value name
        /// </summary>
        public string Name { get; set; } = null!;
        
        /// <summary>
        /// Localized display name
        /// </summary>
        public string DisplayName { get; set; } = null!;
    }
}
