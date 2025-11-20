using System;
using System.ComponentModel.DataAnnotations;
using NouAssist.BusinessOperations.FreelanceOperations.Constants;

namespace NouAssist.BusinessOperations.FreelanceOperations.Proposals.Dtos
{
    public class UpdateProposalDto
    {
        [Required]
        [StringLength(FreelanceOperationsConsts.Proposal.MaxTitleLength)]
        public string Title { get; set; }

        [Required]
        public Guid TemplateId { get; set; }

        [Required]
        public string TemplateName { get; set; }

        public string TemplateVersion { get; set; }

        public DateTime? ExpirationDate { get; set; }
    }
}
