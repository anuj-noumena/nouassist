namespace NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.ValueObjects
{
    /// <summary>
    /// Value object representing content of an investor update
    /// </summary>
    public class UpdateContent : ValueObject
    {
        public string Content { get; private set; }
        public string ContentType { get; private set; }
        public Dictionary<string, object> Metadata { get; private set; }

        private UpdateContent() 
        {
            Metadata = new Dictionary<string, object>();
        }

        public UpdateContent(string content, string contentType = "text/html", Dictionary<string, object> metadata = null)
        {
            Content = Check.NotNullOrWhiteSpace(
                content,
                nameof(content),
                maxLength: IRMConsts.InvestorUpdate.MaxContentLength
            );
            ContentType = contentType ?? "text/html";
            Metadata = metadata ?? new Dictionary<string, object>();
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Content;
            yield return ContentType;
            foreach (var kvp in Metadata.OrderBy(m => m.Key))
            {
                yield return kvp.Key;
                yield return kvp.Value;
            }
        }
    }
}
