namespace NouAssist.RequirementsManagement.Specification.Events.Base
{
    /// <summary>
    /// Base class for requirement-related events
    /// </summary>
    public abstract class RequirementEtoBase
    {
        public Guid Id { get; set; }
        public string Identifier { get; set; }
        public string Title { get; set; }
        public RequirementType Type { get; set; }
        public RequirementStatus Status { get; set; }
        public Guid? TenantId { get; set; }
        public DateTime EventTime { get; set; } = DateTime.UtcNow;
    }
}
