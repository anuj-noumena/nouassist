using System;

namespace NouAssist.BusinessOperations.FreelanceOperations.Exceptions
{
    /// <summary>
    /// Exception thrown when a proposal is not found
    /// </summary>
    public class ProposalNotFoundException : FreelanceOperationsDomainException
    {
        public ProposalNotFoundException(Guid id)
            : base("FreelanceOperations:ErrorMessage:ProposalNotFound")
        {
            WithData("id", id);
        }
    }
}
