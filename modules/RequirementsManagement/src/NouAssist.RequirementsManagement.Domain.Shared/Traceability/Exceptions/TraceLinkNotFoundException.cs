using System;

namespace NouAssist.RequirementsManagement.Traceability.Exceptions
{
    /// <summary>
    /// Exception thrown when a trace link is not found
    /// </summary>
    public class TraceLinkNotFoundException : TraceabilityManagementDomainException
    {
        public Guid LinkId { get; }

        public TraceLinkNotFoundException(Guid id)
            : base("TraceabilityManagement:ErrorMessage:TraceLinkNotFound")
        {
            LinkId = id;
            WithData("id", id);
        }
    }
}
