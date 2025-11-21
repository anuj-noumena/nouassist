using FluentValidation;
using NouAssist.StartupEntrepreneurship.MarketIntelligence.Competitor.Dtos;
using NouAssist.StartupEntrepreneurship.MarketIntelligence.Constants;

namespace NouAssist.StartupEntrepreneurship.MarketIntelligence.Validation
{
    public class CreateCompetitorDtoValidator : AbstractValidator<CreateCompetitorDto>
    {
        public CreateCompetitorDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(MarketIntelligenceConsts.Competitor.MaxNameLength)
                .WithMessage("Competitor name is required and must not exceed {MaxLength} characters.");

            RuleFor(x => x.Description)
                .NotEmpty()
                .MaximumLength(MarketIntelligenceConsts.Competitor.MaxDescriptionLength)
                .WithMessage("Competitor description is required and must not exceed {MaxLength} characters.");

            RuleFor(x => x.CompetitorInfo).NotNull().WithMessage("Competitor information is required.");
            
            RuleFor(x => x.Notes)
                .MaximumLength(MarketIntelligenceConsts.Validation.MaxNotesLength)
                .When(x => !string.IsNullOrEmpty(x.Notes));
            
            RuleFor(x => x.Tags)
                .MaximumLength(MarketIntelligenceConsts.Validation.MaxTagsLength)
                .When(x => !string.IsNullOrEmpty(x.Tags));

            // Nest validator for competitor info
            RuleFor(x => x.CompetitorInfo).SetValidator(new CreateCompetitorInfoDtoValidator());
        }
    }

    public class CreateCompetitorInfoDtoValidator : AbstractValidator<CreateCompetitorInfoDto>
    {
        public CreateCompetitorInfoDtoValidator()
        {
            RuleFor(x => x.Website)
                .MaximumLength(MarketIntelligenceConsts.Competitor.MaxWebsiteLength)
                .When(x => !string.IsNullOrEmpty(x.Website))
                .Matches(@"^(https?:\/\/)?([\da-z\.-]+)\.([a-z\.]{2,6})([\/\w \.-]*)*\/?$")
                .WithMessage("Please enter a valid URL.");

            RuleFor(x => x.Industry)
                .MaximumLength(MarketIntelligenceConsts.Competitor.MaxIndustryLength)
                .When(x => !string.IsNullOrEmpty(x.Industry));

            RuleFor(x => x.Location)
                .MaximumLength(MarketIntelligenceConsts.Competitor.MaxLocationLength)
                .When(x => !string.IsNullOrEmpty(x.Location));

            RuleFor(x => x.EmployeeCount)
                .GreaterThanOrEqualTo(0)
                .When(x => x.EmployeeCount.HasValue)
                .WithMessage("Employee count must be a positive number.");

            RuleFor(x => x.FundingAmount)
                .GreaterThanOrEqualTo(0)
                .When(x => x.FundingAmount.HasValue)
                .WithMessage("Funding amount must be a positive number.");

            RuleFor(x => x.FundingSource)
                .MaximumLength(MarketIntelligenceConsts.Competitor.MaxFundingSourceLength)
                .When(x => !string.IsNullOrEmpty(x.FundingSource));
        }
    }

    public class CreateCompetitorUpdateDtoValidator : AbstractValidator<CreateCompetitorUpdateDto>
    {
        public CreateCompetitorUpdateDtoValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty()
                .MaximumLength(MarketIntelligenceConsts.MarketTrend.MaxTitleLength)
                .WithMessage("Update title is required and must not exceed {MaxLength} characters.");

            RuleFor(x => x.Description)
                .NotEmpty()
                .MaximumLength(MarketIntelligenceConsts.MarketTrend.MaxDescriptionLength)
                .WithMessage("Update description is required and must not exceed {MaxLength} characters.");

            RuleFor(x => x.Source)
                .MaximumLength(MarketIntelligenceConsts.MarketTrend.MaxSourceUrlLength)
                .When(x => !string.IsNullOrEmpty(x.Source));

            RuleFor(x => x.Tags)
                .MaximumLength(MarketIntelligenceConsts.Validation.MaxTagsLength)
                .When(x => !string.IsNullOrEmpty(x.Tags));
        }
    }
}
