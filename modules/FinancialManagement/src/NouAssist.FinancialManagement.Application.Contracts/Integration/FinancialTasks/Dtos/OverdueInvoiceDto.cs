using System;

namespace NouAssist.FinancialManagement.Integration.FinancialTasks.Dtos
{
    public class OverdueInvoiceDto
    {
        public Guid InvoiceId { get; set; }
        public Guid? CollectionTaskId { get; set; }
        public string InvoiceNumber { get; set; }
        public string CustomerName { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal AmountDue { get; set; }
        public string Currency { get; set; }
        public DateTime DueDate { get; set; }
        public int DaysOverdue { get; set; }
        public DateTime? LastContactDate { get; set; }
        public string CollectionPriority { get; set; }
        public int CollectionAttempts { get; set; }
    }
}
