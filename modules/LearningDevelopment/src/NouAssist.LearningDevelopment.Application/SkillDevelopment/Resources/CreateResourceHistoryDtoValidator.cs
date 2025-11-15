using FluentValidation;
using NouAssist.SkillDevelopment.Resources.Dtos;

namespace NouAssist.SkillDevelopment.Resources
{
    public class CreateResourceHistoryDtoValidator : AbstractValidator<CreateResourceHistoryDto>
    {
        public CreateResourceHistoryDtoValidator()
        {
            RuleFor(x => x.ResourceId)
                .NotEmpty().WithMessage("Resource ID is required")
                .MaximumLength(ResourceHistoryConsts.MaxResourceIdLength);

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Resource title is required")
                .MaximumLength(ResourceHistoryConsts.MaxTitleLength);

            RuleFor(x => x.SourceUrl)
                .MaximumLength(ResourceHistoryConsts.MaxSourceUrlLength)
                .Must(BeAValidUrl)
                .When(x => !string.IsNullOrWhiteSpace(x.SourceUrl))
                .WithMessage("Source URL must be a valid URL");

            RuleFor(x => x.RelatedSkillIds)
                .Must(list => list == null || list.Count <= 20)
                .WithMessage("Cannot link more than 20 skills to a resource");
        }

        private bool BeAValidUrl(string url)
        {
            return System.Uri.TryCreate(url, System.UriKind.Absolute, out var uriResult)
                && (uriResult.Scheme == System.Uri.UriSchemeHttp || uriResult.Scheme == System.Uri.UriSchemeHttps);
        }
    }
}
