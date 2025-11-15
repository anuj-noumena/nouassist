namespace NouAssist.FinancialManagement.AccountsReceivable.Enums
{
    public enum InvoiceStatus : byte
    {
        Draft = 1,
        Sent = 2,
        Viewed = 3,
        PartiallyPaid = 4,
        Paid = 5,
        Overdue = 6,
        Cancelled = 7,
        Hold = 8
    }
}
