using System;

namespace NouAssist.BusinessOperations.FreelanceOperations.Exceptions
{
    /// <summary>
    /// Exception thrown when a client is not found
    /// </summary>
    public class ClientNotFoundException : FreelanceOperationsDomainException
    {
        public ClientNotFoundException(Guid id)
            : base("FreelanceOperations:ErrorMessage:ClientNotFound")
        {
            WithData("id", id);
        }
    }
}
