using System;
using System.ComponentModel.DataAnnotations;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.Constants;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.Enums;

namespace NouAssist.StartupEntrepreneurship.CapitalizationTable.SecurityHolders.Dtos
{
    /// <summary>
    /// DTO for creating a new SecurityHolder
    /// </summary>
    public class CreateSecurityHolderDto
    {
        [Required]
        [StringLength(CapTableConsts.MaxHolderNameLength)]
        public string Name { get; set; }
        
        [EmailAddress]
        [StringLength(256)]
        public string Email { get; set; }
        
        [Required]
        public HolderType HolderType { get; set; }
        
        [StringLength(50)]
        public string TaxId { get; set; }
        
        [StringLength(500)]
        public string Address { get; set; }
        
        public DateTime? DateJoined { get; set; }
        
        [StringLength(100)]
        public string ExternalReference { get; set; }
    }
}
