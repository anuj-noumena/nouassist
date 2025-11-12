using System;
using System.ComponentModel.DataAnnotations;

namespace NouAssist.WorkManagement.Workflow.Dtos
{
    public class UpdateWorkflowDto
    {
        [Required]
        [StringLength(256)]
        public string Name { get; set; }

        [StringLength(2000)]
        public string Description { get; set; }

        [Required]
        public Guid InitialStatusId { get; set; }

        public bool IsActive { get; set; }
    }
}
