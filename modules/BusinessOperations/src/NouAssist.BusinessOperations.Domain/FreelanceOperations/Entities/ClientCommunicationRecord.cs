using System;
using NouAssist.BusinessOperations.FreelanceOperations.ValueObjects;
using Volo.Abp.Domain.Entities;

namespace NouAssist.BusinessOperations.FreelanceOperations.Entities
{
    /// <summary>
    /// Entity representing a communication record with a client
    /// </summary>
    public class ClientCommunicationRecord : Entity<Guid>
    {
        /// <summary>
        /// ID of the client this communication is with
        /// </summary>
        public Guid ClientId { get; private set; }
        
        /// <summary>
        /// Details of the communication
        /// </summary>
        public CommunicationLog CommunicationDetails { get; private set; }
        
        /// <summary>
        /// ID of the project related to this communication, if any
        /// </summary>
        public Guid? ProjectId { get; private set; }
        
        /// <summary>
        /// Whether this communication requires follow-up
        /// </summary>
        public bool RequiresFollowUp { get; private set; }
        
        /// <summary>
        /// Date when follow-up is due, if applicable
        /// </summary>
        public DateTime? FollowUpDate { get; private set; }

        protected ClientCommunicationRecord()
        {
        }

        internal ClientCommunicationRecord(
            Guid id,
            Guid clientId,
            CommunicationLog communicationDetails,
            Guid? projectId = null,
            bool requiresFollowUp = false,
            DateTime? followUpDate = null)
            : base(id)
        {
            ClientId = clientId;
            CommunicationDetails = communicationDetails ?? throw new ArgumentNullException(nameof(communicationDetails));
            ProjectId = projectId;
            RequiresFollowUp = requiresFollowUp;
            FollowUpDate = followUpDate;
        }

        /// <summary>
        /// Update the project associated with this communication
        /// </summary>
        internal void UpdateProjectId(Guid? projectId)
        {
            ProjectId = projectId;
        }

        /// <summary>
        /// Set whether this communication requires follow-up
        /// </summary>
        internal void SetFollowUpRequirement(bool requiresFollowUp, DateTime? followUpDate = null)
        {
            RequiresFollowUp = requiresFollowUp;
            FollowUpDate = requiresFollowUp ? followUpDate : null;
        }

        /// <summary>
        /// Mark that follow-up has been completed
        /// </summary>
        internal void MarkFollowUpCompleted()
        {
            RequiresFollowUp = false;
            FollowUpDate = null;
        }
    }
}
