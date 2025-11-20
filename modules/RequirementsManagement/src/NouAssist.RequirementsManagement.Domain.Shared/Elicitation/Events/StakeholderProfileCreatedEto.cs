namespace NouAssist.RequirementsManagement.Elicitation.Events
{
    /// <summary>
    /// Event published when a stakeholder profile is created
    /// </summary>
    [EventName("NouAssist.RequirementsManagement.Elicitation.StakeholderProfileCreated")]
    public class StakeholderProfileCreatedEto : ElicitationEtoBase
    {
        public string Name { get; set; }
        public StakeholderRole Role { get; set; }
        public string Organization { get; set; }
        public string Email { get; set; }
    }
}
