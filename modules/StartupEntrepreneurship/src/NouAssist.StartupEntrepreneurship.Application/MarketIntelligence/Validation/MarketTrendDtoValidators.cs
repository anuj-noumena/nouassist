using System;
using FluentValidation;
using NouAssist.StartupEntrepreneurship.MarketIntelligence.Constants;
using NouAssist.StartupEntrepreneurship.MarketIntelligence.MarketTrend.Dtos;

namespace NouAssist.StartupEntrepreneurship.MarketIntelligence.Validation
{
    public class CreateMarketTrendDtoValidator : AbstractValidator<CreateMarketTrendDto>
    {
        public CreateMarketTrendDtoValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty()
                .MaximumLength(MarketIntelligenceConsts.MarketTrend.MaxTitleLength)
                .WithMessage("Trend title is required and must not exceed {MaxLength} characters.");

            RuleFor(x => x.Description)
                .NotEmpty()
                .MaximumLength(MarketIntelligenceConsts.MarketTrend.MaxDescriptionLength)
                .WithMessage("Trend description is required and must not exceed {MaxLength} characters.");

            RuleFor(x => x.Keywords)
                .MaximumLength(MarketIntelligenceConsts.MarketTrend.MaxKeywordsLength)
                .When(x => !string.IsNullOrEmpty(x.Keywords));

            RuleFor(x => x.SourceUrl)
                .MaximumLength(MarketIntelligenceConsts.MarketTrend.MaxSourceUrlLength)
                .When(x => !string.IsNullOrEmpty(x.SourceUrl))
                .Matches(@"^(https?:\/\/)?([\da-z\.-]+)\.([a-z\.]{2,6})([\/\w \.-]*)*\/?$")
                .WithMessage("Please enter a valid URL.");

            RuleFor(x => x.ImpactDescription)
                .NotEmpty()
                .MaximumLength(MarketIntelligenceConsts.MarketTrend.MaxImpactDescriptionLength)
                .WithMessage("Impact description is required and must not exceed {MaxLength} characters.");

            RuleFor(x => x.ConfidenceScore)
                .InclusiveBetween(0, 1)
                .WithMessage("Confidence score must be between 0 and 1.");

            RuleFor(x => x.RecommendedActions)
                .MaximumLength(MarketIntelligenceConsts.SwotAnalysis.MaxActionItemLength)
                .When(x => !string.IsNullOrEmpty(x.RecommendedActions));

            RuleFor(x => x.ExpiryDate)
                .GreaterThan(DateTime.UtcNow)
                .When(x => x.ExpiryDate.HasValue)
                .WithMessage("Expiry date must be in the future.");

            RuleFor(x => x.Tags)
                .MaximumLength(MarketIntelligenceConsts.Validation.MaxTagsLength)
                .When(x => !string.IsNullOrEmpty(x.Tags));
        }
    }

    public class CreateTrendDataSourceDtoValidator : AbstractValidator<CreateTrendDataSourceDto>
    {
        public CreateTrendDataSourceDtoValidator()
        {
            RuleFor(x => x.SourceName)
                .NotEmpty()
                .MaximumLength(MarketIntelligenceConsts.MarketTrend.MaxTitleLength)
                .WithMessage("Source name is required and must not exceed {MaxLength} characters.");

            RuleFor(x => x.SourceUrl)
                .NotEmpty()
                .MaximumLength(MarketIntelligenceConsts.MarketTrend.MaxSourceUrlLength)
                .Matches(@"^(https?:\/\/)?([\da-z\.-]+)\.([a-z\.]{2,6})([\/\w \.-]*)*\/?$")
                .WithMessage("Please enter a valid URL.");

            RuleFor(x => x.SourceType)
                .MaximumLength(100)
                .When(x => !string.IsNullOrEmpty(x.SourceType));

            RuleFor(x => x.Reliability)
                .InclusiveBetween(0, 1)
                .WithMessage("Reliability score must be between 0 and 1.");

            RuleFor(x => x.DataQuality)
                .MaximumLength(200)
                .When(x => !string.IsNullOrEmpty(x.DataQuality));
        }
    }
}
