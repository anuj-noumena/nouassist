using System.Collections.Generic;
using NouAssist.RequirementsManagement.Elicitation.Constants;
using Volo.Abp;
using Volo.Abp.Domain.Values;

namespace NouAssist.RequirementsManagement.Elicitation.ValueObjects
{
    /// <summary>
    /// Value object for tagging brainstorming ideas
    /// </summary>
    public class IdeaTag : ValueObject
    {
        public string Name { get; private set; }
        public string Color { get; private set; }
        public string Description { get; private set; }

        private IdeaTag() { }

        public IdeaTag(string name, string color = "#007ACC", string description = null)
        {
            SetName(name);
            SetColor(color);
            Description = description ?? string.Empty;
        }

        private void SetName(string name)
        {
            Name = Check.NotNullOrWhiteSpace(
                name,
                nameof(name),
                maxLength: ElicitationConsts.MaxTagNameLength
            );
        }

        private void SetColor(string color)
        {
            if (string.IsNullOrWhiteSpace(color))
            {
                color = "#007ACC"; // Default blue color
            }

            if (!color.StartsWith("#") || color.Length != 7)
            {
                throw new BusinessException("Elicitation:Validation:InvalidValue")
                    .WithData("propertyName", "Color");
            }

            Color = color;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Name;
            yield return Color;
            yield return Description ?? string.Empty;
        }
    }
}
