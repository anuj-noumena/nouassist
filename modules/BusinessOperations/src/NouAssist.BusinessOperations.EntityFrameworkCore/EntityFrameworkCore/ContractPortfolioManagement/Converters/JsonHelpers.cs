using System;
using System.Collections.Generic;
using System.Text.Json;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace NouAssist.ContractPortfolioManagement.EntityFrameworkCore.Converters
{
    /// <summary>
    /// Provides Json conversion for collection types
    /// </summary>
    public static class JsonHelpers
    {
        private static readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = false
        };

        /// <summary>
        /// Creates a value converter for a collection property to store as JSON
        /// </summary>
        public static ValueConverter<List<T>, string> GetJsonValueConverter<T>() 
            where T : class
        {
            return new ValueConverter<List<T>, string>(
                v => JsonSerializer.Serialize(v, _jsonOptions),
                v => v == null ? new List<T>() : JsonSerializer.Deserialize<List<T>>(v, _jsonOptions));
        }

        /// <summary>
        /// Creates a value comparer for a collection property stored as JSON
        /// </summary>
        public static ValueComparer<List<T>> GetJsonValueComparer<T>() 
            where T : class
        {
            return new ValueComparer<List<T>>(
                (c1, c2) => JsonSerializer.Serialize(c1, _jsonOptions) == JsonSerializer.Serialize(c2, _jsonOptions),
                c => c == null ? 0 : JsonSerializer.Serialize(c, _jsonOptions).GetHashCode(),
                c => JsonSerializer.Deserialize<List<T>>(JsonSerializer.Serialize(c, _jsonOptions), _jsonOptions));
        }
    }
}
