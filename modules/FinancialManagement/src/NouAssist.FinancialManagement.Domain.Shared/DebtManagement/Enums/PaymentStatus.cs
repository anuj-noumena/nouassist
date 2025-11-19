namespace NouAssist.FinancialManagement.DebtManagement.Enums
{
    public enum PaymentStatus : byte
    {
        Scheduled = 1,
        Pending = 2,
        Completed = 3,
        Failed = 4,
        Cancelled = 5,
        Reversed = 6
    }
}
