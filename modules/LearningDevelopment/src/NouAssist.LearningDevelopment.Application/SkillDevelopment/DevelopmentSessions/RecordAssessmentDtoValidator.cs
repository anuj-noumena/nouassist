using System.Linq;
using FluentValidation;
using NouAssist.LearningDevelopment.SkillDevelopment.DevelopmentSessions.Dtos;

namespace NouAssist.LearningDevelopment.SkillDevelopment.DevelopmentSessions
{
    public class RecordAssessmentDtoValidator : AbstractValidator<RecordAssessmentDto>
    {
        public RecordAssessmentDtoValidator()
        {
            RuleFor(x => x.Scores)
                .NotNull()
                .WithMessage("Assessment scores are required")
                .Must(scores => scores != null && scores.Count > 0)
                .WithMessage("At least one assessment score is required")
                .Must(scores => scores == null || scores.All(s => s.Value >= 0 && s.Value <= 100))
                .WithMessage("All scores must be between 0 and 100");

            RuleFor(x => x.StrengthAreas)
                .Must(areas => areas == null || areas.Count <= 20)
                .WithMessage("Cannot have more than 20 strength areas");

            RuleFor(x => x.ImprovementAreas)
                .Must(areas => areas == null || areas.Count <= 20)
                .WithMessage("Cannot have more than 20 improvement areas");

            RuleFor(x => x.OverallNotes)
                .MaximumLength(2000);
        }
    }
}
