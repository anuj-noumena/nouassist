using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.InvestorUpdates.Dtos
{
    /// <summary>
    /// DTO for sending an update to investors
    /// </summary>
    public class SendUpdateDto
    {
        [Required]
        [MinLength(1)]
        public List<Guid> InvestorIds { get; set; } = new();

        public DateTime? SendDate { get; set; }
        
        public bool SendImmediately { get; set; } = true;
    }
}
