using System;
using System.ComponentModel.DataAnnotations;

namespace NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.InvestorGroups.Dtos
{
    /// <summary>
    /// DTO for adding an investor to a group
    /// </summary>
    public class AddInvestorToGroupDto
    {
        [Required]
        public Guid InvestorId { get; set; }
    }
}
