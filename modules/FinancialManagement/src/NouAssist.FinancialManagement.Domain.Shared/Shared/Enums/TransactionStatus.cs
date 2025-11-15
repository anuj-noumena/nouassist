namespace NouAssist.FinancialManagement.Shared.Enums
{
    public enum TransactionStatus : byte
    {
        Pending = 1,
        Authorized = 2,
        Completed = 3,
        Failed = 4,
        Cancelled = 5,
        Reversed = 6
    }
}
