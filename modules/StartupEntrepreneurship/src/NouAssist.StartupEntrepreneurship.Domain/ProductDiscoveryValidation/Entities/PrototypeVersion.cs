namespace NouAssist.StartupEntrepreneurship.ProductDiscoveryValidation.Entities
{
    /// <summary>
    /// Entity representing a version of a solution prototype
    /// </summary>
    public class PrototypeVersion : Entity<Guid>
    {
        public Guid PrototypeId { get; private set; }
        public string VersionNumber { get; private set; }
        public string Description { get; private set; }
        public PrototypeLink Link { get; private set; }
        public DateTime CreatedDate { get; private set; }
        public string CreatedBy { get; private set; }
        public string Notes { get; private set; }
        public bool IsActive { get; private set; }

        protected PrototypeVersion() { }

        internal PrototypeVersion(
            Guid id,
            Guid prototypeId,
            string versionNumber,
            string description,
            PrototypeLink link,
            DateTime createdDate,
            string createdBy) : base(id)
        {
            PrototypeId = prototypeId;
            SetVersionNumber(versionNumber);
            SetDescription(description);
            Link = link ?? throw new ArgumentNullException(nameof(link));
            CreatedDate = createdDate;
            CreatedBy = Check.NotNullOrWhiteSpace(createdBy, nameof(createdBy));
            IsActive = true;
        }

        internal void UpdateDescription(string description)
        {
            SetDescription(description);
        }

        internal void UpdateLink(PrototypeLink link)
        {
            Link = link ?? throw new ArgumentNullException(nameof(link));
        }

        internal void UpdateNotes(string notes)
        {
            Notes = notes;
        }

        internal void Deactivate()
        {
            IsActive = false;
        }

        internal void Activate()
        {
            IsActive = true;
        }

        private void SetVersionNumber(string versionNumber)
        {
            VersionNumber = Check.NotNullOrWhiteSpace(
                versionNumber,
                nameof(versionNumber),
                maxLength: 20
            );
        }

        private void SetDescription(string description)
        {
            Description = Check.NotNullOrWhiteSpace(
                description,
                nameof(description),
                maxLength: ProductDiscoveryConsts.MaxVersionNotesLength
            );
        }
    }
}
