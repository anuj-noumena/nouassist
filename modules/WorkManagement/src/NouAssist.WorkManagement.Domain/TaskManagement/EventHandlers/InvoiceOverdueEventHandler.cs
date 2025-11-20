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
    /// Handles InvoiceOverdueEto to create collection tasks
    /// </summary>
    public class InvoiceOverdueEventHandler :
        IDistributedEventHandler<dynamic>,
        ITransientDependency
    {
        private readonly FinancialTaskAutomationService _automationService;
        private readonly ILogger<InvoiceOverdueEventHandler> _logger;

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
                var collectionTask = await _automationService.CreateInvoiceCollectionTaskAsync(
                    invoiceId,
                    invoiceNumber,
                    customerName,
                    amountDue.Amount,
                    amountDue.Currency,
                    daysOverdue,
                    Guid.Empty, // Will be assigned based on role
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
