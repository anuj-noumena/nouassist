namespace NouAssist.FinancialManagement.AccountsPayable.Enums
{
    public enum BillStatus : byte
    {
        Draft = 1,
        PendingApproval = 2,
        Approved = 3,
        Paid = 4,
        Overdue = 5,
        Cancelled = 6
    }
}
