namespace NouAssist.StartupEntrepreneurship.FundraisingPipeline.ValueObjects
{
    /// <summary>
    /// Value object representing a term sheet clause text with formatting
    /// </summary>
    public class ClauseText : ValueObject
    {
        public string Content { get; private set; }
        public bool IsNegotiable { get; private set; }
        public string Category { get; private set; } // Economic, Control, etc.
        public string FormattedHtml { get; private set; } // Optional HTML formatted version
        
        private ClauseText() { }
        
        public ClauseText(string content, bool isNegotiable, string category, string formattedHtml = null)
        {
            Content = content ?? throw new ArgumentNullException(nameof(content));
            IsNegotiable = isNegotiable;
            Category = category ?? throw new ArgumentNullException(nameof(category));
            FormattedHtml = formattedHtml;
        }
        
        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Content;
            yield return IsNegotiable;
            yield return Category;
            yield return FormattedHtml ?? string.Empty;
        }
    }
}
