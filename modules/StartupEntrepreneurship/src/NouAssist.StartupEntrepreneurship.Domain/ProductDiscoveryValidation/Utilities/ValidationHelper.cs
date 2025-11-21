using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NouAssist.StartupEntrepreneurship.ProductDiscoveryValidation.Utilities
{
    /// <summary>
    /// Helper class for common validation logic
    /// </summary>
    public static class ValidationHelper
    {
        /// <summary>
        /// Validates email format
        /// </summary>
        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                var mailAddress = new System.Net.Mail.MailAddress(email);
                return mailAddress.Address == email;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Validates URL format
        /// </summary>
        public static bool IsValidUrl(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
                return false;

            return Uri.TryCreate(url, UriKind.Absolute, out var uriResult)
                && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        }

        /// <summary>
        /// Validates date is in the future
        /// </summary>
        public static bool IsFutureDate(DateTime date)
        {
            return date > DateTime.UtcNow;
        }

        /// <summary>
        /// Validates date is within reasonable business hours (for scheduling)
        /// </summary>
        public static bool IsBusinessHours(DateTime date)
        {
            var hour = date.Hour;
            var dayOfWeek = date.DayOfWeek;

            return dayOfWeek != DayOfWeek.Saturday 
                && dayOfWeek != DayOfWeek.Sunday 
                && hour >= 8 
                && hour <= 18;
        }

        /// <summary>
        /// Validates that a score is within expected range
        /// </summary>
        public static bool IsValidScore(int score, int min = 1, int max = 5)
        {
            return score >= min && score <= max;
        }

        /// <summary>
        /// Validates that a percentage is within 0-100 range
        /// </summary>
        public static bool IsValidPercentage(decimal percentage)
        {
            return percentage >= 0 && percentage <= 100;
        }

        /// <summary>
        /// Extracts and validates keywords from text
        /// </summary>
        public static List<string> ExtractValidKeywords(string text, int minLength = 3, int maxKeywords = 10)
        {
            if (string.IsNullOrWhiteSpace(text))
                return new List<string>();

            var stopWords = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                "the", "a", "an", "and", "or", "but", "in", "on", "at", "to", "for",
                "of", "with", "by", "is", "are", "was", "were", "be", "been", "have",
                "has", "had", "do", "does", "did", "will", "would", "could", "should"
            };

            return text
                .Split(new char[] { ' ', ',', '.', '!', '?', ';', ':', '\n', '\r', '\t' }, 
                       StringSplitOptions.RemoveEmptyEntries)
                .Where(word => word.Length >= minLength && !stopWords.Contains(word))
                .Select(word => word.ToLowerInvariant())
                .Distinct()
                .Take(maxKeywords)
                .ToList();
        }

        /// <summary>
        /// Generates a safe filename from text
        /// </summary>
        public static string GenerateSafeFilename(string text, int maxLength = 100)
        {
            if (string.IsNullOrWhiteSpace(text))
                return "unnamed";

            var invalidChars = Path.GetInvalidFileNameChars();
            var safeText = string.Join("_", text.Split(invalidChars, StringSplitOptions.RemoveEmptyEntries));
            
            if (safeText.Length > maxLength)
                safeText = safeText.Substring(0, maxLength);

            return string.IsNullOrWhiteSpace(safeText) ? "unnamed" : safeText;
        }
    }
}
