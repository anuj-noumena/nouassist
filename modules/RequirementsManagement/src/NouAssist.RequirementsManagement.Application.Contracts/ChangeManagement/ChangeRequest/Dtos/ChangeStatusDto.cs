using System.ComponentModel.DataAnnotations;
using NouAssist.RequirementsManagement.ChangeManagement.Enums;

namespace NouAssist.RequirementsManagement.ChangeManagement.ChangeRequest.Dtos
{
    /// <summary>
    /// Input DTO for changing a request status
    /// </summary>
    public class ChangeStatusDto
    {
        [Required]
        public RequestStatus TargetStatus { get; set; }

        [StringLength(500)]
        public string Reason { get; set; }
    }
}
