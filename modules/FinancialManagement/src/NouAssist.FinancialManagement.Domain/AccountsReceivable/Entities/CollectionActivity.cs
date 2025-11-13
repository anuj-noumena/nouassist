using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace NouAssist.FinancialManagement.AccountsReceivable.Entities
{
    public class CollectionActivity : Entity
    {
        public Guid CaseId { get; private set; }
        public string ActivityType { get; private set; }
        public string Description { get; private set; }
        public DateTime ActivityDate { get; private set; }
        public Guid? ContactedPerson { get; private set; }
        public DateTime? FollowUpDate { get; private set; }
        public Guid LoggedBy { get; private set; }

        protected CollectionActivity() { }

        public CollectionActivity(
            Guid caseId,
            string activityType,
            string description,
            DateTime activityDate,
            Guid loggedBy,
            Guid? contactedPerson = null,
            DateTime? followUpDate = null)
        {
            CaseId = caseId;
            ActivityType = Check.NotNullOrWhiteSpace(activityType, nameof(activityType), 50);
            Description = Check.NotNullOrWhiteSpace(description, nameof(description), 1000);
            ActivityDate = activityDate;
            ContactedPerson = contactedPerson;
            FollowUpDate = followUpDate;
            LoggedBy = loggedBy;
        }

        public override object[] GetKeys()
        {
            return new object[] { CaseId, ActivityDate, LoggedBy };
        }
    }
}
