using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.BusinessOperations.CustomerRelationshipManagement.Entities;
using NouAssist.BusinessOperations.CustomerRelationshipManagement.EntityFrameworkCore;
using NouAssist.BusinessOperations.CustomerRelationshipManagement.Enums;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.BusinessOperations.CustomerRelationshipManagement.Repositories
{
    public class CommunicationTemplateRepository : EfCoreRepository<CRMDbContext, CommunicationTemplate, Guid>, ICommunicationTemplateRepository
    {
        public CommunicationTemplateRepository(IDbContextProvider<CRMDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<CommunicationTemplate> GetByCodeAsync(
            string code, 
            CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();
            
            return await dbContext.CommunicationTemplates
                .Where(t => t.Code == code)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<List<CommunicationTemplate>> GetByTypeAsync(
            InteractionType type, 
            bool activeOnly = true, 
            int skipCount = 0, 
            int maxResultCount = 10, 
            string sorting = null, 
            CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();
            
            var query = dbContext.CommunicationTemplates
                .Where(t => t.Type == type);
                
            if (activeOnly)
            {
                query = query.Where(t => t.IsActive);
            }
            
            return await query
                .OrderBy(sorting ?? nameof(CommunicationTemplate.Name))
                .Skip(skipCount)
                .Take(maxResultCount)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<CommunicationTemplate>> GetByCategoryAsync(
            string category, 
            bool activeOnly = true, 
            int skipCount = 0, 
            int maxResultCount = 10, 
            string sorting = null, 
            CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();
            
            var query = dbContext.CommunicationTemplates
                .Where(t => t.Category == category);
                
            if (activeOnly)
            {
                query = query.Where(t => t.IsActive);
            }
            
            return await query
                .OrderBy(sorting ?? nameof(CommunicationTemplate.Name))
                .Skip(skipCount)
                .Take(maxResultCount)
                .ToListAsync(cancellationToken);
        }
    }
}
