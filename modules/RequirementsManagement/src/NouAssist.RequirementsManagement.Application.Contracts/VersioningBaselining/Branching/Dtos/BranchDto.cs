using NouAssist.RequirementsManagement.VersioningBaselining.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.RequirementsManagement.VersioningBaselining.Branching.Dtos
{
    /// <summary>
    /// DTO representing a branch
    /// </summary>
    public class BranchDto : AuditedEntityDto<Guid>
    {
        /// <summary>
        /// Name of the branch
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// Description of the branch
        /// </summary>
        public string Description { get; set; }
        
        /// <summary>
        /// Status of the branch
        /// </summary>
        public BranchStatus Status { get; set; }
        
        /// <summary>
        /// ID of the source baseline or branch
        /// </summary>
        public Guid SourceId { get; set; }
        
        /// <summary>
        /// Type of the source ("Baseline" or "Branch")
        /// </summary>
        public string SourceType { get; set; }
        
        /// <summary>
        /// ID of the user who created the branch
        /// </summary>
        public Guid CreatedById { get; set; }
        
        /// <summary>
        /// Name of the user who created the branch
        /// </summary>
        public string CreatedByName { get; set; }
        
        /// <summary>
        /// Collection of merge operations involving this branch
        /// </summary>
        public List<MergeOperationDto> MergeOperations { get; set; }
    }
}
