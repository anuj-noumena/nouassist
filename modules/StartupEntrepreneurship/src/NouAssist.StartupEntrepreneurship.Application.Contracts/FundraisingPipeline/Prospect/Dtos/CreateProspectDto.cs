using System;
using System.ComponentModel.DataAnnotations;
using NouAssist.StartupEntrepreneurship.FundraisingPipeline.Constants;
using NouAssist.StartupEntrepreneurship.FundraisingPipeline.Enums;

namespace NouAssist.StartupEntrepreneurship.FundraisingPipeline.Prospect.Dtos
{
    public class CreateProspectDto
    {
        [Required]
        [StringLength(FundraisingPipelineConsts.Prospect.MaxNameLength)]
        public string Name { get; set; }

        [Required]
        [StringLength(FundraisingPipelineConsts.Prospect.MaxCompanyLength)]
        public string Company { get; set; }

        [Required]
        [StringLength(FundraisingPipelineConsts.Prospect.MaxTitleLength)]
        public string Title { get; set; }

        [StringLength(FundraisingPipelineConsts.Prospect.MaxDescriptionLength)]
        public string Description { get; set; }

        [Required]
        public CreateContactInfoDto ContactInfo { get; set; }

        public ProspectStatus? InitialStatus { get; set; }

        public Guid? FundraisingRoundId { get; set; }
    }

    public class CreateContactInfoDto
    {
        [Required]
        [EmailAddress]
        [StringLength(FundraisingPipelineConsts.Contact.MaxEmailLength)]
        public string Email { get; set; }

        [StringLength(FundraisingPipelineConsts.Contact.MaxPhoneLength)]
        public string Phone { get; set; }

        [StringLength(FundraisingPipelineConsts.Contact.MaxAddressLength)]
        public string Address { get; set; }
    }
}
