using System;
using System.Collections.Generic;
using NouAssist.BusinessOperations.FreelanceOperations.Enums;

namespace NouAssist.BusinessOperations.FreelanceOperations.ClientPortal.Dtos
{
    public class ClientPortalProjectDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public ProjectStatus Status { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public double CompletionPercentage { get; set; }
        public List<ClientPortalMilestoneDto> Milestones { get; set; }
    }
}
