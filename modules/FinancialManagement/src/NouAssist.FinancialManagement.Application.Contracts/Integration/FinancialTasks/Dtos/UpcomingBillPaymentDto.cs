using System;

namespace NouAssist.FinancialManagement.Integration.FinancialTasks.Dtos
{
    public class UpcomingBillPaymentDto
    {
        public Guid BillId { get; set; }
        public Guid? TaskInstanceId { get; set; }
        public string BillNumber { get; set; }
        public string VendorName { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime TaskDueDate { get; set; }
        public int DaysUntilDue { get; set; }
        public string Status { get; set; }
        public bool IsApproved { get; set; }
        public Guid? PaymentAccountId { get; set; }
    }
}
