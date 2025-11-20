using System;
using NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.Enums;

namespace NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.InvestorGroups.Dtos
{
    /// <summary>
    /// DTO representing a member of an investor group
    /// </summary>
    public class InvestorGroupMemberDto
    {
        public Guid InvestorId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public InvestorType Type { get; set; }
        public InvestorStatus Status { get; set; }
        public string CompanyName { get; set; }
        public decimal TotalInvestment { get; set; }
        public int RelationshipScore { get; set; }
    }
}
