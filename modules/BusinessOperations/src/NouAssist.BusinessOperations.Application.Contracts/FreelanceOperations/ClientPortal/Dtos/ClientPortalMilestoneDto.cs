using System;

namespace NouAssist.BusinessOperations.FreelanceOperations.ClientPortal.Dtos
{
    public class ClientPortalMilestoneDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime? CompletedDate { get; set; }
        public DateTime? Deadline { get; set; }
        public bool IsOverdue { get; set; }
    }
}
