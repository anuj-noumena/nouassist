using System;
using FluentValidation;
using NouAssist.StartupEntrepreneurship.FundraisingPipeline.Constants;
using NouAssist.StartupEntrepreneurship.FundraisingPipeline.Prospect.Dtos;

namespace NouAssist.StartupEntrepreneurship.FundraisingPipeline.Validation
{
    public class CreateProspectDtoValidator : AbstractValidator<CreateProspectDto>
    {
        public CreateProspectDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(FundraisingPipelineConsts.Prospect.MaxNameLength);
            
            RuleFor(x => x.Company)
                .NotEmpty()
                .MaximumLength(FundraisingPipelineConsts.Prospect.MaxCompanyLength);
            
            RuleFor(x => x.Title)
                .NotEmpty()
                .MaximumLength(FundraisingPipelineConsts.Prospect.MaxTitleLength);
            
            RuleFor(x => x.Description)
                .MaximumLength(FundraisingPipelineConsts.Prospect.MaxDescriptionLength);
            
            RuleFor(x => x.ContactInfo).NotNull();
            
            When(x => x.ContactInfo != null, () =>
            {
                RuleFor(x => x.ContactInfo.Email)
                    .NotEmpty()
                    .EmailAddress()
                    .MaximumLength(FundraisingPipelineConsts.Contact.MaxEmailLength);
                
                RuleFor(x => x.ContactInfo.Phone)
                    .MaximumLength(FundraisingPipelineConsts.Contact.MaxPhoneLength);
                
                RuleFor(x => x.ContactInfo.Address)
                    .MaximumLength(FundraisingPipelineConsts.Contact.MaxAddressLength);
            });
        }
    }

    public class UpdateProspectDtoValidator : AbstractValidator<UpdateProspectDto>
    {
        public UpdateProspectDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(FundraisingPipelineConsts.Prospect.MaxNameLength);
            
            RuleFor(x => x.Company)
                .NotEmpty()
                .MaximumLength(FundraisingPipelineConsts.Prospect.MaxCompanyLength);
            
            RuleFor(x => x.Title)
                .NotEmpty()
                .MaximumLength(FundraisingPipelineConsts.Prospect.MaxTitleLength);
            
            RuleFor(x => x.Description)
                .MaximumLength(FundraisingPipelineConsts.Prospect.MaxDescriptionLength);
            
            RuleFor(x => x.ContactInfo).NotNull();
            
            When(x => x.ContactInfo != null, () =>
            {
                RuleFor(x => x.ContactInfo.Email)
                    .NotEmpty()
                    .EmailAddress()
                    .MaximumLength(FundraisingPipelineConsts.Contact.MaxEmailLength);
                
                RuleFor(x => x.ContactInfo.Phone)
                    .MaximumLength(FundraisingPipelineConsts.Contact.MaxPhoneLength);
                
                RuleFor(x => x.ContactInfo.Address)
                    .MaximumLength(FundraisingPipelineConsts.Contact.MaxAddressLength);
            });
        }
    }

    public class CreateInteractionDtoValidator : AbstractValidator<CreateInteractionDto>
    {
        public CreateInteractionDtoValidator()
        {
            RuleFor(x => x.Summary)
                .NotEmpty()
                .MaximumLength(500);
            
            RuleFor(x => x.InteractionDate)
                .NotEmpty()
                .LessThanOrEqualTo(DateTime.Now);
            
            RuleFor(x => x.Notes)
                .MaximumLength(2000);
        }
    }
}
