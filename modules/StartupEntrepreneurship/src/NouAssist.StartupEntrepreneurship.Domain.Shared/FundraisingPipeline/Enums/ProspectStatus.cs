namespace NouAssist.StartupEntrepreneurship.FundraisingPipeline.Enums
{
    /// <summary>
    /// Represents the current status of a prospect in the fundraising pipeline
    /// </summary>
    public enum ProspectStatus
    {
        New = 0,
        Researching = 10,
        Contacted = 20,
        Engaged = 30,
        Pitched = 40,
        InDueDiligence = 50,
        TermSheetReceived = 60,
        Negotiating = 70,
        Committed = 80,
        Closed = 90,
        Declined = 100
    }
}
