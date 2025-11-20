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
    /// Handles BillApprovedEto to create payment task after approval
    /// </summary>
    public class BillApprovedEventHandler :
        IDistributedEventHandler<dynamic>,
        ITransientDependency
    {
        private readonly FinancialTaskAutomationService _automationService;
        private readonly ILogger<BillApprovedEventHandler> _logger;

        public BillApprovedEventHandler(
            FinancialTaskAutomationService automationService,
            ILogger<BillApprovedEventHandler> logger)
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
                var tenantId = eventData.TenantId as Guid?;
                var approvedBy = (Guid)eventData.ApprovedBy;

                _logger.LogInformation(
                    "Bill #{BillNumber} approved, creating payment task",
                    billNumber);

                var paymentTask = await _automationService.CreateBillPaymentTaskAsync(
                    billId,
                    billNumber,
                    vendorName,
                    amount.Amount,
                    amount.Currency,
                    dueDate,
                    approvedBy,
                    tenantId);

                _logger.LogInformation(
                    "Created payment task {TaskId} for approved bill #{BillNumber}",
                    paymentTask.Id, billNumber);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error handling BillApprovedEto");
                throw;
            }
        }
    }
}
