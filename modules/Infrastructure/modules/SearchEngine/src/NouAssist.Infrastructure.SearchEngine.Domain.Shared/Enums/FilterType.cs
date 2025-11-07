namespace NouAssist.Infrastructure.SearchEngine.Enums
{
    public enum FilterType : byte
    {
        Exact = 1,
        Contains = 2,
        DateRange = 3,
        NumericRange = 4,
        InList = 5,
        Boolean = 6
    }
}
