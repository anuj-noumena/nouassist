namespace NouAssist.RequirementsManagement.Elicitation.Entities
{
    /// <summary>
    /// Entity representing a participant in an elicitation session
    /// </summary>
    public class ElicitationSessionParticipant : Entity
    {
        public Guid SessionId { get; private set; }
        public Guid StakeholderId { get; private set; }
        public bool IsRequired { get; private set; }
        public bool HasAttended { get; private set; }
        public DateTime? AttendedAt { get; private set; }

        protected ElicitationSessionParticipant() { }

        internal ElicitationSessionParticipant(Guid sessionId, Guid stakeholderId, bool isRequired)
        {
            SessionId = sessionId;
            StakeholderId = stakeholderId;
            IsRequired = isRequired;
            HasAttended = false;
        }

        public void MarkAsAttended(DateTime attendedAt)
        {
            HasAttended = true;
            AttendedAt = attendedAt;
        }

        public override object[] GetKeys()
        {
            return new object[] { SessionId, StakeholderId };
        }
    }
}
