namespace NouAssist.FinancialManagement.AuditTrail.Enums
{
    public enum AuditEventType : byte
    {
        Create = 1,
        Update = 2,
        Delete = 3,
        Read = 4,
        Execute = 5,
        Approve = 6,
        Reject = 7,
        Post = 8,
        Cancel = 9
    }
}
