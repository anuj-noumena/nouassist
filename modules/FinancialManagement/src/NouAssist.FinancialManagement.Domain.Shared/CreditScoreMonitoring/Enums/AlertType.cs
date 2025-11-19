namespace NouAssist.FinancialManagement.CreditScoreMonitoring.Enums
{
    public enum AlertType : byte
    {
        ScoreIncrease = 1,
        ScoreDecrease = 2,
        NewAccount = 3,
        NewInquiry = 4,
        UtilizationIncrease = 5,
        DelinquentAccount = 6,
        CollectionAccount = 7,
        PublicRecord = 8,
        AccountClosed = 9,
        CreditLimitChange = 10
    }
}
