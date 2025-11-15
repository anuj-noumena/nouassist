namespace NouAssist.DebtManagement.Enums
{
    public enum PaymentStrategy : byte
    {
        HighestInterestFirst = 1, // Avalanche
        LowestBalanceFirst = 2,   // Snowball
        HighestBalanceFirst = 3,
        MinimumPaymentsOnly = 4,
        Custom = 5
    }
}
