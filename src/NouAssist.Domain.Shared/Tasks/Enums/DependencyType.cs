namespace NouAssist.Tasks.Enums
{
    public enum DependencyType : byte
    {
        FinishToStart = 1,
        StartToStart = 2,
        FinishToFinish = 3,
        StartToFinish = 4,
        Related = 5
    }
}
