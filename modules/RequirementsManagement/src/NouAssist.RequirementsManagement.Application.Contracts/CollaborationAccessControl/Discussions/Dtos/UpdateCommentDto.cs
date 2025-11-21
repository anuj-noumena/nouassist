using System.ComponentModel.DataAnnotations;
using NouAssist.RequirementsManagement.CollaborationAccessControl.Constants;

namespace NouAssist.RequirementsManagement.CollaborationAccessControl.Discussions.Dtos
{
    public class UpdateCommentDto
    {
        [Required]
        [StringLength(CollaborationConsts.MaxCommentContentLength)]
        public string Content { get; set; }
    }
}
