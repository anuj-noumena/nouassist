using System;
using NouAssist.BusinessOperations.FreelanceOperations.ValueObjects;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace NouAssist.BusinessOperations.FreelanceOperations.Entities
{
    /// <summary>
    /// Entity representing a milestone for a project
    /// </summary>
    public class ProjectMilestone : Entity<Guid>
    {
        /// <summary>
        /// ID of the project this milestone belongs to
        /// </summary>
        public Guid ProjectId { get; private set; }
        
        /// <summary>
        /// Details of the milestone
        /// </summary>
        public MilestoneDetails Details { get; private set; }
        
        /// <summary>
        /// Deadline for the milestone
        /// </summary>
        public DateTime? Deadline { get; private set; }
        
        /// <summary>
        /// Whether the milestone is completed
        /// </summary>
        public bool IsCompleted { get; private set; }
        
        /// <summary>
        /// Date when the milestone was completed
        /// </summary>
        public DateTime? CompletedDate { get; private set; }
        
        /// <summary>
        /// Whether the milestone has been invoiced
        /// </summary>
        public bool IsInvoiced { get; private set; }
        
        /// <summary>
        /// Invoice ID if this milestone has been invoiced
        /// </summary>
        public Guid? InvoiceId { get; private set; }

        protected ProjectMilestone()
        {
        }

        internal ProjectMilestone(
            Guid id,
            Guid projectId,
            MilestoneDetails details,
            DateTime? deadline = null)
            : base(id)
        {
            ProjectId = projectId;
            Details = details ?? throw new ArgumentNullException(nameof(details));
            Deadline = deadline;
            IsCompleted = false;
            IsInvoiced = false;
        }

        /// <summary>
        /// Update the details of this milestone
        /// </summary>
        internal void UpdateDetails(MilestoneDetails details)
        {
            if (IsInvoiced)
            {
                throw new BusinessException("FreelanceOperations:ErrorMessage:InvalidOperation");
            }
            
            Details = details ?? throw new ArgumentNullException(nameof(details));
        }

        /// <summary>
        /// Update the deadline of this milestone
        /// </summary>
        internal void UpdateDeadline(DateTime? deadline)
        {
            Deadline = deadline;
        }

        /// <summary>
        /// Mark this milestone as completed
        /// </summary>
        internal void MarkAsCompleted(DateTime completedDate)
        {
            if (IsCompleted)
            {
                return; // Already completed
            }
            
            IsCompleted = true;
            CompletedDate = completedDate;
        }

        /// <summary>
        /// Mark this milestone as not completed (e.g., if additional work is needed)
        /// </summary>
        internal void MarkAsIncomplete()
        {
            if (IsInvoiced)
            {
                throw new BusinessException("FreelanceOperations:ErrorMessage:InvalidOperation");
            }
            
            IsCompleted = false;
            CompletedDate = null;
        }

        /// <summary>
        /// Mark this milestone as invoiced
        /// </summary>
        internal void MarkAsInvoiced(Guid invoiceId)
        {
            if (!IsCompleted)
            {
                throw new BusinessException("FreelanceOperations:ErrorMessage:InvalidOperation");
            }
            
            if (IsInvoiced)
            {
                return; // Already invoiced
            }
            
            IsInvoiced = true;
            InvoiceId = invoiceId;
        }

        /// <summary>
        /// Mark this milestone as not invoiced (e.g., if invoice is canceled)
        /// </summary>
        internal void MarkAsNotInvoiced()
        {
            IsInvoiced = false;
            InvoiceId = null;
        }

        /// <summary>
        /// Check if the milestone is overdue
        /// </summary>
        public bool IsOverdue()
        {
            return !IsCompleted && Deadline.HasValue && DateTime.UtcNow > Deadline.Value;
        }
    }
}
