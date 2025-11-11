using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.Customization.CustomFields.Aggregates;
using NouAssist.Customization.CustomFields.Data;
using NouAssist.Customization.CustomFields.Enums;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Specifications;

namespace NouAssist.Customization.CustomFields.EntityFrameworkCore.Repositories
{
    public class EfCoreCustomFieldDefinitionRepository 
        : EfCoreRepository<ICustomFieldsDbContext, CustomFieldDefinition, Guid>, 
          ICustomFieldDefinitionRepository
    {
        public EfCoreCustomFieldDefinitionRepository(
            IDbContextProvider<ICustomFieldsDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<List<CustomFieldDefinition>> GetByEntityTypeAsync(
            CustomFieldEntityType entityType)
        {
            var dbSet = await GetDbSetAsync();
            
            return await dbSet
                .Where(f => f.EntityType == entityType)
                .OrderBy(f => f.DisplayOrder)
                .ThenBy(f => f.Name)
                .ToListAsync();
        }

        public async Task<List<CustomFieldDefinition>> GetActiveFieldsAsync(
            CustomFieldEntityType entityType)
        {
            var dbSet = await GetDbSetAsync();
            
            return await dbSet
                .Where(f => f.EntityType == entityType && f.IsActive)
                .OrderBy(f => f.DisplayOrder)
                .ThenBy(f => f.Name)
                .ToListAsync();
        }

        public async Task<CustomFieldDefinition> GetByFieldKeyAsync(string fieldKey)
        {
            var dbSet = await GetDbSetAsync();
            
            return await dbSet
                .FirstOrDefaultAsync(f => f.FieldKey == fieldKey);
        }

        public async Task<List<CustomFieldDefinition>> GetByOwnerAsync(Guid ownerId)
        {
            var dbSet = await GetDbSetAsync();
            
            return await dbSet
                .Where(f => f.OwnerId == ownerId)
                .OrderBy(f => f.EntityType)
                .ThenBy(f => f.DisplayOrder)
                .ToListAsync();
        }

        public async Task<List<CustomFieldDefinition>> GetListAsync(
            ISpecification<CustomFieldDefinition> spec)
        {
            var query = await GetQueryableAsync();
            
            return await query
                .Where(spec.ToExpression())
                .ToListAsync();
        }
    }
}
