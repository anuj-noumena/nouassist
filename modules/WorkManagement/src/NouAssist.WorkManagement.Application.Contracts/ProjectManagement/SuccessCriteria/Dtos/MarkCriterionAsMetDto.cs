using System.ComponentModel.DataAnnotations;

namespace NouAssist.WorkManagement.ProjectManagement.SuccessCriteria.Dtos
{
    public class MarkCriterionAsMetDto
    {
        [StringLength(5000)]
        public string Notes { get; set; }

        [StringLength(2000)]
        public string EvidenceUrl { get; set; }
    }
}
