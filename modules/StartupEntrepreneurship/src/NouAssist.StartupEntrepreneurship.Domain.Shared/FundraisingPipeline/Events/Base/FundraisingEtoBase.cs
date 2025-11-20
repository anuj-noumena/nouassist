namespace NouAssist.StartupEntrepreneurship.FundraisingPipeline.Events.Base
{
    /// <summary>
    /// Base class for event transfer objects in the Fundraising Pipeline domain
    /// </summary>
    [Serializable]
    public abstract class FundraisingEtoBase
    {
        public Guid Id { get; set; }
        public Guid? TenantId { get; set; }
    }
}
