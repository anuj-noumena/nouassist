namespace NouAssist.Infrastructure.Activity.Enums
{
    public enum AuditCategory : byte
    {
        Authentication = 1,
        Authorization = 2,
        DataAccess = 3,
        DataModification = 4,
        Configuration = 5,
        Security = 6,
        Compliance = 7,
        System = 8
    }
}
