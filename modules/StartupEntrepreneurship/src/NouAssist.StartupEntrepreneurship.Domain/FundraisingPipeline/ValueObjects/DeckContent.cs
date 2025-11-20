namespace NouAssist.StartupEntrepreneurship.FundraisingPipeline.ValueObjects
{
    /// <summary>
    /// Value object representing the content structure of a pitch deck
    /// </summary>
    public class DeckContent : ValueObject
    {
        public string StorageUrl { get; private set; }
        public string ThumbnailUrl { get; private set; }
        public int SlideCount { get; private set; }
        public string ContentHash { get; private set; } // For tracking changes
        public List<string> SlideTopics { get; private set; }
        
        private DeckContent() 
        {
            SlideTopics = new List<string>();
        }
        
        public DeckContent(string storageUrl, string thumbnailUrl, int slideCount, string contentHash, List<string> slideTopics)
        {
            StorageUrl = storageUrl ?? throw new ArgumentNullException(nameof(storageUrl));
            ThumbnailUrl = thumbnailUrl;
            SlideCount = slideCount > 0 ? slideCount : throw new ArgumentException("Slide count must be greater than zero.");
            ContentHash = contentHash ?? throw new ArgumentNullException(nameof(contentHash));
            SlideTopics = slideTopics ?? new List<string>();
        }
        
        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return StorageUrl;
            yield return ThumbnailUrl ?? string.Empty;
            yield return SlideCount;
            yield return ContentHash;
            
            foreach (var topic in SlideTopics)
            {
                yield return topic;
            }
        }
    }
}
