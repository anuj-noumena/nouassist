namespace NouAssist.BusinessOperations.ResourceInventoryManagement.Enums
{
    /// <summary>
    /// Defines the data type of a resource attribute
    /// </summary>
    public enum AttributeDataType
    {
        /// <summary>
        /// Text attribute
        /// </summary>
        Text = 1,

        /// <summary>
        /// Numeric attribute
        /// </summary>
        Number = 2,

        /// <summary>
        /// Boolean attribute
        /// </summary>
        Boolean = 3,

        /// <summary>
        /// Date attribute
        /// </summary>
        Date = 4,

        /// <summary>
        /// Date with time attribute
        /// </summary>
        DateTime = 5,

        /// <summary>
        /// Enumeration (selection from fixed list)
        /// </summary>
        Enum = 6,

        /// <summary>
        /// JSON data attribute
        /// </summary>
        Json = 7
    }
}
