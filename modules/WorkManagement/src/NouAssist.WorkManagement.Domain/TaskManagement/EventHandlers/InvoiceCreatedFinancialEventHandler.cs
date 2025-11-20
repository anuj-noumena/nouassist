using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NouAssist.WorkManagement.TaskManagement.Services;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Uow;

namespace NouAssist.WorkManagement.TaskManagement.EventHandlers
{
    /// <summary>
    /// Handles InvoiceCreatedEto to create follow-up tasks
    /// </summary>
    public class InvoiceCreatedFinancialEventHandler :
        IDistributedEventHandler<dynamic>,
        ITransientDependency
    {
        private readonly FinancialTaskAutomationService _automationService;
        private readonly ILogger<InvoiceCreatedFinancialEventHandler> _logger;

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
                var followUpTask = await _automationService.CreateInvoiceFollowUpTaskAsync(
                    invoiceId,
                    invoiceNumber,
                    customerName,
                    totalAmount.Amount,
                    totalAmount.Currency,
                    issueDate,
                    createdBy,
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
