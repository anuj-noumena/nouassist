using NouAssist.StartupEntrepreneurship.FundraisingPipeline.Enums;

namespace NouAssist.StartupEntrepreneurship.FundraisingPipeline.Exceptions
{
    /// <summary>
    /// Exception thrown when attempting an invalid prospect status transition
    /// </summary>
    public class InvalidProspectStatusTransitionException : FundraisingPipelineDomainException
    {
        public InvalidProspectStatusTransitionException(ProspectStatus currentStatus, ProspectStatus newStatus)
            : base("FundraisingPipeline:ErrorMessage:InvalidProspectStatusTransition")
        {
            WithData("currentStatus", currentStatus);
            WithData("newStatus", newStatus);
        }
    }
}
