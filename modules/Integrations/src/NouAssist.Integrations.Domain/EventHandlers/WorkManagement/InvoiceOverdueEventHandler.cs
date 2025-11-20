using System;
using Microsoft.Extensions.Logging;
using NouAssist.WorkManagement.TaskManagement.Aggregates;
using NouAssist.WorkManagement.TaskManagement.Services;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Uow;
using Task = System.Threading.Tasks.Task;

namespace NouAssist.Integrations.EventHandlers.WorkManagement
{
    /// <summary>
    /// Handles InvoiceOverdueEto to create collection tasks
    /// </summary>
    public class InvoiceOverdueEventHandler :
        ITransientDependency
    {
        private readonly FinancialTaskAutomationService _automationService;
        private readonly ILogger<InvoiceOverdueEventHandler> _logger;

        // Default IDs - these would ideally come from configuration or a service
        private readonly Guid _defaultTaskTypeId = Guid.Parse("00000000-0000-0000-0000-000000000001");
        private readonly Guid _defaultWorkflowId = Guid.Parse("00000000-0000-0000-0000-000000000002");
        private readonly Guid _todoStatusId = Guid.Parse("00000000-0000-0000-0000-000000000003");

        public InvoiceOverdueEventHandler(
            FinancialTaskAutomationService automationService,
            ILogger<InvoiceOverdueEventHandler> logger)
        {
            _automationService = automationService;
            _logger = logger;
        }

        [UnitOfWork]
        public virtual async Task HandleEventAsync(dynamic eventData)
        {
            try
            {
                var invoiceId = (Guid)eventData.InvoiceId;
                var invoiceNumber = (string)eventData.InvoiceNumber;
                var customerName = (string)eventData.CustomerName;
                var amountDue = eventData.AmountDue;
                var daysOverdue = (int)eventData.DaysOverdue;
                var tenantId = eventData.TenantId as Guid?;

                _logger.LogInformation(
                    "Invoice #{InvoiceNumber} is {Days} days overdue, creating collection task",
                    invoiceNumber, daysOverdue);

                // Create collection task
                TaskInstance collectionTask = await _automationService.CreateInvoiceCollectionTaskAsync(
                    invoiceId,
                    invoiceNumber,
                    customerName,
                    amountDue.Amount,
                    amountDue.Currency,
                    daysOverdue,
                    Guid.Empty, // Will be assigned based on role
                    _defaultTaskTypeId,
                    _defaultWorkflowId,
                    _todoStatusId,
                    tenantId);

                _logger.LogInformation(
                    "Created collection task {TaskId} for overdue invoice #{InvoiceNumber}",
                    collectionTask.Id, invoiceNumber);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error handling InvoiceOverdueEto");
                throw;
            }
        }
    }
}
