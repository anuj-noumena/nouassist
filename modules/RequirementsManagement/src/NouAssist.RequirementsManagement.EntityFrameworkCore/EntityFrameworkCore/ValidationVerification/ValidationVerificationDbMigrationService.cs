using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.MultiTenancy;

namespace NouAssist.RequirementsManagement.ValidationVerification.EntityFrameworkCore
{
    /// <summary>
    /// Service to handle database migrations for ValidationVerification module
    /// </summary>
    public class ValidationVerificationDbMigrationService : ITransientDependency
    {
        private readonly ILogger<ValidationVerificationDbMigrationService> _logger;
        private readonly IDbContextProvider<ValidationVerificationDbContext> _dbContextProvider;
        private readonly ICurrentTenant _currentTenant;
        private readonly IDataSeeder _dataSeeder;

        public ValidationVerificationDbMigrationService(
            ILogger<ValidationVerificationDbMigrationService> logger,
            IDbContextProvider<ValidationVerificationDbContext> dbContextProvider,
            ICurrentTenant currentTenant,
            IDataSeeder dataSeeder)
        {
            _logger = logger;
            _dbContextProvider = dbContextProvider;
            _currentTenant = currentTenant;
            _dataSeeder = dataSeeder;
        }

        /// <summary>
        /// Migrates the database schema and seeds initial data
        /// </summary>
        public async Task MigrateAsync()
        {
            _logger.LogInformation("Starting ValidationVerification database migration...");

            try
            {
                var dbContext = await _dbContextProvider.GetDbContextAsync();
                
                // Apply pending migrations
                await dbContext.Database.MigrateAsync();
                
                _logger.LogInformation("ValidationVerification database migration completed successfully");

                // Seed initial data
                await SeedAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while migrating ValidationVerification database");
                throw;
            }
        }

        /// <summary>
        /// Seeds initial data for the module
        /// </summary>
        public async Task SeedAsync()
        {
            _logger.LogInformation("Starting ValidationVerification data seeding...");

            try
            {
                using (_currentTenant.Change(null))
                {
                    await _dataSeeder.SeedAsync(new DataSeedContext()
                        .WithProperty("ValidationVerification", true));
                }

                _logger.LogInformation("ValidationVerification data seeding completed successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while seeding ValidationVerification data");
                throw;
            }
        }
    }
}
