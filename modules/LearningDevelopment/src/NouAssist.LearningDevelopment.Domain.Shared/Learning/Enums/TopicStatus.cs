namespace NouAssist.Learning.Enums
{
    /// <summary>
    /// Represents the lifecycle status of a learning topic
    /// </summary>
    public enum TopicStatus : byte
    {
        Planned = 1,
        Active = 2,
        Paused = 3,
        Completed = 4,
        Archived = 5
    }
}
