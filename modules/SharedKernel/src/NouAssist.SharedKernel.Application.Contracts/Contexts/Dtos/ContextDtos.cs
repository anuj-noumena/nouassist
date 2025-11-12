using System;
using Volo.Abp.Application.Dtos;

namespace NouAssist.SharedKernel.Contexts.Dtos
{
    
    public class ContextDto : FullAuditedEntityDto<Guid>
    {
        public Guid OwnerId { get; set; }
        public string Title { get; set; }
        public string? Notes { get; set; }
        public string? Icon { get; set; }
        public string? Color { get; set; }
        public string? Location { get; set; }
        public int? EnergyLevel { get; set; }
        public bool IsActive { get; set; }
        public int UsageCount { get; set; }
        public DateTime? LastUsedAt { get; set; }
        public bool IsArchived { get; set; }
    }

    public class CreateContextDto
    {
        public string Title { get; set; }
        public string? Notes { get; set; }
        public string? Icon { get; set; }
        public string? Color { get; set; }
        public string? Location { get; set; }
        public int? EnergyLevel { get; set; }
    }
    
    public class UpdateContextDto
    {
        public string? Title { get; set; }
        public string? Notes { get; set; }
        public string? Icon { get; set; }
        public string? Color { get; set; }
        public string? Location { get; set; }
        public int? EnergyLevel { get; set; }
    }
}
