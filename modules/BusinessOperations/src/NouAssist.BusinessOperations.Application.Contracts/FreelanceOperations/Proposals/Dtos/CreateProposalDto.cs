using System;
using System.ComponentModel.DataAnnotations;
using NouAssist.BusinessOperations.FreelanceOperations.Constants;

namespace NouAssist.BusinessOperations.FreelanceOperations.Proposals.Dtos
{
    public class CreateProposalDto
    {
        [Required]
        [StringLength(FreelanceOperationsConsts.Proposal.MaxTitleLength)]
        public string Title { get; set; }

        [Required]
        public Guid ClientId { get; set; }

        public Guid? ProjectId { get; set; }

        [Required]
        public Guid TemplateId { get; set; }

        [Required]
        public string TemplateName { get; set; }

        public string TemplateVersion { get; set; } = "1.0";

        public DateTime? ExpirationDate { get; set; }
    }
}
