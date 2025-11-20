namespace NouAssist.RequirementsManagement.AnalysisPrioritization.Exceptions
{
    /// <summary>
    /// Exception thrown when trying to vote on an activity that's not in voting status
    /// </summary>
    public class ActivityNotInVotingStatusException : RequirementsAnalysisToolsDomainException
    {
        public ActivityNotInVotingStatusException(Guid activityId, ActivityStatus currentStatus)
            : base("RequirementsAnalysisTools:ErrorMessage:ActivityNotInVotingStatus")
        {
            WithData("activityId", activityId);
            WithData("currentStatus", currentStatus);
        }
    }
}
