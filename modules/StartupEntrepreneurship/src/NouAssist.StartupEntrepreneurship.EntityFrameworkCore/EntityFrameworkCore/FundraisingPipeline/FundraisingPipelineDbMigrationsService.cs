using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NouAssist.StartupEntrepreneurship.FundraisingPipeline.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.MultiTenancy;

namespace NouAssist.StartupEntrepreneurship.FundraisingPipeline.EntityFrameworkCore
{
    public class FundraisingPipelineDbMigrationsService : ITransientDependency
    {
        private readonly ILogger<FundraisingPipelineDbMigrationsService> _logger;
        private readonly IDbContextProvider<FundraisingPipelineDbContext> _dbContextProvider;
        private readonly ICurrentTenant _currentTenant;
        private readonly IDataSeeder _dataSeeder;

        public FundraisingPipelineDbMigrationsService(
            ILogger<FundraisingPipelineDbMigrationsService> logger,
            IDbContextProvider<FundraisingPipelineDbContext> dbContextProvider,
            ICurrentTenant currentTenant,
            IDataSeeder dataSeeder)
        {
            _logger = logger;
            _dbContextProvider = dbContextProvider;
            _currentTenant = currentTenant;
            _dataSeeder = dataSeeder;
        }

        public async Task MigrateAsync()
        {
            try
            {
                if (_currentTenant.Id == null)
                {
                    _logger.LogInformation("Migrating FundraisingPipeline database for host...");
                    await MigrateAndSeedAsync(null);
                    _logger.LogInformation("Successfully completed FundraisingPipeline database migrations for host.");
                }
                else
                {
                    _logger.LogInformation($"Migrating FundraisingPipeline database for tenant {_currentTenant.Id}...");
                    await MigrateAndSeedAsync(_currentTenant.Id);
                    _logger.LogInformation($"Successfully completed FundraisingPipeline database migrations for tenant {_currentTenant.Id}.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during FundraisingPipeline database migration");
                throw;
            }
        }

        private async Task MigrateAndSeedAsync(Guid? tenantId)
        {
            using (_currentTenant.Change(tenantId))
            {
                var dbContext = await _dbContextProvider.GetDbContextAsync();
                await dbContext.Database.MigrateAsync();
                await _dataSeeder.SeedAsync();
            }
        }
    }
}
