using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.CustomFields.Data;
using NouAssist.CustomFields.Entities;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.CustomFields.EntityFrameworkCore.Repositories
{
    public class EfCoreCustomFieldValueRepository 
        : EfCoreRepository<ICustomFieldsDbContext, CustomFieldValue, Guid>, 
          ICustomFieldValueRepository
    {
        public EfCoreCustomFieldValueRepository(
            IDbContextProvider<ICustomFieldsDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<List<CustomFieldValue>> GetByEntityAsync(
            string entityType, 
            Guid entityId)
        {
            var dbSet = await GetDbSetAsync();
            
            return await dbSet
                .Where(v => v.EntityType == entityType && v.EntityId == entityId)
                .ToListAsync();
        }

        public async Task<CustomFieldValue> GetByFieldAndEntityAsync(
            Guid fieldDefinitionId, 
            string entityType, 
            Guid entityId)
        {
            var dbSet = await GetDbSetAsync();
            
            return await dbSet
                .FirstOrDefaultAsync(v => 
                    v.FieldDefinitionId == fieldDefinitionId &&
                    v.EntityType == entityType &&
                    v.EntityId == entityId);
        }

        public async Task<Dictionary<Guid, CustomFieldValue>> GetValuesForEntityAsync(
            string entityType, 
            Guid entityId)
        {
            var values = await GetByEntityAsync(entityType, entityId);
            
            return values.ToDictionary(v => v.FieldDefinitionId, v => v);
        }

        public async Task DeleteByEntityAsync(string entityType, Guid entityId)
        {
            var dbSet = await GetDbSetAsync();
            
            var values = await dbSet
                .Where(v => v.EntityType == entityType && v.EntityId == entityId)
                .ToListAsync();

            await DeleteManyAsync(values);
        }
    }
}
