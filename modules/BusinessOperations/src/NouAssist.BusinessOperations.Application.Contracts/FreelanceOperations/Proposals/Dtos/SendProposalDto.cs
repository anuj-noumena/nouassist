using System.ComponentModel.DataAnnotations;

namespace NouAssist.BusinessOperations.FreelanceOperations.Proposals.Dtos
{
    public class SendProposalDto
    {
        [Required]
        [EmailAddress]
        public string SentToEmail { get; set; }
    }
}
