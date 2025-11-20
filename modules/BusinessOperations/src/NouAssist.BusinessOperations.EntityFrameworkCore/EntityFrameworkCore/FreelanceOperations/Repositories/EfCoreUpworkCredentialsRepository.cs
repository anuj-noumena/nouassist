using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using NouAssist.BusinessOperations.FreelanceOperations.Interfaces.Repositories;
using NouAssist.BusinessOperations.FreelanceOperations.ValueObjects;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Json;

namespace NouAssist.BusinessOperations.FreelanceOperations.EntityFrameworkCore.Repositories
{
    public class EfCoreUpworkCredentialsRepository : IUpworkCredentialsRepository, ITransientDependency
    {
        private readonly IDbContextProvider<FreelanceOperationsDbContext> _dbContextProvider;
        private readonly IDistributedCache<UpworkCredentials> _cache;
        private readonly IJsonSerializer _jsonSerializer;

        public EfCoreUpworkCredentialsRepository(
            IDbContextProvider<FreelanceOperationsDbContext> dbContextProvider,
            IDistributedCache<UpworkCredentials> cache,
            IJsonSerializer jsonSerializer)
        {
            _dbContextProvider = dbContextProvider;
            _cache = cache;
            _jsonSerializer = jsonSerializer;
        }

        public async Task<UpworkCredentials> GetCredentialsAsync(Guid? tenantId)
        {
            // Check cache first
            var cacheKey = GetCacheKey(tenantId);
            var cachedCredentials = await _cache.GetAsync(cacheKey);
            if (cachedCredentials != null)
            {
                return cachedCredentials;
            }

            // If not in cache, get from database
            var dbContext = await _dbContextProvider.GetDbContextAsync();
            
            // Using EF DbContext directly for settings table specific to this use case
            var settingsEntity = await dbContext.Set<UpworkCredentialsSetting>()
                .FirstOrDefaultAsync(s => s.TenantId == tenantId);

            if (settingsEntity == null)
            {
                return null;
            }

            // Deserialize stored credentials
            var credentials = _jsonSerializer.Deserialize<UpworkCredentials>(settingsEntity.CredentialsJson);
            
            // Cache the credentials
            await _cache.SetAsync(cacheKey, credentials);
            
            return credentials;
        }

        public async Task SetCredentialsAsync(UpworkCredentials credentials, Guid? tenantId)
        {
            var dbContext = await _dbContextProvider.GetDbContextAsync();
            
            // Serialize credentials to JSON
            var credentialsJson = _jsonSerializer.Serialize(credentials);
            
            // Check if the credentials already exist
            var settingsEntity = await dbContext.Set<UpworkCredentialsSetting>()
                .FirstOrDefaultAsync(s => s.TenantId == tenantId);

            if (settingsEntity == null)
            {
                // Create new settings
                settingsEntity = new UpworkCredentialsSetting
                {
                    Id = Guid.NewGuid(),
                    TenantId = tenantId,
                    CredentialsJson = credentialsJson
                };
                
                dbContext.Set<UpworkCredentialsSetting>().Add(settingsEntity);
            }
            else
            {
                // Update existing settings
                settingsEntity.CredentialsJson = credentialsJson;
                settingsEntity.LastModifiedTime = DateTime.UtcNow;
            }
            
            await dbContext.SaveChangesAsync();
            
            // Update cache
            var cacheKey = GetCacheKey(tenantId);
            await _cache.SetAsync(cacheKey, credentials);
        }

        private string GetCacheKey(Guid? tenantId)
        {
            return $"UpworkCredentials:{tenantId?.ToString() ?? "Host"}";
        }
    }

    /// <summary>
    /// Entity for storing Upwork credentials in the database
    /// </summary>
    [EntityFrameworkCore.Modeling.ConnectionStringName(FreelanceOperationsDbProperties.ConnectionStringName)]
    public class UpworkCredentialsSetting
    {
        public Guid Id { get; set; }
        public Guid? TenantId { get; set; }
        public string CredentialsJson { get; set; }
        public DateTime CreationTime { get; set; } = DateTime.UtcNow;
        public DateTime? LastModifiedTime { get; set; }
    }
}
