using Volo.Abp.Application.Dtos;
using NouAssist.RequirementsManagement.ChangeManagement.Enums;

namespace NouAssist.RequirementsManagement.ChangeManagement.ChangeRequest.Dtos
{
    /// <summary>
    /// Input DTO for filtering change requests
    /// </summary>
    public class GetChangeRequestListDto : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
        public RequestStatus? Status { get; set; }
        public DateTime? SubmissionDateMin { get; set; }
        public DateTime? SubmissionDateMax { get; set; }
        public Guid? SubmitterId { get; set; }
        public Guid? ApproverId { get; set; }
        public Guid? AffectedRequirementId { get; set; }
        public bool? PendingMyApproval { get; set; }
    }
}
