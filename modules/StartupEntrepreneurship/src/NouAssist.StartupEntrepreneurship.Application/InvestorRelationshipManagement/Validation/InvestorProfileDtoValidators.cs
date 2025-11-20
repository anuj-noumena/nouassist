using FluentValidation;
using NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.InvestorProfiles.Dtos;

namespace NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.Validation
{
    /// <summary>
    /// FluentValidation validators for investor profile DTOs
    /// </summary>
    public class CreateInvestorProfileDtoValidator : AbstractValidator<CreateInvestorProfileDto>
    {
        public CreateInvestorProfileDtoValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty()
                .MaximumLength(200);

            RuleFor(x => x.LastName)
                .NotEmpty()
                .MaximumLength(200);

            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress()
                .MaximumLength(256);

            RuleFor(x => x.Type)
                .IsInEnum();

            RuleFor(x => x.CompanyName)
                .MaximumLength(300);

            RuleFor(x => x.Phone)
                .MaximumLength(50);

            RuleFor(x => x.Notes)
                .MaximumLength(2000);
        }
    }

    public class UpdateInvestorProfileDtoValidator : AbstractValidator<UpdateInvestorProfileDto>
    {
        public UpdateInvestorProfileDtoValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty()
                .MaximumLength(200);

            RuleFor(x => x.LastName)
                .NotEmpty()
                .MaximumLength(200);

            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress()
                .MaximumLength(256);

            RuleFor(x => x.Type)
                .IsInEnum();

            RuleFor(x => x.CompanyName)
                .MaximumLength(300);

            RuleFor(x => x.Phone)
                .MaximumLength(50);

            RuleFor(x => x.Notes)
                .MaximumLength(2000);
        }
    }

    public class UpdateRelationshipScoreDtoValidator : AbstractValidator<UpdateRelationshipScoreDto>
    {
        public UpdateRelationshipScoreDtoValidator()
        {
            RuleFor(x => x.Score)
                .GreaterThanOrEqualTo(0)
                .LessThanOrEqualTo(100);

            RuleFor(x => x.Notes)
                .MaximumLength(500);
        }
    }
}
