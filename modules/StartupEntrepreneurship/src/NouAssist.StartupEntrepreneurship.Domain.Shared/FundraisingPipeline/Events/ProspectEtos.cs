namespace NouAssist.StartupEntrepreneurship.FundraisingPipeline.Events
{
    /// <summary>
    /// Event raised when a new prospect is created
    /// </summary>
    [EventName("NouAssist.StartupEntrepreneurship.FundraisingPipeline.ProspectCreated")]
    public class ProspectCreatedEto : FundraisingEtoBase
    {
        public string Name { get; set; }
        public string Company { get; set; }
        public ProspectStatus Status { get; set; }
    }

    /// <summary>
    /// Event raised when a prospect's status changes
    /// </summary>
    [EventName("NouAssist.StartupEntrepreneurship.FundraisingPipeline.ProspectStatusChanged")]
    public class ProspectStatusChangedEto : FundraisingEtoBase
    {
        public ProspectStatus OldStatus { get; set; }
        public ProspectStatus NewStatus { get; set; }
    }
}
