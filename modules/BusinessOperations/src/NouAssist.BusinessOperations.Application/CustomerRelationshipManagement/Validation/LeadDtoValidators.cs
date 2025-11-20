using FluentValidation;
using NouAssist.BusinessOperations.CustomerRelationshipManagement.Lead.Dtos;

namespace NouAssist.BusinessOperations.CustomerRelationshipManagement.Validation
{
    public class CreateLeadDtoValidator : AbstractValidator<CreateLeadDto>
    {
        public CreateLeadDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Lead name is required")
                .MaximumLength(CRMConsts.MaxLengths.Name).WithMessage($"Lead name cannot exceed {CRMConsts.MaxLengths.Name} characters");
            
            RuleFor(x => x.ContactInfo).NotNull().WithMessage("Contact information is required");
            RuleFor(x => x.Details).NotNull().WithMessage("Lead details are required");
            
            When(x => x.Tags != null && x.Tags.Count > 0, () => {
                RuleFor(x => x.Tags.Count)
                    .LessThanOrEqualTo(CRMConsts.Defaults.MaxTagsPerEntity)
                    .WithMessage($"Maximum {CRMConsts.Defaults.MaxTagsPerEntity} tags are allowed");
                
                RuleForEach(x => x.Tags)
                    .MaximumLength(CRMConsts.MaxLengths.TagName)
                    .WithMessage($"Tag cannot exceed {CRMConsts.MaxLengths.TagName} characters");
            });
        }
    }

    public class CreateContactInfoDtoValidator : AbstractValidator<CreateContactInfoDto>
    {
        public CreateContactInfoDtoValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("A valid email address is required")
                .MaximumLength(CRMConsts.MaxLengths.Email).WithMessage($"Email cannot exceed {CRMConsts.MaxLengths.Email} characters");
            
            RuleFor(x => x.Phone)
                .NotEmpty().WithMessage("Phone number is required")
                .MaximumLength(CRMConsts.MaxLengths.Phone).WithMessage($"Phone number cannot exceed {CRMConsts.MaxLengths.Phone} characters");
            
            RuleFor(x => x.AlternativePhone)
                .MaximumLength(CRMConsts.MaxLengths.Phone).WithMessage($"Alternative phone number cannot exceed {CRMConsts.MaxLengths.Phone} characters");
            
            RuleFor(x => x.Website)
                .MaximumLength(CRMConsts.MaxLengths.Url).WithMessage($"Website URL cannot exceed {CRMConsts.MaxLengths.Url} characters");
            
            RuleFor(x => x.Address)
                .MaximumLength(CRMConsts.MaxLengths.Address).WithMessage($"Address cannot exceed {CRMConsts.MaxLengths.Address} characters");
        }
    }

    public class CreateLeadDetailsDtoValidator : AbstractValidator<CreateLeadDetailsDto>
    {
        public CreateLeadDetailsDtoValidator()
        {
            RuleFor(x => x.JobTitle)
                .MaximumLength(CRMConsts.MaxLengths.Title).WithMessage($"Job title cannot exceed {CRMConsts.MaxLengths.Title} characters");
            
            RuleFor(x => x.CompanyName)
                .MaximumLength(CRMConsts.MaxLengths.Name).WithMessage($"Company name cannot exceed {CRMConsts.MaxLengths.Name} characters");
            
            RuleFor(x => x.Notes)
                .MaximumLength(CRMConsts.MaxLengths.Note).WithMessage($"Notes cannot exceed {CRMConsts.MaxLengths.Note} characters");
            
            RuleFor(x => x.EstimatedBudget)
                .GreaterThanOrEqualTo(0).When(x => x.EstimatedBudget.HasValue)
                .WithMessage("Estimated budget cannot be negative");
        }
    }

    public class UpdateLeadDtoValidator : AbstractValidator<UpdateLeadDto>
    {
        public UpdateLeadDtoValidator()
        {
            RuleFor(x => x.Name)
                .MaximumLength(CRMConsts.MaxLengths.Name)
                .When(x => !string.IsNullOrEmpty(x.Name))
                .WithMessage($"Lead name cannot exceed {CRMConsts.MaxLengths.Name} characters");
            
            RuleFor(x => x.Score)
                .GreaterThanOrEqualTo(0)
                .When(x => x.Score.HasValue)
                .WithMessage("Score cannot be negative");
        }
    }

    public class UpdateContactInfoDtoValidator : AbstractValidator<UpdateContactInfoDto>
    {
        public UpdateContactInfoDtoValidator()
        {
            RuleFor(x => x.Email)
                .EmailAddress().When(x => !string.IsNullOrEmpty(x.Email))
                .WithMessage("A valid email address is required")
                .MaximumLength(CRMConsts.MaxLengths.Email).When(x => !string.IsNullOrEmpty(x.Email))
                .WithMessage($"Email cannot exceed {CRMConsts.MaxLengths.Email} characters");
            
            RuleFor(x => x.Phone)
                .MaximumLength(CRMConsts.MaxLengths.Phone).When(x => !string.IsNullOrEmpty(x.Phone))
                .WithMessage($"Phone number cannot exceed {CRMConsts.MaxLengths.Phone} characters");
            
            RuleFor(x => x.AlternativePhone)
                .MaximumLength(CRMConsts.MaxLengths.Phone).When(x => !string.IsNullOrEmpty(x.AlternativePhone))
                .WithMessage($"Alternative phone number cannot exceed {CRMConsts.MaxLengths.Phone} characters");
            
            RuleFor(x => x.Website)
                .MaximumLength(CRMConsts.MaxLengths.Url).When(x => !string.IsNullOrEmpty(x.Website))
                .WithMessage($"Website URL cannot exceed {CRMConsts.MaxLengths.Url} characters");
            
            RuleFor(x => x.Address)
                .MaximumLength(CRMConsts.MaxLengths.Address).When(x => !string.IsNullOrEmpty(x.Address))
                .WithMessage($"Address cannot exceed {CRMConsts.MaxLengths.Address} characters");
        }
    }

    public class UpdateLeadDetailsDtoValidator : AbstractValidator<UpdateLeadDetailsDto>
    {
        public UpdateLeadDetailsDtoValidator()
        {
            RuleFor(x => x.JobTitle)
                .MaximumLength(CRMConsts.MaxLengths.Title).When(x => !string.IsNullOrEmpty(x.JobTitle))
                .WithMessage($"Job title cannot exceed {CRMConsts.MaxLengths.Title} characters");
            
            RuleFor(x => x.CompanyName)
                .MaximumLength(CRMConsts.MaxLengths.Name).When(x => !string.IsNullOrEmpty(x.CompanyName))
                .WithMessage($"Company name cannot exceed {CRMConsts.MaxLengths.Name} characters");
            
            RuleFor(x => x.Notes)
                .MaximumLength(CRMConsts.MaxLengths.Note).When(x => !string.IsNullOrEmpty(x.Notes))
                .WithMessage($"Notes cannot exceed {CRMConsts.MaxLengths.Note} characters");
            
            RuleFor(x => x.EstimatedBudget)
                .GreaterThanOrEqualTo(0).When(x => x.EstimatedBudget.HasValue)
                .WithMessage("Estimated budget cannot be negative");
        }
    }

    public class ChangeLeadStatusDtoValidator : AbstractValidator<ChangeLeadStatusDto>
    {
        public ChangeLeadStatusDtoValidator()
        {
            RuleFor(x => x.Status).IsInEnum().WithMessage("Invalid lead status");
        }
    }

    public class LeadTagsDtoValidator : AbstractValidator<LeadTagsDto>
    {
        public LeadTagsDtoValidator()
        {
            RuleFor(x => x.Tags).NotEmpty().WithMessage("At least one tag must be provided");
            
            RuleFor(x => x.Tags.Count)
                .LessThanOrEqualTo(CRMConsts.Defaults.MaxTagsPerEntity)
                .WithMessage($"Maximum {CRMConsts.Defaults.MaxTagsPerEntity} tags are allowed");
            
            RuleForEach(x => x.Tags)
                .NotEmpty().WithMessage("Tag cannot be empty")
                .MaximumLength(CRMConsts.MaxLengths.TagName)
                .WithMessage($"Tag cannot exceed {CRMConsts.MaxLengths.TagName} characters");
        }
    }

    public class ConvertLeadDtoValidator : AbstractValidator<ConvertLeadDto>
    {
        public ConvertLeadDtoValidator()
        {
            RuleFor(x => x.IsPrimaryContact)
                .Equal(false)
                .When(x => !x.OrganizationId.HasValue)
                .WithMessage("Cannot set as primary contact without an organization");
        }
    }
}
