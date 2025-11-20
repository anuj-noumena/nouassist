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
    /// Handles BillDueSoonEto to create reminder tasks
    /// </summary>
    public class BillDueSoonEventHandler :
        ITransientDependency
    {
        private readonly FinancialTaskAutomationService _automationService;
        private readonly ILogger<BillDueSoonEventHandler> _logger;

        // Default IDs - these would ideally come from configuration or a service
        private readonly Guid _defaultTaskTypeId = Guid.Parse("00000000-0000-0000-0000-000000000001");
        private readonly Guid _defaultWorkflowId = Guid.Parse("00000000-0000-0000-0000-000000000002");
        private readonly Guid _todoStatusId = Guid.Parse("00000000-0000-0000-0000-000000000003");

        public BillDueSoonEventHandler(
            FinancialTaskAutomationService automationService,
            ILogger<BillDueSoonEventHandler> logger)
        {
            _automationService = automationService;
            _logger = logger;
        }

        [UnitOfWork]
        public virtual async Task HandleEventAsync(dynamic eventData)
        {
            try
            {
                var billId = (Guid)eventData.BillId;
                var billNumber = (string)eventData.BillNumber;
                var vendorName = (string)eventData.VendorName;
                var amount = eventData.Amount;
                var dueDate = (DateTime)eventData.DueDate;
                var daysUntilDue = (int)eventData.DaysUntilDue;
                var tenantId = eventData.TenantId as Guid?;

                _logger.LogInformation(
                    "Bill #{BillNumber} due in {Days} days, creating reminder task",
                    billNumber, daysUntilDue);

                // Create payment reminder task
                TaskInstance paymentTask = await _automationService.CreateBillPaymentTaskAsync(
                    billId,
                    billNumber,
                    vendorName,
                    amount.Amount,
                    amount.Currency,
                    dueDate,
                    Guid.Empty, // Will be assigned based on role
                    _defaultTaskTypeId,
                    _defaultWorkflowId,
                    _todoStatusId,
                    tenantId);

                _logger.LogInformation(
                    "Created payment reminder task {TaskId} for bill #{BillNumber}",
                    paymentTask.Id, billNumber);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error handling BillDueSoonEto");
                throw;
            }
        }
    }
}
