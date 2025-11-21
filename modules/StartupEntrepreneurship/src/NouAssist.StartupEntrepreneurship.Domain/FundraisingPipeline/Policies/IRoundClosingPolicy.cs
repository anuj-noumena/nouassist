using NouAssist.StartupEntrepreneurship.FundraisingPipeline.Aggregates;

namespace NouAssist.StartupEntrepreneurship.FundraisingPipeline.Policies
{
    /// <summary>
    /// Policy for validating fundraising round closing
    /// </summary>
    public interface IRoundClosingPolicy
    {
        /// <summary>
        /// Checks if a fundraising round can be closed
        /// </summary>
        PolicyResult CanClose(FundraisingRound round);
    }
}
