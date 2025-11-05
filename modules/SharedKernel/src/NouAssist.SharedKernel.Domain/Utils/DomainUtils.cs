using System;
using System.Security.Cryptography;
using System.Text;

namespace NouAssist.SharedKernel.Utils
{
    public static class DomainUtils
    {
        /// <summary>
        /// Convert entity type string to deterministic Guid for composite keys
        /// </summary>
        public static Guid ConvertEntityTypeToGuid(string entityType)
        {
            if (string.IsNullOrWhiteSpace(entityType))
                throw new ArgumentException("Entity type cannot be null or empty", nameof(entityType));

            // Create deterministic Guid from string
            using var md5 = MD5.Create();
            var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(entityType.ToLowerInvariant()));
            return new Guid(hash);
        }

        /// <summary>
        /// Get entity type from Guid
        /// Note: This is a reverse lookup - you'd need a registry for production use
        /// </summary>
        public static string GetEntityTypeFromGuid(Guid guid)
        {
            // In production, maintain a registry/dictionary
            // For now, return placeholder
            return "Unknown";
        }
    }
}
