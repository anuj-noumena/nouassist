namespace NouAssist.RequirementsManagement.Specification.Events
{
    /// <summary>
    /// Event published when a requirement status changes
    /// </summary>
    [EventName("NouAssist.RequirementsManagement.Specification.RequirementStatusChanged")]
    public class RequirementStatusChangedEto : RequirementEtoBase
    {
        public RequirementStatus PreviousStatus { get; set; }
        public RequirementStatus NewStatus { get; set; }
        public Guid ModifierId { get; set; }
        public string Reason { get; set; }
    }
}
