using System;
using System.ComponentModel.DataAnnotations;

namespace NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.InvestorGroups.Dtos
{
    /// <summary>
    /// DTO for removing an investor from a group
    /// </summary>
    public class RemoveInvestorFromGroupDto
    {
        [Required]
        public Guid InvestorId { get; set; }
    }
}
