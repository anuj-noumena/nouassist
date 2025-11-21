using NouAssist.StartupEntrepreneurship.FundraisingPipeline.Aggregates;
using NouAssist.StartupEntrepreneurship.FundraisingPipeline.Enums;

namespace NouAssist.StartupEntrepreneurship.FundraisingPipeline.Policies.impl
{
    /// <summary>
    /// Strict implementation of prospect transition policy with additional business rules
    /// </summary>
    public class StrictProspectTransitionPolicy : IProspectTransitionPolicy
    {
        public PolicyResult CanTransition(Prospect prospect, ProspectStatus targetStatus)
        {
            var currentStatus = prospect.PipelineStatus.Status;

            // Same status is always allowed
            if (currentStatus == targetStatus)
            {
                return PolicyResult.Allow();
            }

            // Cannot move out of terminal states
            if (currentStatus == ProspectStatus.Committed)
            {
                return PolicyResult.Deny("Cannot change status of a committed prospect");
            }

            if (currentStatus == ProspectStatus.Disqualified)
            {
                return PolicyResult.Deny("Cannot change status of a disqualified prospect");
            }

            if (currentStatus == ProspectStatus.Declined)
            {
                return PolicyResult.Deny("Cannot change status of a declined prospect");
            }

            // Must have contact info before moving to Contacted
            if (targetStatus == ProspectStatus.Contacted)
            {
                if (string.IsNullOrWhiteSpace(prospect.ContactInfo.Email) &&
                    string.IsNullOrWhiteSpace(prospect.ContactInfo.Phone))
                {
                    return PolicyResult.Deny("Prospect must have email or phone before being contacted");
                }
            }

            // Must have score before moving to DueDiligence
            if (targetStatus == ProspectStatus.DueDiligence)
            {
                if (prospect.Score == null || prospect.Score.OverallScore < 50)
                {
                    return PolicyResult.Deny("Prospect must have a score of at least 50 before due diligence");
                }
            }

            // Must be assigned to a round before committing
            if (targetStatus == ProspectStatus.Committed)
            {
                if (!prospect.FundraisingRoundId.HasValue)
                {
                    return PolicyResult.Deny("Prospect must be assigned to a fundraising round before committing");
                }
            }

            // Define valid sequential transitions
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
