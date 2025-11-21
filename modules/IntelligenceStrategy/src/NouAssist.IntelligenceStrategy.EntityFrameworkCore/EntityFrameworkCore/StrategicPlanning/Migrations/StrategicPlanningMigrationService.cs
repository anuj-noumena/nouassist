using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EntityFrameworkCore.Migrations;
using Volo.Abp.MultiTenancy;

namespace NouAssist.IntelligenceStrategy.StrategicPlanning.EntityFrameworkCore.Migrations
{
    /// <summary>
    /// Service for managing Strategic Planning database migrations
    /// </summary>
    public class StrategicPlanningMigrationService : ITransientDependency
    {
        private readonly ILogger<StrategicPlanningMigrationService> _logger;
        private readonly ICurrentTenant _currentTenant;
        private readonly IServiceProvider _serviceProvider;

        public StrategicPlanningMigrationService(
            ILogger<StrategicPlanningMigrationService> logger,
            ICurrentTenant currentTenant,
            IServiceProvider serviceProvider)
        {
            _logger = logger;
            _currentTenant = currentTenant;
            _serviceProvider = serviceProvider;
        }

        public async Task MigrateAsync()
        {
            using var scope = _serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<StrategicPlanningDbContext>();

            _logger.LogInformation("Starting Strategic Planning database migration...");

            await dbContext.Database.MigrateAsync();

            _logger.LogInformation("Strategic Planning database migration completed successfully.");
        }

        public async Task MigrateAsync(Guid? tenantId)
        {
            using (_currentTenant.Change(tenantId))
            {
                await MigrateAsync();
            }
        }
    }
}
