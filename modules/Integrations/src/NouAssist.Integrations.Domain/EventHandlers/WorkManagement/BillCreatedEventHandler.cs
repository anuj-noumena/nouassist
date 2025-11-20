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
    /// Handles BillCreatedEto to automatically create approval and payment tasks
    /// </summary>
    public class BillCreatedEventHandler :
        ITransientDependency
    {
        private readonly FinancialTaskAutomationService _automationService;
        private readonly ILogger<BillCreatedEventHandler> _logger;

        // Default IDs - these would ideally come from configuration or a service
        private readonly Guid _defaultTaskTypeId = Guid.Parse("00000000-0000-0000-0000-000000000001");
        private readonly Guid _defaultWorkflowId = Guid.Parse("00000000-0000-0000-0000-000000000002");
        private readonly Guid _todoStatusId = Guid.Parse("00000000-0000-0000-0000-000000000003");

        public BillCreatedEventHandler(
            FinancialTaskAutomationService automationService,
            ILogger<BillCreatedEventHandler> logger)
        {
            _automationService = automationService;
            _logger = logger;
        }

        [UnitOfWork]
        public virtual async Task HandleEventAsync(dynamic eventData)
        {
            // This handler is registered manually to avoid compile-time dependency
            // The event structure is defined in FinancialManagement.Domain.Shared

            try
            {
                var billId = (Guid)eventData.BillId;
                var billNumber = (string)eventData.BillNumber;
                var vendorName = (string)eventData.VendorName;
                var amount = eventData.Amount;
                var dueDate = (DateTime)eventData.DueDate;
                var categoryId = (Guid)eventData.CategoryId;
                var projectId = eventData.ProjectId as Guid?;
                var requiresApproval = (bool)eventData.RequiresApproval;
                var tenantId = eventData.TenantId as Guid?;
                var createdBy = (Guid)eventData.CreatedBy;

                _logger.LogInformation(
                    "Processing bill created event for Bill #{BillNumber} from {VendorName}",
                    billNumber, vendorName);

                // Create approval task if required
                if (requiresApproval)
                {
                    TaskInstance approvalTask = await _automationService.CreateBillApprovalTaskAsync(
                        billId,
                        billNumber,
                        vendorName,
                        amount.Amount,
                        amount.Currency,
                        dueDate,
                        categoryId,
                        projectId,
                        createdBy,
                        _defaultTaskTypeId,
                        _defaultWorkflowId,
                        _todoStatusId,
                        tenantId);

                    _logger.LogInformation(
                        "Created approval task {TaskId} for bill #{BillNumber}",
                        approvalTask.Id, billNumber);
                }
                else
                {
                    // Create payment task directly if no approval needed
                    TaskInstance paymentTask = await _automationService.CreateBillPaymentTaskAsync(
                        billId,
                        billNumber,
                        vendorName,
                        amount.Amount,
                        amount.Currency,
                        dueDate,
                        createdBy,
                        _defaultTaskTypeId,
                        _defaultWorkflowId,
                        _todoStatusId,
                        tenantId);

                    _logger.LogInformation(
                        "Created payment task {TaskId} for bill #{BillNumber}",
                        paymentTask.Id, billNumber);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error handling BillCreatedEto");
                throw;
            }
        }
    }
}
