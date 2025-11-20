using System;

namespace NouAssist.FinancialManagement.Integration.FinancialTasks.Dtos
{
    public class PendingBillApprovalDto
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
        public string Priority { get; set; }
        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; }
        public Guid? ProjectId { get; set; }
        public string ProjectName { get; set; }
    }
}
