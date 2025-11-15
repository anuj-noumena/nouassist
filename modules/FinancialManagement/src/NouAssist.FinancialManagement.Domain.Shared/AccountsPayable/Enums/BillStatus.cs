namespace NouAssist.FinancialManagement.AccountsPayable.Enums
{
    public enum BillStatus : byte
    {
        Draft = 1,
        PendingApproval = 2,
        Approved = 3,
        Scheduled = 4,
        Paid = 5,
        Overdue = 6,
        Cancelled = 7
    }
}
