using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NouAssist.BusinessOperations.CustomerRelationshipManagement.Enums;
using NouAssist.BusinessOperations.CustomerRelationshipManagement.Services.Interfaces;
using Volo.Abp.BackgroundWorkers;
using Volo.Abp.Threading;

namespace NouAssist.BusinessOperations.CustomerRelationshipManagement.BackgroundWorkers
{
    /// <summary>
    /// Background worker that executes automation workflows
    /// </summary>
    public class AutomationWorkflowExecutor : AsyncPeriodicBackgroundWorkerBase
    {
        public AutomationWorkflowExecutor(
            AbpAsyncTimer timer,
            IServiceScopeFactory serviceScopeFactory)
            : base(timer, serviceScopeFactory)
        {
            Timer.Period = 60000; // 1 minute
        }

        protected override async Task DoWorkAsync(PeriodicBackgroundWorkerContext workerContext)
        {
            Logger.LogInformation("AutomationWorkflowExecutor is working...");
            
            try
            {
                var automationManager = workerContext.ServiceProvider.GetRequiredService<IAutomationManager>();
                
                // Execute scheduled workflows
                // This is a simplified implementation
                // In a real system, we would have a more sophisticated scheduling system
                
                // Find scheduled workflows
                var workflows = await automationManager.GetMatchingWorkflowsAsync(
                    AutomationTriggerType.ScheduledTime,
                    "system");
                    
                foreach (var workflow in workflows)
                {
                    try
                    {
                        // Trigger the workflow
                        await automationManager.TriggerWorkflowExecutionAsync(
                            workflow.Id,
                            Guid.Empty, // System trigger
                            "system");
                    }
                    catch (Exception ex)
                    {
                        Logger.LogError(ex, "Error executing workflow {WorkflowId}: {Message}", workflow.Id, ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error in AutomationWorkflowExecutor: {Message}", ex.Message);
            }
        }
    }
}
