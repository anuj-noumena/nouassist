using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NouAssist.BusinessOperations.CustomerRelationshipManagement.Entities;
using NouAssist.BusinessOperations.CustomerRelationshipManagement.Enums;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.BusinessOperations.CustomerRelationshipManagement.Repositories
{
    /// <summary>
    /// Repository interface for CommunicationTemplate entity
    /// </summary>
    public interface ICommunicationTemplateRepository : IRepository<CommunicationTemplate, Guid>
    {
        /// <summary>
        /// Gets a template by its unique code
        /// </summary>
        Task<CommunicationTemplate> GetByCodeAsync(
            string code, 
            CancellationToken cancellationToken = default);
            
        /// <summary>
        /// Gets templates of the specified type
        /// </summary>
        Task<List<CommunicationTemplate>> GetByTypeAsync(
            InteractionType type, 
            bool activeOnly = true, 
            int skipCount = 0, 
            int maxResultCount = 10, 
            string sorting = null, 
            CancellationToken cancellationToken = default);
            
        /// <summary>
        /// Gets templates in the specified category
        /// </summary>
        Task<List<CommunicationTemplate>> GetByCategoryAsync(
            string category, 
            bool activeOnly = true, 
            int skipCount = 0, 
            int maxResultCount = 10, 
            string sorting = null, 
            CancellationToken cancellationToken = default);
    }
}
