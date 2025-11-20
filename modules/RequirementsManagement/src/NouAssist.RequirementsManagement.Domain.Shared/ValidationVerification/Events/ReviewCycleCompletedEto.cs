namespace NouAssist.RequirementsManagement.ValidationVerification.Events
{
    /// <summary>
    /// Event published when a review cycle is completed
    /// </summary>
    [EventName("NouAssist.RequirementsManagement.ValidationVerification.ReviewCycleCompleted")]
    public class ReviewCycleCompletedEto : ValidationVerificationEtoBase
    {
        public string Name { get; set; }
        public Guid RequirementId { get; set; }
        public ReviewCycleStatus FinalStatus { get; set; }
        public int TotalReviewers { get; set; }
        public int ApprovedCount { get; set; }
        public int RejectedCount { get; set; }
        public int ChangesRequestedCount { get; set; }
        public DateTime CompletedAt { get; set; }
    }
}
