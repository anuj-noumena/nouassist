namespace NouAssist.RequirementsManagement.AnalysisPrioritization.Entities
{
    /// <summary>
    /// Represents a stakeholder vote in a prioritization activity
    /// </summary>
    public class StakeholderVote : Entity<Guid>
    {
        public Guid PrioritizationActivityId { get; private set; }
        public Guid RequirementId { get; private set; }
        public Guid StakeholderId { get; private set; }
        public VoteValue Vote { get; private set; }
        public string? Comment { get; private set; }
        public DateTime VotedAt { get; private set; }

        protected StakeholderVote() { }

        internal StakeholderVote(
            Guid id,
            Guid prioritizationActivityId,
            Guid requirementId,
            Guid stakeholderId,
            VoteValue vote,
            string? comment,
            DateTime votedAt)
            : base(id)
        {
            PrioritizationActivityId = prioritizationActivityId;
            RequirementId = requirementId;
            StakeholderId = stakeholderId;
            Vote = Check.NotNull(vote, nameof(vote));
            Comment = comment;
            VotedAt = votedAt;
        }

        internal void UpdateVote(VoteValue newVote, string? newComment)
        {
            Vote = Check.NotNull(newVote, nameof(newVote));
            Comment = newComment;
        }
    }
}
