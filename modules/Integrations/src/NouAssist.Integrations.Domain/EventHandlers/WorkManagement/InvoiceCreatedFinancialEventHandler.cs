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
    /// Handles InvoiceCreatedEto to create follow-up tasks
    /// </summary>
    public class InvoiceCreatedFinancialEventHandler :
        ITransientDependency
    {
        private readonly FinancialTaskAutomationService _automationService;
        private readonly ILogger<InvoiceCreatedFinancialEventHandler> _logger;

        // Default IDs - these would ideally come from configuration or a service
        private readonly Guid _defaultTaskTypeId = Guid.Parse("00000000-0000-0000-0000-000000000001");
        private readonly Guid _defaultWorkflowId = Guid.Parse("00000000-0000-0000-0000-000000000002");
        private readonly Guid _todoStatusId = Guid.Parse("00000000-0000-0000-0000-000000000003");

        public InvoiceCreatedFinancialEventHandler(
            FinancialTaskAutomationService automationService,
            ILogger<InvoiceCreatedFinancialEventHandler> logger)
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
                var totalAmount = eventData.TotalAmount;
                var issueDate = (DateTime)eventData.IssueDate;
                var tenantId = eventData.TenantId as Guid?;
                var createdBy = eventData.CreatedBy as Guid? ?? Guid.Empty;

                _logger.LogInformation(
                    "Processing invoice created event for Invoice #{InvoiceNumber} to {CustomerName}",
                    invoiceNumber, customerName);

                // Create follow-up task (7 days after invoice sent)
                TaskInstance followUpTask = await _automationService.CreateInvoiceFollowUpTaskAsync(
                    invoiceId,
                    invoiceNumber,
                    customerName,
                    totalAmount.Amount,
                    totalAmount.Currency,
                    issueDate,
                    createdBy,
                    _defaultTaskTypeId,
                    _defaultWorkflowId,
                    _todoStatusId,
                    tenantId);

                _logger.LogInformation(
                    "Created follow-up task {TaskId} for invoice #{InvoiceNumber}",
                    followUpTask.Id, invoiceNumber);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error handling InvoiceCreatedEto");
                throw;
            }
        }
    }
}
