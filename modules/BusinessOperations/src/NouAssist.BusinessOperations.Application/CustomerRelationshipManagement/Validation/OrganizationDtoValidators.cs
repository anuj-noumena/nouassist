using FluentValidation;
using NouAssist.BusinessOperations.CustomerRelationshipManagement.Organization.Dtos;

namespace NouAssist.BusinessOperations.CustomerRelationshipManagement.Validation
{
    public class CreateOrganizationDtoValidator : AbstractValidator<CreateOrganizationDto>
    {
        public CreateOrganizationDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Organization name is required")
                .MaximumLength(CRMConsts.MaxLengths.Name).WithMessage($"Organization name cannot exceed {CRMConsts.MaxLengths.Name} characters");
            
            RuleFor(x => x.ContactInfo).NotNull().WithMessage("Contact information is required");
            RuleFor(x => x.Details).NotNull().WithMessage("Organization details are required");
            
            RuleFor(x => x.RegistrationNumber)
                .MaximumLength(50).WithMessage("Registration number cannot exceed 50 characters");
            
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

    public class CreateOrganizationInfoDtoValidator : AbstractValidator<CreateOrganizationInfoDto>
    {
        public CreateOrganizationInfoDtoValidator()
        {
            RuleFor(x => x.Industry)
                .MaximumLength(100).WithMessage("Industry cannot exceed 100 characters");
            
            RuleFor(x => x.EmployeeCount)
                .GreaterThan(0).When(x => x.EmployeeCount.HasValue)
                .WithMessage("Employee count must be greater than 0");
            
            RuleFor(x => x.AnnualRevenue)
                .GreaterThanOrEqualTo(0).When(x => x.AnnualRevenue.HasValue)
                .WithMessage("Annual revenue cannot be negative");
            
            RuleFor(x => x.TaxId)
                .MaximumLength(50).WithMessage("Tax ID cannot exceed 50 characters");
            
            RuleFor(x => x.Notes)
                .MaximumLength(CRMConsts.MaxLengths.Note).WithMessage($"Notes cannot exceed {CRMConsts.MaxLengths.Note} characters");
        }
    }

    public class UpdateOrganizationDtoValidator : AbstractValidator<UpdateOrganizationDto>
    {
        public UpdateOrganizationDtoValidator()
        {
            RuleFor(x => x.Name)
                .MaximumLength(CRMConsts.MaxLengths.Name)
                .When(x => !string.IsNullOrEmpty(x.Name))
                .WithMessage($"Organization name cannot exceed {CRMConsts.MaxLengths.Name} characters");
            
            RuleFor(x => x.RegistrationNumber)
                .MaximumLength(50)
                .When(x => !string.IsNullOrEmpty(x.RegistrationNumber))
                .WithMessage("Registration number cannot exceed 50 characters");
        }
    }

    public class UpdateOrganizationInfoDtoValidator : AbstractValidator<UpdateOrganizationInfoDto>
    {
        public UpdateOrganizationInfoDtoValidator()
        {
            RuleFor(x => x.Industry)
                .MaximumLength(100)
                .When(x => !string.IsNullOrEmpty(x.Industry))
                .WithMessage("Industry cannot exceed 100 characters");
            
            RuleFor(x => x.EmployeeCount)
                .GreaterThan(0).When(x => x.EmployeeCount.HasValue)
                .WithMessage("Employee count must be greater than 0");
            
            RuleFor(x => x.AnnualRevenue)
                .GreaterThanOrEqualTo(0).When(x => x.AnnualRevenue.HasValue)
                .WithMessage("Annual revenue cannot be negative");
            
            RuleFor(x => x.TaxId)
                .MaximumLength(50)
                .When(x => !string.IsNullOrEmpty(x.TaxId))
                .WithMessage("Tax ID cannot exceed 50 characters");
            
            RuleFor(x => x.Notes)
                .MaximumLength(CRMConsts.MaxLengths.Note)
                .When(x => !string.IsNullOrEmpty(x.Notes))
                .WithMessage($"Notes cannot exceed {CRMConsts.MaxLengths.Note} characters");
        }
    }

    public class SetParentOrganizationDtoValidator : AbstractValidator<SetParentOrganizationDto>
    {
        public SetParentOrganizationDtoValidator()
        {
            RuleFor(x => x.ParentId).NotEmpty().WithMessage("Parent organization ID is required");
        }
    }

    public class OrganizationTagsDtoValidator : AbstractValidator<OrganizationTagsDto>
    {
        public OrganizationTagsDtoValidator()
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
}
