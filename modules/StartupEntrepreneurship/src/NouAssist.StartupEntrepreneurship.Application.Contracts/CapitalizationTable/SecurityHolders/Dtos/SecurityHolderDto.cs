using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace NouAssist.StartupEntrepreneurship.CapitalizationTable.SecurityHolders.Dtos
{
    /// <summary>
    /// DTO for SecurityHolder view operations
    /// </summary>
    public class SecurityHolderDto : FullAuditedEntityDto<Guid>
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string HolderType { get; set; }
        public string TaxId { get; set; }
        public string Address { get; set; }
        public bool IsActive { get; set; }
        public DateTime? DateJoined { get; set; }
        public string ExternalReference { get; set; }
        
        // Calculated fields
        public int VestingScheduleCount { get; set; }
    }
}
