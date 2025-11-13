using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.WorkManagement.Reviews.Aggregates;
using NouAssist.WorkManagement.Reviews.Data;
using NouAssist.WorkManagement.Reviews.Enums;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.WorkManagement.Reviews.EntityFrameworkCore.Repositories
{
    public class EfCoreReviewTemplateRepository 
        : EfCoreRepository<IReviewsDbContext, ReviewTemplate, Guid>, IReviewTemplateRepository
    {
        public EfCoreReviewTemplateRepository(
            IDbContextProvider<IReviewsDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<List<ReviewTemplate>> GetSystemTemplatesAsync(
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            
            return await query
                .Where(t => t.IsSystem)
                .OrderBy(t => t.ReviewType)
                .ThenBy(t => t.Title)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<ReviewTemplate>> GetCustomTemplatesAsync(
            Guid ownerId,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            
            return await query
                .Where(t => !t.IsSystem && t.OwnerId == ownerId)
                .OrderBy(t => t.ReviewType)
                .ThenBy(t => t.Title)
                .ToListAsync(cancellationToken);
        }

        public async Task<ReviewTemplate> GetByReviewTypeAsync(
            ReviewType reviewType,
            Guid? ownerId = null,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            
            // First try to get custom template, fallback to system template
            var template = await query
                .Where(t => t.ReviewType == reviewType && !t.IsSystem && t.OwnerId == ownerId)
                .FirstOrDefaultAsync(cancellationToken);

            if (template == null)
            {
                template = await query
                    .Where(t => t.ReviewType == reviewType && t.IsSystem)
                    .FirstOrDefaultAsync(cancellationToken);
            }

            return template;
        }

        public async Task<List<ReviewTemplate>> GetTemplatesForOwnerAsync(
            Guid ownerId,
            bool includeSystem = true,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            
            if (includeSystem)
            {
                query = query.Where(t => t.IsSystem || t.OwnerId == ownerId);
            }
            else
            {
                query = query.Where(t => !t.IsSystem && t.OwnerId == ownerId);
            }

            return await query
                .OrderBy(t => t.ReviewType)
                .ThenBy(t => t.Title)
                .ToListAsync(cancellationToken);
        }
    }
}
