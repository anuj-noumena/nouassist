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
    /// Handles BillDueSoonEto to create reminder tasks
    /// </summary>
    public class BillDueSoonEventHandler :
        IDistributedEventHandler<dynamic>,
        ITransientDependency
    {
        private readonly FinancialTaskAutomationService _automationService;
        private readonly ILogger<BillDueSoonEventHandler> _logger;

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
                var paymentTask = await _automationService.CreateBillPaymentTaskAsync(
                    billId,
                    billNumber,
                    vendorName,
                    amount.Amount,
                    amount.Currency,
                    dueDate,
                    Guid.Empty, // Will be assigned based on role
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
