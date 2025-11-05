namespace NouAssist.Approval.Enums
{
    public enum ApprovalStatus : byte
    {
        Draft = 1,
        Pending = 2,
        InProgress = 3,
        Approved = 4,
        Rejected = 5,
        Cancelled = 6,
        Expired = 7
    }
}
