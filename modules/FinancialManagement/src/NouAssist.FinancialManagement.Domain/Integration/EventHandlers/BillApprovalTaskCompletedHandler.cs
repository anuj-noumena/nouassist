using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NouAssist.FinancialManagement.AccountsPayable.Data;
using NouAssist.FinancialManagement.AccountsPayable.Enums;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Uow;

namespace NouAssist.FinancialManagement.Integration.EventHandlers
{
    /// <summary>
    /// Handles TaskCompletedEto for bill approval tasks to approve the bill
    /// </summary>
    public class BillApprovalTaskCompletedHandler :
        ITransientDependency
    {
        private readonly IBillRepository _billRepository;
        private readonly ILogger<BillApprovalTaskCompletedHandler> _logger;

        public BillApprovalTaskCompletedHandler(
            IBillRepository billRepository,
            ILogger<BillApprovalTaskCompletedHandler> logger)
        {
            _billRepository = billRepository;
            _logger = logger;
        }

        [UnitOfWork]
        public virtual async Task HandleEventAsync(dynamic eventData)
        {
            try
            {
                // Check if this is a bill approval task
                var customFields = eventData.CustomFields as System.Collections.Generic.Dictionary<string, string>;
                if (customFields == null ||
                    !customFields.ContainsKey("FinancialEntityType") ||
                    customFields["FinancialEntityType"] != "Bill")
                {
                    return; // Not a bill-related task
                }

                if (!customFields.ContainsKey("BillId"))
                {
                    return; // Missing BillId
                }

                var billId = Guid.Parse(customFields["BillId"]);
                var completedBy = (Guid)eventData.CompletedBy;
                var resolutionNote = eventData.ResolutionNote as string;

                _logger.LogInformation(
                    "Processing bill approval task completion for Bill {BillId}",
                    billId);

                // Get the bill
                var bill = await _billRepository.GetAsync(billId);

                // Approve the bill
                bill.Approve(completedBy, ApprovalLevel.Level1, resolutionNote);

                await _billRepository.UpdateAsync(bill, autoSave: true);

                _logger.LogInformation(
                    "Bill {BillId} approved via task completion",
                    billId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error handling bill approval task completion");
                // Don't throw - this is a secondary operation
            }
        }
    }
}
