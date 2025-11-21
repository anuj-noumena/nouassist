using NouAssist.StartupEntrepreneurship.FundraisingPipeline.Enums;

namespace NouAssist.StartupEntrepreneurship.FundraisingPipeline.Exceptions
{
    /// <summary>
    /// Exception thrown when attempting an invalid fundraising round stage transition
    /// </summary>
    public class InvalidRoundStageTransitionException : FundraisingPipelineDomainException
    {
        public InvalidRoundStageTransitionException(RoundStage currentStage, RoundStage newStage)
            : base("FundraisingPipeline:ErrorMessage:InvalidRoundStageTransition")
        {
            WithData("currentStage", currentStage);
            WithData("newStage", newStage);
        }
    }
}
