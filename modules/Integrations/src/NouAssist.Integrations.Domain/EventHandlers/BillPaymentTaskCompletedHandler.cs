using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NouAssist.FinancialManagement.AccountsPayable.Aggregates;
using NouAssist.FinancialManagement.AccountsPayable.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Uow;

namespace NouAssist.FinancialManagement.Integration.EventHandlers
{
    /// <summary>
    /// Handles TaskCompletedEto for bill payment tasks to mark bill as paid/scheduled
    /// </summary>
    public class BillPaymentTaskCompletedHandler :
        IDistributedEventHandler<dynamic>,
        ITransientDependency
    {
        private readonly IBillRepository _billRepository;
        private readonly ILogger<BillPaymentTaskCompletedHandler> _logger;

        public BillPaymentTaskCompletedHandler(
            IBillRepository billRepository,
            ILogger<BillPaymentTaskCompletedHandler> logger)
        {
            _billRepository = billRepository;
            _logger = logger;
        }

        [UnitOfWork]
        public virtual async Task HandleEventAsync(dynamic eventData)
        {
            try
            {
                // Check if this is a bill payment task
                var customFields = eventData.CustomFields as System.Collections.Generic.Dictionary<string, string>;
                if (customFields == null ||
                    !customFields.ContainsKey("FinancialEntityType") ||
                    customFields["FinancialEntityType"] != "Bill" ||
                    !customFields.ContainsKey("TaskSubtype") ||
                    customFields["TaskSubtype"] != "Payment")
                {
                    return; // Not a bill payment task
                }

                if (!customFields.ContainsKey("BillId"))
                {
                    return; // Missing BillId
                }

                var billId = Guid.Parse(customFields["BillId"]);
                var resolutionNote = eventData.ResolutionNote as string;

                _logger.LogInformation(
                    "Processing bill payment task completion for Bill {BillId}",
                    billId);

                // Get the bill
                var bill = await _billRepository.GetAsync(billId);

                // Mark as scheduled for payment
                // In real implementation, you would call bill.SchedulePayment() or bill.MarkAsPaid()
                // For now, just log the completion
                _logger.LogInformation(
                    "Bill {BillId} payment task completed. Resolution: {Note}",
                    billId, resolutionNote);

                // You could update bill notes or add metadata
                // bill.AddNote($"Payment scheduled: {resolutionNote}");
                // await _billRepository.UpdateAsync(bill, autoSave: true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error handling bill payment task completion");
                // Don't throw - this is a secondary operation
            }
        }
    }
}
