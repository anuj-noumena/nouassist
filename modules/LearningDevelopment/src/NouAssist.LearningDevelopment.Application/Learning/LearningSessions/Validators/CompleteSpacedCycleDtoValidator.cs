using FluentValidation;
using NouAssist.LearningDevelopment.Learning.LearningSessions.Dtos;

namespace NouAssist.LearningDevelopment.Learning.LearningSessions.Validators
{
    public class CompleteSpacedCycleDtoValidator : AbstractValidator<CompleteSpacedCycleDto>
    {
        public CompleteSpacedCycleDtoValidator()
        {
            RuleFor(x => x.CycleNumber)
                .InclusiveBetween(1, 4)
                .WithMessage("Cycle number must be between 1 and 4");

            RuleFor(x => x.RecallAccuracy)
                .InclusiveBetween(0, 1)
                .WithMessage("Recall accuracy must be between 0 and 1");

            RuleFor(x => x.AverageConfidence)
                .InclusiveBetween(0, 1)
                .WithMessage("Average confidence must be between 0 and 1");

            RuleFor(x => x.CompletionRate)
                .InclusiveBetween(0, 100)
                .WithMessage("Completion rate must be between 0 and 100");
        }
    }
}
