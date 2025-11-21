using System;
using System.Collections.Generic;
using System.Linq;
using NouAssist.StartupEntrepreneurship.LegalCompliance.Constants;
using Volo.Abp;
using Volo.Abp.Domain.Values;

namespace NouAssist.StartupEntrepreneurship.LegalCompliance.ValueObjects
{
    /// <summary>
    /// Value object representing the content of a legal document template
    /// </summary>
    public class TemplateContent : ValueObject
    {
        public string Content { get; private set; }
        public string Format { get; private set; }
        public Dictionary<string, string> Placeholders { get; private set; }
        public DateTime LastUpdated { get; private set; }

        protected TemplateContent() 
        {
            Placeholders = new Dictionary<string, string>();
        }

        public TemplateContent(string content, string format, Dictionary<string, string> placeholders, DateTime lastUpdated)
        {
            Content = Check.NotNullOrWhiteSpace(
                content,
                nameof(content),
                maxLength: LegalComplianceConsts.MaxTemplateContentLength
            );
            Format = Check.NotNullOrWhiteSpace(format, nameof(format));
            Placeholders = placeholders ?? new Dictionary<string, string>();
            LastUpdated = lastUpdated;
        }

        public string ApplyPlaceholders(Dictionary<string, string> values)
        {
            string result = Content;
            foreach (var placeholder in Placeholders.Keys)
            {
                if (values.ContainsKey(placeholder))
                {
                    result = result.Replace($"{{{{{placeholder}}}}}", values[placeholder]);
                }
            }
            return result;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Content;
            yield return Format;
            yield return LastUpdated;
            
            foreach (var placeholder in Placeholders.OrderBy(x => x.Key))
            {
                yield return $"{placeholder.Key}:{placeholder.Value}";
            }
        }
    }
}
