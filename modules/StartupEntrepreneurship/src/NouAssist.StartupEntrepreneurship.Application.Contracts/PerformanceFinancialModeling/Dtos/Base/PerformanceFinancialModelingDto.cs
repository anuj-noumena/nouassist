namespace NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Dtos.Base
{
    /// <summary>
    /// Base DTO for all module DTOs
    /// </summary>
    public abstract class PerformanceFinancialModelingDto
    {
        public Guid Id { get; set; }
    }

    /// <summary>
    /// Base DTO for audited entities
    /// </summary>
    public abstract class PerformanceFinancialModelingAuditedDto : PerformanceFinancialModelingDto
    {
        public DateTime CreationTime { get; set; }
        public Guid? CreatorId { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public Guid? LastModifierId { get; set; }
    }

    /// <summary>
    /// Base DTO for full audited entities
    /// </summary>
    public abstract class PerformanceFinancialModelingFullAuditedDto : PerformanceFinancialModelingAuditedDto
    {
        public bool IsDeleted { get; set; }
        public Guid? DeleterId { get; set; }
        public DateTime? DeletionTime { get; set; }
    }
}
