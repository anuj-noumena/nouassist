namespace NouAssist.SharedKernel.Enums
{
    public enum GoalStatus : byte
    {
        Active = 1,
        OnHold = 2,
        Achieved = 3,
        Archived = 4
    }

    public enum GoalProgressLevel : byte
    {
        NotStarted = 0,
        JustStarted = 1,
        Progressing = 2,
        WellUnderway = 3,
        NearlyComplete = 4,
        Completed = 5
    }

    public enum InboxItemType : byte
    {
        Task = 1,
        Idea = 2,
        Note = 3,
        Reference = 4,
        Contact = 5,
        Link = 6,
        Other = 7
    }

    public enum InboxItemSource : byte
    {
        Manual = 1,
        Email = 2,
        Web = 3,
        Mobile = 4,
        Voice = 5,
        Other = 6
    }

    public enum AchievementCategory : byte
    {
        Tasks = 1,
        Projects = 2,
        Goals = 3,
        Habits = 4,
        Productivity = 5,
        Consistency = 6,
        Social = 7,
        Special = 8
    }

    public enum AchievementRarity : byte
    {
        Common = 1,
        Uncommon = 2,
        Rare = 3,
        Epic = 4,
        Legendary = 5
    }

    public enum CustomerType : byte
    {
        Standard = 1,
        Premium = 2
    }
}
