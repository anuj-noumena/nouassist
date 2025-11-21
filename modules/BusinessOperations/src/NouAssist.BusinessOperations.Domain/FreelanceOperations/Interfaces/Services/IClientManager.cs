using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.BusinessOperations.FreelanceOperations.Aggregates;
using NouAssist.BusinessOperations.FreelanceOperations.Entities;
using NouAssist.BusinessOperations.FreelanceOperations.Enums;
using NouAssist.BusinessOperations.FreelanceOperations.ValueObjects;

namespace NouAssist.BusinessOperations.FreelanceOperations.Interfaces.Services
{
    /// <summary>
    /// Domain service interface for managing freelance clients
    /// </summary>
    public interface IClientManager
    {
        /// <summary>
        /// Creates a new freelance client
        /// </summary>
        Task<FreelanceClient> CreateClientAsync(
            string name,
            ClientContactInfo contactInfo,
            string companyName = null,
            ClientStatus initialStatus = ClientStatus.Lead,
            string source = null,
            string notes = null);
        
        /// <summary>
        /// Updates the basic information for a client
        /// </summary>
        Task UpdateClientBasicInfoAsync(
            Guid clientId,
            string name,
            string companyName,
            string notes);
        
        /// <summary>
        /// Updates the contact information for a client
        /// </summary>
        Task UpdateClientContactInfoAsync(
            Guid clientId,
            ClientContactInfo contactInfo);
        
        /// <summary>
        /// Changes the status of a client
        /// </summary>
        Task ChangeClientStatusAsync(
            Guid clientId,
            ClientStatus newStatus);
        
        /// <summary>
        /// Adds a communication record for a client
        /// </summary>
        Task<ClientCommunicationRecord> AddCommunicationRecordAsync(
            Guid clientId,
            string communicationType,
            string subject,
            string content,
            Guid? projectId = null,
            bool requiresFollowUp = false,
            DateTime? followUpDate = null);
        
        /// <summary>
        /// Gets clients with pending follow-ups
        /// </summary>
        Task<List<FreelanceClient>> GetClientsWithPendingFollowUpsAsync(DateTime? dueBy = null);
    }
}
