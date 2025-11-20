namespace NouAssist.StartupEntrepreneurship.BusinessModelManagement.ValueObjects
{
    /// <summary>
    /// Represents the content of a business model canvas component
    /// </summary>
    public class ComponentContent : ValueObject
    {
        public string Text { get; private set; }
        public List<string> BulletPoints { get; private set; }
        public Dictionary<string, string> AdditionalProperties { get; private set; }

        protected ComponentContent()
        {
            BulletPoints = new List<string>();
            AdditionalProperties = new Dictionary<string, string>();
        }

        public ComponentContent(string text, List<string> bulletPoints = null, Dictionary<string, string> additionalProperties = null)
        {
            SetText(text);
            BulletPoints = bulletPoints ?? new List<string>();
            AdditionalProperties = additionalProperties ?? new Dictionary<string, string>();
        }

        private void SetText(string text)
        {
            Text = Check.NotNull(
                text,
                nameof(text),
                maxLength: BusinessModelManagementConsts.BusinessModelCanvas.MaxComponentContentLength
            );
        }

        public ComponentContent AddBulletPoint(string bulletPoint)
        {
            if (string.IsNullOrWhiteSpace(bulletPoint))
                return this;

            var newBulletPoints = new List<string>(BulletPoints) { bulletPoint };
            return new ComponentContent(Text, newBulletPoints, new Dictionary<string, string>(AdditionalProperties));
        }

        public ComponentContent RemoveBulletPoint(string bulletPoint)
        {
            var newBulletPoints = BulletPoints.Where(bp => bp != bulletPoint).ToList();
            return new ComponentContent(Text, newBulletPoints, new Dictionary<string, string>(AdditionalProperties));
        }

        public ComponentContent AddProperty(string key, string value)
        {
            var newProperties = new Dictionary<string, string>(AdditionalProperties)
            {
                [key] = value
            };
            return new ComponentContent(Text, new List<string>(BulletPoints), newProperties);
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Text;
            foreach (var bulletPoint in BulletPoints.OrderBy(bp => bp))
            {
                yield return bulletPoint;
            }
            foreach (var kvp in AdditionalProperties.OrderBy(kvp => kvp.Key))
            {
                yield return kvp.Key;
                yield return kvp.Value;
            }
        }
    }
}
