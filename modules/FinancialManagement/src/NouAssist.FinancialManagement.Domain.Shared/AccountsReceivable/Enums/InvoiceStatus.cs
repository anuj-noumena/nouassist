namespace NouAssist.FinancialManagement.AccountsReceivable.Enums
{
    public enum InvoiceStatus : byte
    {
        Draft = 1,
        Sent = 2,
        Paid = 3,
        PartiallyPaid = 4,
        Overdue = 5,
        Cancelled = 6,
        Hold = 7
    }
}
