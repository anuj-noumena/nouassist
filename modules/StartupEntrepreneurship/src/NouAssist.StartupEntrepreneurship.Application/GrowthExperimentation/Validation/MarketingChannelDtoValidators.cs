using System;
using FluentValidation;
using NouAssist.StartupEntrepreneurship.GrowthExperimentation.MarketingChannels.Dtos;

namespace NouAssist.StartupEntrepreneurship.GrowthExperimentation.Validation
{
    public class CreateMarketingChannelDtoValidator : AbstractValidator<CreateMarketingChannelDto>
    {
        public CreateMarketingChannelDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(100)
                .WithMessage("Channel name is required and must be at most 100 characters.");

            RuleFor(x => x.Details).NotNull();
            RuleFor(x => x.Details.Platform)
                .NotEmpty()
                .When(x => x.Details != null)
                .WithMessage("Platform name is required.");
                
            RuleFor(x => x.Details.Url)
                .Must(BeAValidUrl)
                .When(x => !string.IsNullOrEmpty(x.Details?.Url))
                .WithMessage("Must be a valid URL.");
        }
        
        private bool BeAValidUrl(string url)
        {
            return Uri.TryCreate(url, UriKind.Absolute, out var uriResult) && 
                   (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        }
    }

    public class UpdateCostPerAcquisitionDtoValidator : AbstractValidator<UpdateCostPerAcquisitionDto>
    {
        public UpdateCostPerAcquisitionDtoValidator()
        {
            RuleFor(x => x.Cost)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Cost must be a non-negative value.");

            RuleFor(x => x.Acquisitions)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Acquisitions must be a non-negative value.");
                
            RuleFor(x => x.PeriodEnd)
                .GreaterThanOrEqualTo(x => x.PeriodStart)
                .WithMessage("Period end date must be after or equal to period start date.");
        }
    }
}
