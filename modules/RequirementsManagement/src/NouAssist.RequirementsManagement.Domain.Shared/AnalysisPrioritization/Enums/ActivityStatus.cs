namespace NouAssist.RequirementsManagement.AnalysisPrioritization.Enums
{
    /// <summary>
    /// Status of prioritization activities
    /// </summary>
    public enum ActivityStatus : byte
    {
        Draft = 1,
        In_Progress = 2,
        Voting = 3,
        Completed = 4,
        Cancelled = 5
    }
}
