using NouAssist.StartupEntrepreneurship.FundraisingPipeline.Aggregates;
using NouAssist.StartupEntrepreneurship.FundraisingPipeline.Enums;

namespace NouAssist.StartupEntrepreneurship.FundraisingPipeline.Policies.impl
{
    /// <summary>
    /// Default implementation of round closing policy
    /// </summary>
    public class DefaultRoundClosingPolicy : IRoundClosingPolicy
    {
        public PolicyResult CanClose(FundraisingRound round)
        {
            // Can only close from Active or Closing stage
            if (round.Stage != RoundStage.Active && round.Stage != RoundStage.Closing)
            {
                return PolicyResult.Deny($"Cannot close round in {round.Stage} stage");
            }

            // Already closed
            if (round.Stage == RoundStage.Closed)
            {
                return PolicyResult.Deny("Round is already closed");
            }

            return PolicyResult.Allow();
        }
    }
}
