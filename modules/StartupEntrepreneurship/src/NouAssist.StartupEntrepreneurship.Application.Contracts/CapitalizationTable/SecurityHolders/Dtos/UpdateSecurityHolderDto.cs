using System.ComponentModel.DataAnnotations;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.Constants;

namespace NouAssist.StartupEntrepreneurship.CapitalizationTable.SecurityHolders.Dtos
{
    /// <summary>
    /// DTO for updating an existing SecurityHolder
    /// </summary>
    public class UpdateSecurityHolderDto
    {
        [Required]
        [StringLength(CapTableConsts.MaxHolderNameLength)]
        public string Name { get; set; }
        
        [EmailAddress]
        [StringLength(256)]
        public string Email { get; set; }
        
        [StringLength(50)]
        public string TaxId { get; set; }
        
        [StringLength(500)]
        public string Address { get; set; }
    }
}
