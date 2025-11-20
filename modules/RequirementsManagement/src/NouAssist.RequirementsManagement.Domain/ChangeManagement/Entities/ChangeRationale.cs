using Volo.Abp.Domain.Entities;

namespace NouAssist.RequirementsManagement.ChangeManagement.Entities
{
    /// <summary>
    /// Represents the rationale for a change request
    /// </summary>
    public class ChangeRationale : Entity<Guid>
    {
        public Guid ChangeRequestId { get; private set; }
        public string RationaleText { get; private set; }
        public string Category { get; private set; }
        public DateTime CreatedDate { get; private set; }

        protected ChangeRationale() { }

        internal ChangeRationale(
            Guid changeRequestId,
            string rationaleText,
            string category = null)
        {
            Id = Guid.NewGuid();
            ChangeRequestId = Check.NotEmpty(changeRequestId, nameof(changeRequestId));
            SetRationaleText(rationaleText);
            Category = category;
            CreatedDate = DateTime.UtcNow;
        }

        internal void UpdateRationale(string rationaleText, string category = null)
        {
            SetRationaleText(rationaleText);
            Category = category;
        }

        private void SetRationaleText(string rationaleText)
        {
            RationaleText = Check.NotNullOrWhiteSpace(
                rationaleText,
                nameof(rationaleText),
                maxLength: ChangeManagementConsts.MaxRationaleLength
            );
        }
    }
}
