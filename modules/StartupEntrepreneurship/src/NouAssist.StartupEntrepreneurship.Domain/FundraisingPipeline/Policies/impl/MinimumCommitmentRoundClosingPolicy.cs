using NouAssist.StartupEntrepreneurship.FundraisingPipeline.Aggregates;
using NouAssist.StartupEntrepreneurship.FundraisingPipeline.Enums;

namespace NouAssist.StartupEntrepreneurship.FundraisingPipeline.Policies.impl
{
    /// <summary>
    /// Round closing policy that requires minimum commitment percentage
    /// </summary>
    public class MinimumCommitmentRoundClosingPolicy : IRoundClosingPolicy
    {
        private readonly decimal _minimumCommitmentPercentage;

        public MinimumCommitmentRoundClosingPolicy(decimal minimumCommitmentPercentage = 0.75m)
        {
            _minimumCommitmentPercentage = minimumCommitmentPercentage;
        }

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

            // Check minimum commitment
            if (round.TargetAmount > 0)
            {
                var commitmentPercentage = round.CommittedAmount / round.TargetAmount;
                if (commitmentPercentage < _minimumCommitmentPercentage)
                {
                    return PolicyResult.Deny(
                        $"Cannot close round with only {commitmentPercentage:P} committed. " +
                        $"Minimum required: {_minimumCommitmentPercentage:P}");
                }
            }

            return PolicyResult.Allow();
        }
    }
}
