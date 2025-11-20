namespace NouAssist.StartupEntrepreneurship.FundraisingPipeline.Events
{
    /// <summary>
    /// Event raised when a new fundraising round is created
    /// </summary>
    [EventName("NouAssist.StartupEntrepreneurship.FundraisingPipeline.FundraisingRoundCreated")]
    public class FundraisingRoundCreatedEto : FundraisingEtoBase
    {
        public string Name { get; set; }
        public RoundType Type { get; set; }
        public RoundStage Stage { get; set; }
        public decimal TargetAmount { get; set; }
    }

    /// <summary>
    /// Event raised when a fundraising round is closed
    /// </summary>
    [EventName("NouAssist.StartupEntrepreneurship.FundraisingPipeline.FundraisingRoundClosed")]
    public class FundraisingRoundClosedEto : FundraisingEtoBase
    {
        public string Name { get; set; }
        public decimal RaisedAmount { get; set; }
        public int InvestorCount { get; set; }
        public DateTime ClosedDate { get; set; }
    }
}
