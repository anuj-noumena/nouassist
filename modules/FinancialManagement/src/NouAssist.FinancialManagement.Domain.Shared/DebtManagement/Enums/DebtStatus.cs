namespace NouAssist.FinancialManagement.DebtManagement.Enums
{
    public enum DebtStatus : byte
    {
        Active = 1,
        PaidOff = 2,
        InDefault = 3,
        Refinanced = 4,
        Consolidated = 5,
        WrittenOff = 6
    }
}
