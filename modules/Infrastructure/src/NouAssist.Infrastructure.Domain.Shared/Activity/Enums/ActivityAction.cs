namespace NouAssist.Infrastructure.Activity.Enums
{
    public enum ActivityAction : byte
    {
        Created = 1,
        Updated = 2,
        Deleted = 3,
        Completed = 4,
        Archived = 5,
        Restored = 6,
        Assigned = 7,
        Unassigned = 8,
        Commented = 9,
        Mentioned = 10,
        Approved = 11,
        Rejected = 12,
        Shared = 13,
        Moved = 14,
        Duplicated = 15,
        Attached = 16,
        Detached = 17,
        Tagged = 18,
        Untagged = 19,
        Starred = 20,
        Unstarred = 21,
        Locked = 22,
        Unlocked = 23,
        Exported = 24,
        Imported = 25
    }
}
