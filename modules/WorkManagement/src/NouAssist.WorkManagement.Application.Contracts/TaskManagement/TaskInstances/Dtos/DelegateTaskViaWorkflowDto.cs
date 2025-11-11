using System;
using System.ComponentModel.DataAnnotations;

namespace NouAssist.WorkManagement.TaskManagement.TaskInstances.Dtos
{
    /// <summary>
    /// DTO for delegating a task via workflow transition
    /// </summary>
    public class DelegateTaskViaWorkflowDto
    {
        /// <summary>
        /// User to delegate the task to
        /// </summary>
        [Required]
        public Guid DelegatedTo { get; set; }

        /// <summary>
        /// Optional note explaining the delegation
        /// </summary>
        [StringLength(2000)]
        public string Note { get; set; }

        /// <summary>
        /// Optional comment for the workflow transition
        /// </summary>
        [StringLength(2000)]
        public string Comment { get; set; }

        /// <summary>
        /// Optional: Specific transition ID to use for delegation
        /// If not provided, the system will find the delegation transition automatically
        /// </summary>
        public Guid? TransitionId { get; set; }
    }
}
