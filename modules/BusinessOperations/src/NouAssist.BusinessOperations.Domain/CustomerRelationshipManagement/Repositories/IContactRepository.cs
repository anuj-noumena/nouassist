using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NouAssist.BusinessOperations.CustomerRelationshipManagement.Aggregates;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.BusinessOperations.CustomerRelationshipManagement.Repositories
{
    /// <summary>
    /// Repository interface for Contact aggregate
    /// </summary>
    public interface IContactRepository : IRepository<Contact, Guid>
    {
        /// <summary>
        /// Checks if a contact with the specified email already exists
        /// </summary>
        Task<bool> ExistsByEmailAsync(
            string email, 
            Guid? excludeId = null, 
            CancellationToken cancellationToken = default);
            
        /// <summary>
        /// Gets a contact by email address
        /// </summary>
        Task<Contact> FindByEmailAsync(
            string email, 
            CancellationToken cancellationToken = default);
            
        /// <summary>
        /// Gets contacts belonging to the specified organization
        /// </summary>
        Task<List<Contact>> GetByOrganizationAsync(
            Guid organizationId, 
            bool primaryOnly = false, 
            int skipCount = 0, 
            int maxResultCount = 10, 
            string sorting = null, 
            CancellationToken cancellationToken = default);
            
        /// <summary>
        /// Gets contacts belonging to the specified group
        /// </summary>
        Task<List<Contact>> GetByGroupAsync(
            Guid groupId, 
            int skipCount = 0, 
            int maxResultCount = 10, 
            string sorting = null, 
            CancellationToken cancellationToken = default);
            
        /// <summary>
        /// Gets contacts with the specified tag
        /// </summary>
        Task<List<Contact>> GetByTagAsync(
            string tag, 
            int skipCount = 0, 
            int maxResultCount = 10, 
            string sorting = null, 
            CancellationToken cancellationToken = default);
    }
}
