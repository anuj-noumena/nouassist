using FluentValidation;
using NouAssist.BusinessOperations.CustomerRelationshipManagement.Contact.Dtos;

namespace NouAssist.BusinessOperations.CustomerRelationshipManagement.Validation
{
    public class CreateContactDtoValidator : AbstractValidator<CreateContactDto>
    {
        public CreateContactDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Contact name is required")
                .MaximumLength(CRMConsts.MaxLengths.Name).WithMessage($"Contact name cannot exceed {CRMConsts.MaxLengths.Name} characters");
            
            RuleFor(x => x.ContactInfo).NotNull().WithMessage("Contact information is required");
            
            RuleFor(x => x.JobTitle)
                .MaximumLength(CRMConsts.MaxLengths.Title).WithMessage($"Job title cannot exceed {CRMConsts.MaxLengths.Title} characters");
            
            RuleFor(x => x.Notes)
                .MaximumLength(CRMConsts.MaxLengths.Note).WithMessage($"Notes cannot exceed {CRMConsts.MaxLengths.Note} characters");
            
            RuleFor(x => x.IsPrimaryContact)
                .Equal(false)
                .When(x => !x.OrganizationId.HasValue)
                .WithMessage("Cannot set as primary contact without an organization");
            
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

    public class UpdateContactDtoValidator : AbstractValidator<UpdateContactDto>
    {
        public UpdateContactDtoValidator()
        {
            RuleFor(x => x.Name)
                .MaximumLength(CRMConsts.MaxLengths.Name)
                .When(x => !string.IsNullOrEmpty(x.Name))
                .WithMessage($"Contact name cannot exceed {CRMConsts.MaxLengths.Name} characters");
            
            RuleFor(x => x.JobTitle)
                .MaximumLength(CRMConsts.MaxLengths.Title)
                .When(x => !string.IsNullOrEmpty(x.JobTitle))
                .WithMessage($"Job title cannot exceed {CRMConsts.MaxLengths.Title} characters");
            
            RuleFor(x => x.Notes)
                .MaximumLength(CRMConsts.MaxLengths.Note)
                .When(x => !string.IsNullOrEmpty(x.Notes))
                .WithMessage($"Notes cannot exceed {CRMConsts.MaxLengths.Note} characters");
        }
    }

    public class AssignToOrganizationDtoValidator : AbstractValidator<AssignToOrganizationDto>
    {
        public AssignToOrganizationDtoValidator()
        {
            RuleFor(x => x.OrganizationId)
                .NotEmpty().WithMessage("Organization ID is required");
        }
    }

    public class ContactTagsDtoValidator : AbstractValidator<ContactTagsDto>
    {
        public ContactTagsDtoValidator()
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

    public class ContactGroupsDtoValidator : AbstractValidator<ContactGroupsDto>
    {
        public ContactGroupsDtoValidator()
        {
            RuleFor(x => x.GroupIds).NotEmpty().WithMessage("At least one group ID must be provided");
        }
    }
}
