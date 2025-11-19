namespace NouAssist.FinancialManagement.CurrencyManagement.Enums
{
    public enum ExchangeRateProvider : byte
    {
        Manual = 1,
        CentralBank = 2,
        OpenExchangeRates = 3,
        ECB = 4,             // European Central Bank
        IMF = 5,             // International Monetary Fund
        Custom = 99
    }
}
