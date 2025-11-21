using System;
using System.Threading.Tasks;
using NouAssist.BusinessOperations.CustomerRelationshipManagement.Aggregates;
using NouAssist.BusinessOperations.CustomerRelationshipManagement.Enums;
using NouAssist.BusinessOperations.CustomerRelationshipManagement.ValueObjects;

namespace NouAssist.BusinessOperations.CustomerRelationshipManagement.Services.Interfaces
{
    /// <summary>
    /// Domain service for managing leads
    /// </summary>
    public interface ILeadManager
    {
        /// <summary>
        /// Creates a new lead
        /// </summary>
        Task<Lead> CreateAsync(
            string name,
            ContactInfo contactInfo,
            LeadDetails details,
            LeadStatus status = LeadStatus.New,
            int score = 0);
            
        /// <summary>
        /// Updates an existing lead
        /// </summary>
        Task<Lead> UpdateAsync(
            Guid id,
            string name = null,
            ContactInfo contactInfo = null,
            LeadDetails details = null);
            
        /// <summary>
        /// Changes the status of a lead
        /// </summary>
        Task<Lead> ChangeStatusAsync(
            Guid id,
            LeadStatus newStatus);
            
        /// <summary>
        /// Updates the score of a lead
        /// </summary>
        Task<Lead> UpdateScoreAsync(
            Guid id,
            int score);
            
        /// <summary>
        /// Converts a lead to a contact
        /// </summary>
        Task<(Lead Lead, Contact Contact)> ConvertToContactAsync(
            Guid id,
            Guid? organizationId = null,
            bool isPrimaryContact = false);
            
        /// <summary>
        /// Adds a tag to a lead
        /// </summary>
        Task<Lead> AddTagAsync(
            Guid id,
            string tag);
            
        /// <summary>
        /// Removes a tag from a lead
        /// </summary>
        Task<Lead> RemoveTagAsync(
            Guid id,
            string tag);
    }
}
