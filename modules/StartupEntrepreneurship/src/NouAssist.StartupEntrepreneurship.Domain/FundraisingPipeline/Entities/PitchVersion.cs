namespace NouAssist.StartupEntrepreneurship.FundraisingPipeline.Entities
{
    /// <summary>
    /// Entity representing a version of a pitch deck
    /// </summary>
    public class PitchVersion : Entity<Guid>, IHasCreationTime
    {
        public Guid PitchDeckId { get; private set; }
        public string VersionName { get; private set; }
        public int VersionNumber { get; private set; }
        public DeckContent Content { get; private set; }
        public string ChangeNotes { get; private set; }
        public DateTime CreationTime { get; set; }
        
        protected PitchVersion() { }
        
        internal PitchVersion(
            Guid id,
            Guid pitchDeckId,
            string versionName,
            int versionNumber,
            DeckContent content,
            string changeNotes = null) : base(id)
        {
            PitchDeckId = pitchDeckId;
            SetVersionName(versionName);
            VersionNumber = versionNumber;
            Content = content ?? throw new ArgumentNullException(nameof(content));
            ChangeNotes = changeNotes;
            CreationTime = DateTime.Now;
        }
        
        public void Update(
            string versionName,
            DeckContent content,
            string changeNotes = null)
        {
            SetVersionName(versionName);
            Content = content ?? throw new ArgumentNullException(nameof(content));
            ChangeNotes = changeNotes;
        }
        
        private void SetVersionName(string versionName)
        {
            if (string.IsNullOrWhiteSpace(versionName))
            {
                throw new ArgumentException("Version name cannot be null or empty.");
            }
            
            VersionName = versionName;
        }
    }
}
