namespace NouAssist.StartupEntrepreneurship.GrowthExperimentation.ValueObjects
{
    /// <summary>
    /// Value object that represents detailed information about a marketing channel
    /// </summary>
    public class ChannelDetails : ValueObject
    {
        /// <summary>
        /// Platform or provider of the channel (e.g., Google, Facebook)
        /// </summary>
        public string Platform { get; private set; }
        
        /// <summary>
        /// Account identifier for the channel
        /// </summary>
        public string AccountId { get; private set; }
        
        /// <summary>
        /// URL or endpoint for the channel
        /// </summary>
        public string Url { get; private set; }
        
        /// <summary>
        /// Additional settings or configuration in JSON format
        /// </summary>
        public string Settings { get; private set; }
        
        protected ChannelDetails() { }
        
        public ChannelDetails(string platform, string accountId, string url, string settings = null)
        {
            Platform = Check.NotNullOrWhiteSpace(platform, nameof(platform), maxLength: 100);
            AccountId = accountId; // Can be null for some channels
            Url = url; // Can be null
            Settings = settings; // Can be null
        }
        
        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Platform;
            
            if (!string.IsNullOrEmpty(AccountId))
            {
                yield return AccountId;
            }
            
            if (!string.IsNullOrEmpty(Url))
            {
                yield return Url;
            }
            
            if (!string.IsNullOrEmpty(Settings))
            {
                yield return Settings;
            }
        }
    }
}
