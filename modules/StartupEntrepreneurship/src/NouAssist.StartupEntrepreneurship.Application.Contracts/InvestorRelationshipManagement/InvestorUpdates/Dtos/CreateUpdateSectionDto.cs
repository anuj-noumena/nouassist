using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.InvestorUpdates.Dtos
{
    /// <summary>
    /// DTO for creating a new update section
    /// </summary>
    public class CreateUpdateSectionDto
    {
        [Required]
        [StringLength(300)]
        public string Title { get; set; }

        [Required]
        [StringLength(10000)]
        public string Content { get; set; }

        [StringLength(50)]
        public string ContentType { get; set; } = "text/html";

        public Dictionary<string, object> ContentMetadata { get; set; } = new();

        public int? Order { get; set; }
    }
}
