using System.Text.Json;
using System.Text.Json.Serialization;

namespace NouAssist.BusinessOperations.ResourceInventoryManagement.EntityFrameworkCore
{
    /// <summary>
    /// Default options for JSON serialization
    /// </summary>
    internal static class JsonSerializerDefaultOptions
    {
        /// <summary>
        /// Default options for JSON serialization
        /// </summary>
        public static JsonSerializerOptions Options { get; } = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = false,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            Converters = { new JsonStringEnumConverter() }
        };
    }
}
