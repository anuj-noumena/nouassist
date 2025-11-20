namespace NouAssist.StartupEntrepreneurship.ProductDiscoveryValidation.ValueObjects
{
    /// <summary>
    /// Value object representing a link to a prototype resource
    /// </summary>
    public class PrototypeLink : ValueObject
    {
        public string Url { get; private set; }
        public string Title { get; private set; }
        public string Type { get; private set; } // figma, invision, html, etc.
        public string AccessInstructions { get; private set; }

        private PrototypeLink() { }

        public PrototypeLink(string url, string title, string type, string accessInstructions = null)
        {
            SetUrl(url);
            SetTitle(title);
            SetType(type);
            AccessInstructions = accessInstructions;
        }

        private void SetUrl(string url)
        {
            Url = Check.NotNullOrWhiteSpace(
                url,
                nameof(url),
                maxLength: ProductDiscoveryConsts.MaxUrlLength
            );

            if (!Uri.IsWellFormedUriString(url, UriKind.Absolute))
            {
                throw new BusinessException("ProductDiscovery:Validation:InvalidValue", 
                    "Invalid URL format");
            }
        }

        private void SetTitle(string title)
        {
            Title = Check.NotNullOrWhiteSpace(
                title,
                nameof(title),
                maxLength: 100
            );
        }

        private void SetType(string type)
        {
            Type = Check.NotNullOrWhiteSpace(
                type,
                nameof(type),
                maxLength: 50
            );
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Url;
            yield return Title;
            yield return Type;
            yield return AccessInstructions ?? string.Empty;
        }
    }
}
