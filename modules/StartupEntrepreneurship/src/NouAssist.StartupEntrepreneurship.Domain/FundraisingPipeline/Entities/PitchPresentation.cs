namespace NouAssist.StartupEntrepreneurship.FundraisingPipeline.Entities
{
    /// <summary>
    /// Entity representing a pitch presentation event
    /// </summary>
    public class PitchPresentation : Entity<Guid>, IHasCreationTime
    {
        public Guid PitchDeckId { get; private set; }
        public Guid VersionId { get; private set; }
        public Guid ProspectId { get; private set; }
        public PresentationDetails Details { get; private set; }
        public DateTime CreationTime { get; set; }
        
        protected PitchPresentation() { }
        
        internal PitchPresentation(
            Guid id,
            Guid pitchDeckId,
            Guid versionId,
            Guid prospectId,
            PresentationDetails details) : base(id)
        {
            PitchDeckId = pitchDeckId;
            VersionId = versionId;
            ProspectId = prospectId;
            Details = details ?? throw new ArgumentNullException(nameof(details));
            CreationTime = DateTime.Now;
        }
        
        public void Update(
            Guid versionId,
            Guid prospectId,
            PresentationDetails details)
        {
            VersionId = versionId;
            ProspectId = prospectId;
            Details = details ?? throw new ArgumentNullException(nameof(details));
        }
    }
}
