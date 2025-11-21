using System.Collections.Generic;
using NouAssist.StartupEntrepreneurship.ProductDiscoveryValidation.Constants;
using Volo.Abp;
using Volo.Abp.Domain.Values;

namespace NouAssist.StartupEntrepreneurship.ProductDiscoveryValidation.ValueObjects
{
    /// <summary>
    /// Value object representing a categorization tag for feedback
    /// </summary>
    public class FeedbackTag : ValueObject
    {
        public string Name { get; private set; }
        public string Color { get; private set; }
        public string Category { get; private set; }

        private FeedbackTag() { }

        public FeedbackTag(string name, string color = "#2196F3", string category = "General")
        {
            SetName(name);
            SetColor(color);
            SetCategory(category);
        }

        private void SetName(string name)
        {
            Name = Check.NotNullOrWhiteSpace(
                name,
                nameof(name),
                maxLength: ProductDiscoveryConsts.MaxTagLength
            );
        }

        private void SetColor(string color)
        {
            Color = Check.NotNullOrWhiteSpace(color, nameof(color));
            
            // Basic validation for hex color format
            if (!color.StartsWith("#") || color.Length != 7)
            {
                throw new BusinessException("ProductDiscovery:Validation:InvalidValue", "Color must be in hex format (#RRGGBB)");
            }
        }

        private void SetCategory(string category)
        {
            Category = Check.NotNullOrWhiteSpace(
                category,
                nameof(category),
                maxLength: ProductDiscoveryConsts.MaxTagLength
            );
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Name;
            yield return Color;
            yield return Category;
        }
    }
}
