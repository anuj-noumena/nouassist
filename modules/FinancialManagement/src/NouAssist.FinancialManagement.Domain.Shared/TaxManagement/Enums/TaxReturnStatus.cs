namespace NouAssist.FinancialManagement.TaxManagement.Enums
{
    public enum TaxReturnStatus : byte
    {
        Draft = 1,
        ReadyToFile = 2,
        Filed = 3,
        Accepted = 4,
        Rejected = 5,
        Amended = 6
    }
}
