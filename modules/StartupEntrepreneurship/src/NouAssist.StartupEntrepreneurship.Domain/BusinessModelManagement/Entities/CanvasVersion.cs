namespace NouAssist.StartupEntrepreneurship.BusinessModelManagement.Entities
{
    /// <summary>
    /// Represents a version of a business model canvas
    /// </summary>
    public class CanvasVersion : Entity<Guid>
    {
        public Guid CanvasId { get; private set; }
        public string VersionName { get; private set; }
        public int VersionNumber { get; private set; }
        public string Notes { get; private set; }
        public bool IsCurrentVersion { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public Guid CreatedBy { get; private set; }

        protected CanvasVersion() { }

        internal CanvasVersion(
            Guid id,
            Guid canvasId,
            string versionName,
            int versionNumber,
            Guid createdBy,
            DateTime createdAt,
            string notes = null,
            bool isCurrentVersion = false) : base(id)
        {
            CanvasId = canvasId;
            SetVersionName(versionName);
            VersionNumber = versionNumber;
            CreatedBy = createdBy;
            CreatedAt = createdAt;
            Notes = notes;
            IsCurrentVersion = isCurrentVersion;
        }

        internal void SetAsCurrent()
        {
            IsCurrentVersion = true;
        }

        internal void SetAsNotCurrent()
        {
            IsCurrentVersion = false;
        }

        internal void UpdateNotes(string notes)
        {
            Notes = notes;
        }

        private void SetVersionName(string versionName)
        {
            VersionName = Check.NotNullOrWhiteSpace(
                versionName,
                nameof(versionName),
                maxLength: BusinessModelManagementConsts.BusinessModelCanvas.MaxVersionNameLength
            );
        }
    }
}
