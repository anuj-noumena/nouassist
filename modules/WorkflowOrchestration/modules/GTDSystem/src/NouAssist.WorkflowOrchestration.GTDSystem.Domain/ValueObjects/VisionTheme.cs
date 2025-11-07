using System;
using System.Collections.Generic;
using System.Linq;
using Volo.Abp;
using Volo.Abp.Domain.Values;

namespace NouAssist.WorkflowOrchestration.GTDSystem.ValueObjects
{
    public class VisionTheme : ValueObject
    {
        public string Theme { get; private set; }
        public string? Description { get; private set; }
        public List<string> KeyWords { get; private set; }
        public string? Color { get; private set; }

        private VisionTheme() 
        {
            KeyWords = new List<string>();
        }

        public VisionTheme(string theme, string? description = null, List<string>? keyWords = null)
        {
            Theme = Check.NotNullOrWhiteSpace(theme, nameof(theme), maxLength: 100);
            Description = description;
            KeyWords = keyWords ?? new List<string>();
        }

        public bool HasKeyWords => KeyWords.Any();

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Theme;
            yield return Description ?? string.Empty;
            yield return string.Join(",", KeyWords);
        }
    }
}
