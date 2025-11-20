using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NouAssist.IntelligenceStrategy.WorkflowEngine.EntityFrameworkCore
{
    /// <summary>
    /// Extensions for configuring JSON conversion of properties
    /// </summary>
    public static class JsonConversionExtensions
    {
        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        /// <summary>
        /// Configures a property to be stored as JSON
        /// </summary>
        /// <typeparam name="TProperty">The property type</typeparam>
        /// <param name="propertyBuilder">The property builder</param>
        /// <returns>The property builder for chaining</returns>
        public static PropertyBuilder<TProperty> HasJsonConversion<TProperty>(
            this PropertyBuilder<TProperty> propertyBuilder)
        {
            var converter = new ValueConverter<TProperty, string>(
                v => JsonSerializer.Serialize(v, JsonOptions),
                v => v == null ? default! : JsonSerializer.Deserialize<TProperty>(v, JsonOptions)!);

            var comparer = new ValueComparer<TProperty>(
                (a, b) => JsonSerializer.Serialize(a, JsonOptions) == JsonSerializer.Serialize(b, JsonOptions),
                v => v == null ? 0 : JsonSerializer.Serialize(v, JsonOptions).GetHashCode(),
                v => v == null ? default! : JsonSerializer.Deserialize<TProperty>(JsonSerializer.Serialize(v, JsonOptions), JsonOptions)!);

            propertyBuilder.HasConversion(converter);
            propertyBuilder.Metadata.SetValueConverter(converter);
            propertyBuilder.Metadata.SetValueComparer(comparer);

            return propertyBuilder;
        }
    }
}
