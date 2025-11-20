namespace NouAssist.IntelligenceStrategy.WorkflowEngine.Validation
{
    /// <summary>
    /// Module to register FluentValidation validators
    /// </summary>
    [DependsOn(
        typeof(AbpFluentValidationModule)
    )]
    public class WorkflowEngineValidationModule : AbpModule
    {
        /// <summary>
        /// Configures services for the module
        /// </summary>
        /// <param name="context">The service configuration context</param>
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            // Register all validators in this assembly
            context.Services.AddValidatorsFromAssemblyContaining<WorkflowEngineValidationModule>();
        }
    }
}
