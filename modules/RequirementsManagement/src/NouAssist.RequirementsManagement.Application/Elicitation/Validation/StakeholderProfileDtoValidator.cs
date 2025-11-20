using FluentValidation;
using NouAssist.RequirementsManagement.Elicitation.Constants;
using NouAssist.RequirementsManagement.Elicitation.StakeholderProfiles.Dtos;

namespace NouAssist.RequirementsManagement.Elicitation.Validation
{
    public class CreateStakeholderProfileDtoValidator : AbstractValidator<CreateStakeholderProfileDto>
    {
        public CreateStakeholderProfileDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required")
                .MaximumLength(ElicitationConsts.MaxStakeholderNameLength);

            RuleFor(x => x.Description)
                .MaximumLength(ElicitationConsts.MaxStakeholderDescriptionLength);

            RuleFor(x => x.Organization)
                .MaximumLength(ElicitationConsts.MaxOrganizationLength);

            RuleFor(x => x.ContactInfo)
                .NotNull().WithMessage("Contact information is required")
                .SetValidator(new CreateContactInfoDtoValidator());

            RuleFor(x => x.InfluenceData)
                .NotNull().WithMessage("Influence data is required")
                .SetValidator(new CreateInfluenceDataDtoValidator());
        }
    }

    public class UpdateStakeholderProfileDtoValidator : AbstractValidator<UpdateStakeholderProfileDto>
    {
        public UpdateStakeholderProfileDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required")
                .MaximumLength(ElicitationConsts.MaxStakeholderNameLength);

            RuleFor(x => x.Description)
                .MaximumLength(ElicitationConsts.MaxStakeholderDescriptionLength);

            RuleFor(x => x.Organization)
                .MaximumLength(ElicitationConsts.MaxOrganizationLength);

            RuleFor(x => x.ContactInfo)
                .NotNull().WithMessage("Contact information is required")
                .SetValidator(new UpdateContactInfoDtoValidator());

            RuleFor(x => x.InfluenceData)
                .NotNull().WithMessage("Influence data is required")
                .SetValidator(new UpdateInfluenceDataDtoValidator());
        }
    }

    public class CreateContactInfoDtoValidator : AbstractValidator<CreateContactInfoDto>
    {
        public CreateContactInfoDtoValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Valid email is required")
                .MaximumLength(ElicitationConsts.MaxEmailLength);

            RuleFor(x => x.Phone)
                .MaximumLength(ElicitationConsts.MaxPhoneLength);

            RuleFor(x => x.Address)
                .MaximumLength(ElicitationConsts.MaxAddressLength);
        }
    }

    public class UpdateContactInfoDtoValidator : AbstractValidator<UpdateContactInfoDto>
    {
        public UpdateContactInfoDtoValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Valid email is required")
                .MaximumLength(ElicitationConsts.MaxEmailLength);

            RuleFor(x => x.Phone)
                .MaximumLength(ElicitationConsts.MaxPhoneLength);

            RuleFor(x => x.Address)
                .MaximumLength(ElicitationConsts.MaxAddressLength);
        }
    }

    public class CreateInfluenceDataDtoValidator : AbstractValidator<CreateInfluenceDataDto>
    {
        public CreateInfluenceDataDtoValidator()
        {
            RuleFor(x => x.InfluenceLevel)
                .InclusiveBetween(ElicitationConsts.MinInfluenceLevel, ElicitationConsts.MaxInfluenceLevel);

            RuleFor(x => x.InterestLevel)
                .InclusiveBetween(ElicitationConsts.MinInterestLevel, ElicitationConsts.MaxInterestLevel);

            RuleFor(x => x.Notes)
                .MaximumLength(500);
        }
    }

    public class UpdateInfluenceDataDtoValidator : AbstractValidator<UpdateInfluenceDataDto>
    {
        public UpdateInfluenceDataDtoValidator()
        {
            RuleFor(x => x.InfluenceLevel)
                .InclusiveBetween(ElicitationConsts.MinInfluenceLevel, ElicitationConsts.MaxInfluenceLevel);

            RuleFor(x => x.InterestLevel)
                .InclusiveBetween(ElicitationConsts.MinInterestLevel, ElicitationConsts.MaxInterestLevel);

            RuleFor(x => x.Notes)
                .MaximumLength(500);
        }
    }
}
