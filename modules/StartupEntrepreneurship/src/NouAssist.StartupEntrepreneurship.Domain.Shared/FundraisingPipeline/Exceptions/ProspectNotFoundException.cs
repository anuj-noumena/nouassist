using System;

namespace NouAssist.StartupEntrepreneurship.FundraisingPipeline.Exceptions
{
    /// <summary>
    /// Exception thrown when a prospect is not found
    /// </summary>
    public class ProspectNotFoundException : FundraisingPipelineDomainException
    {
        public ProspectNotFoundException(Guid id)
            : base("FundraisingPipeline:ErrorMessage:ProspectNotFound")
        {
            WithData("id", id);
        }
    }
}
