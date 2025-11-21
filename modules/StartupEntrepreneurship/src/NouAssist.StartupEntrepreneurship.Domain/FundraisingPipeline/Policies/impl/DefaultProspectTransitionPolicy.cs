using NouAssist.StartupEntrepreneurship.FundraisingPipeline.Aggregates;
using NouAssist.StartupEntrepreneurship.FundraisingPipeline.Enums;

namespace NouAssist.StartupEntrepreneurship.FundraisingPipeline.Policies.impl
{
    /// <summary>
    /// Default implementation of prospect transition policy
    /// </summary>
    public class DefaultProspectTransitionPolicy : IProspectTransitionPolicy
    {
        public PolicyResult CanTransition(Prospect prospect, ProspectStatus targetStatus)
        {
            var currentStatus = prospect.PipelineStatus.Status;

            // Same status is always allowed (for updates)
            if (currentStatus == targetStatus)
            {
                return PolicyResult.Allow();
            }

            // Define valid transitions
            var isValidTransition = (currentStatus, targetStatus) switch
            {
                // From New
                (ProspectStatus.New, ProspectStatus.Researching) => true,
                (ProspectStatus.New, ProspectStatus.Qualified) => true,
                (ProspectStatus.New, ProspectStatus.Disqualified) => true,

                // From Researching
                (ProspectStatus.Researching, ProspectStatus.Qualified) => true,
                (ProspectStatus.Researching, ProspectStatus.Disqualified) => true,

                // From Qualified
                (ProspectStatus.Qualified, ProspectStatus.Contacted) => true,
                (ProspectStatus.Qualified, ProspectStatus.Disqualified) => true,

                // From Contacted
                (ProspectStatus.Contacted, ProspectStatus.InDiscussion) => true,
                (ProspectStatus.Contacted, ProspectStatus.Disqualified) => true,

                // From InDiscussion
                (ProspectStatus.InDiscussion, ProspectStatus.DueDiligence) => true,
                (ProspectStatus.InDiscussion, ProspectStatus.Declined) => true,

                // From DueDiligence
                (ProspectStatus.DueDiligence, ProspectStatus.Negotiating) => true,
                (ProspectStatus.DueDiligence, ProspectStatus.Declined) => true,

                // From Negotiating
                (ProspectStatus.Negotiating, ProspectStatus.Committed) => true,
                (ProspectStatus.Negotiating, ProspectStatus.Declined) => true,

                _ => false
            };

            return isValidTransition
                ? PolicyResult.Allow()
                : PolicyResult.Deny($"Cannot transition from {currentStatus} to {targetStatus}");
        }
    }
}
