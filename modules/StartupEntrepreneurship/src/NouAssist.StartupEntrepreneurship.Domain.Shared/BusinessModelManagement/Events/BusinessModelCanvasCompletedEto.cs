namespace NouAssist.StartupEntrepreneurship.BusinessModelManagement.Events
{
    [EventName("NouAssist.StartupEntrepreneurship.BusinessModelManagement.BusinessModelCanvasCompleted")]
    public class BusinessModelCanvasCompletedEto : BusinessModelManagementEtoBase
    {
        public double ValidationProgress { get; set; }
        public DateTime CompletedAt { get; set; }
    }
}
