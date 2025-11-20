using System;
using Microsoft.Extensions.DependencyInjection;
using NouAssist.ContractPortfolioManagement.Aggregates.Contract;
using NouAssist.ContractPortfolioManagement.Aggregates.Portfolio;
using NouAssist.ContractPortfolioManagement.Infrastructure.EntityFrameworkCore.Repositories;
using NouAssist.ContractPortfolioManagement.Repositories;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.PostgreSQL;
using Volo.Abp.Modularity;

namespace NouAssist.ContractPortfolioManagement.EntityFrameworkCore
{
    [DependsOn(
        typeof(ContractPortfolioManagementDomainModule),
        typeof(AbpEntityFrameworkCorePostgreSQLModule)
    )]
    public class ContractPortfolioManagementEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            // Register the DbContext for Dependency Injection
            context.Services.AddAbpDbContext<ContractPortfolioManagementDbContext>(options =>
            {
                // Register CUSTOM repositories
                options.AddRepository<ContractAggregate, EfCoreContractRepository>();
                options.AddRepository<PortfolioAggregate, EfCorePortfolioRepository>();
                options.AddRepository<PortfolioRoadmapAggregate, EfCorePortfolioRoadmapRepository>();
            });

            // Configure EF Core options
            Configure<AbpDbContextOptions>(options =>
            {
                options.UseNpgsql(); // Configure to use PostgreSQL
            });
        }
    }
}
