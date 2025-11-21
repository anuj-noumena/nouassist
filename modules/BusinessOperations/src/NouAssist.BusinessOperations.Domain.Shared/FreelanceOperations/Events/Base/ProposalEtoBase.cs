using System;
using NouAssist.BusinessOperations.FreelanceOperations.Enums;

namespace NouAssist.BusinessOperations.FreelanceOperations.Events.Base
{
    /// <summary>
    /// Base event class for proposal-related events
    /// </summary>
    [Serializable]
    public abstract class ProposalEtoBase
    {
        /// <summary>
        /// ID of the proposal
        /// </summary>
        public Guid Id { get; set; }
        
        /// <summary>
        /// Title of the proposal
        /// </summary>
        public string Title { get; set; }
        
        /// <summary>
        /// ID of the project associated with this proposal
        /// </summary>
        public Guid ProjectId { get; set; }
        
        /// <summary>
        /// ID of the client receiving this proposal
        /// </summary>
        public Guid ClientId { get; set; }
        
        /// <summary>
        /// Current status of the proposal
        /// </summary>
        public ProposalStatus Status { get; set; }
        
        /// <summary>
        /// Document ID reference for the proposal document
        /// </summary>
        public string DocumentId { get; set; }
        
        /// <summary>
        /// Tenant ID for multi-tenancy support
        /// </summary>
        public Guid? TenantId { get; set; }
    }
}
