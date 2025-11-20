using Volo.Abp.Application.Dtos;

namespace NouAssist.RequirementsManagement.VersioningBaselining.Versioning.Dtos
{
    /// <summary>
    /// DTO representing a version record in the version history
    /// </summary>
    public class VersionRecordDto : EntityDto
    {
        /// <summary>
        /// Version identifier (e.g. "1.0.0")
        /// </summary>
        public string Version { get; set; }
        
        /// <summary>
        /// ID of the author who created this version
        /// </summary>
        public Guid AuthorId { get; set; }
        
        /// <summary>
        /// Name of the author
        /// </summary>
        public string AuthorName { get; set; }
        
        /// <summary>
        /// Comment describing the changes in this version
        /// </summary>
        public string ChangeComment { get; set; }
        
        /// <summary>
        /// Timestamp when this version was created
        /// </summary>
        public DateTime CreationTime { get; set; }
    }
}
