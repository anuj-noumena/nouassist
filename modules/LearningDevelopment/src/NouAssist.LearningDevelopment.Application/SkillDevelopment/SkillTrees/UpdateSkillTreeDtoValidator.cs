using FluentValidation;
using NouAssist.LearningDevelopment.SkillDevelopment.SkillTrees.Dtos;
using NouAssist.SkillDevelopment;

namespace NouAssist.LearningDevelopment.SkillDevelopment.SkillTrees
{
    public class UpdateSkillTreeDtoValidator : AbstractValidator<UpdateSkillTreeDto>
    {
        public UpdateSkillTreeDtoValidator()
        {
            RuleFor(x => x.Structure)
                .NotNull()
                .WithMessage("Tree structure is required")
                .Must(structure => structure.ContainsKey("nodes") && structure.ContainsKey("edges"))
                .WithMessage("Structure must contain 'nodes' and 'edges' properties");

            RuleFor(x => x.VisualizationUrl)
                .MaximumLength(SkillTreeConsts.MaxVisualizationUrlLength)
                .Must(BeAValidUrl)
                .When(x => !string.IsNullOrWhiteSpace(x.VisualizationUrl))
                .WithMessage("Visualization URL must be a valid URL");
        }

        private bool BeAValidUrl(string url)
        {
            return System.Uri.TryCreate(url, System.UriKind.Absolute, out var uriResult)
                && (uriResult.Scheme == System.Uri.UriSchemeHttp || uriResult.Scheme == System.Uri.UriSchemeHttps);
        }
    }
}
