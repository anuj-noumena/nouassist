using System.Collections.Generic;
using System.Linq;
using Volo.Abp.Domain.Services;

namespace NouAssist.IdentityAccess.Users.Services
{
    public class UserPreferenceService : DomainService
    {
        public Dictionary<string, object> GetDefaultPreferences()
        {
            return new Dictionary<string, object>
            {
                ["theme"] = "system",
                ["language"] = "en",
                ["timezone"] = "UTC",
                ["dateFormat"] = "yyyy-MM-dd",
                ["timeFormat"] = "HH:mm",
                ["emailNotifications"] = true,
                ["pushNotifications"] = true,
                ["weekStartsOn"] = "monday"
            };
        }

        public Dictionary<string, object> MergePreferences(
            Dictionary<string, object> defaults,
            Dictionary<string, object> userPreferences)
        {
            var merged = new Dictionary<string, object>(defaults);

            foreach (var pref in userPreferences)
            {
                merged[pref.Key] = pref.Value;
            }

            return merged;
        }

        public bool ValidatePreference(string key, object value)
        {
            // Add validation logic for specific preferences
            switch (key.ToLower())
            {
                case "theme":
                    var theme = value?.ToString()?.ToLower();
                    return theme == "light" || theme == "dark" || theme == "system";

                case "language":
                    var lang = value?.ToString()?.ToLower();
                    var supportedLanguages = new[] { "en", "es", "fr", "de", "ja", "zh" };
                    return supportedLanguages.Contains(lang);

                case "weekstartson":
                    var day = value?.ToString()?.ToLower();
                    return day == "sunday" || day == "monday";

                default:
                    return true; // Allow unknown preferences
            }
        }
    }
}
