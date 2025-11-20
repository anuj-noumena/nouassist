// Update in BusinessModelManagementApplicationModule.cs
[DependsOn(
    typeof(BusinessModelManagementDomainModule),
    typeof(BusinessModelManagementApplicationContractsModule),
    typeof(AbpDddApplicationModule),
    typeof(AbpAutoMapperModule),
    typeof(AbpFluentValidationModule),
    typeof(AbpBackgroundWorkersModule) // Add this
)]
public class BusinessModelManagementApplicationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        // Existing configuration...

        // Add background workers
        context.Services.AddSingleton<PivotMonitoringBackgroundService>();
        
        // Configure distributed events
        Configure<AbpDistributedEventBusOptions>(options =>
        {
            // Add any additional event configurations
            options.EnabledErrorHandle = true;
            options.ErrorHandleSelector = context => true;
        });
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        // Add background worker
        context.AddBackgroundWorker<PivotMonitoringBackgroundService>();
    }
}

// Add to BusinessModelManagementApplicationModule.cs ConfigureServices method
Configure<AbpUnitOfWorkDefaultOptions>(options =>
{
    // Configure default isolation level for this module
    options.IsolationLevel = IsolationLevel.ReadCommitted;
});
