using FluentValidation;
using NouAssist.LearningDevelopment.Learning.LearningSessions.Dtos;

namespace NouAssist.LearningDevelopment.Learning.LearningSessions.Validators
{
    public class RecordBloomAssessmentDtoValidator : AbstractValidator<RecordBloomAssessmentDto>
    {
        public RecordBloomAssessmentDtoValidator()
        {
            RuleFor(x => x.RememberScore)
                .InclusiveBetween(0, 1);

            RuleFor(x => x.UnderstandScore)
                .InclusiveBetween(0, 1);

            RuleFor(x => x.ApplyScore)
                .InclusiveBetween(0, 1);

            RuleFor(x => x.AnalyzeScore)
                .InclusiveBetween(0, 1);

            RuleFor(x => x.EvaluateScore)
                .InclusiveBetween(0, 1);

            RuleFor(x => x.CreateScore)
                .InclusiveBetween(0, 1);
        }
    }
}
