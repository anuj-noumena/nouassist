using FluentValidation;
using NouAssist.Learning.Enums;
using NouAssist.Learning.LearningNotes.Dtos;

namespace NouAssist.Learning.LearningNotes.Validators
{
    public class CreateLearningNoteDtoValidator : AbstractValidator<CreateLearningNoteDto>
    {
        public CreateLearningNoteDtoValidator()
        {
            RuleFor(x => x.TopicId)
                .NotEmpty();

            RuleFor(x => x.Type)
                .IsInEnum();

            When(x => x.Type == LearningNoteType.Cornell, () =>
            {
                RuleFor(x => x.Notes)
                    .NotEmpty()
                    .WithMessage("Notes are required for Cornell-style notes");
            });

            When(x => x.Type != LearningNoteType.Cornell, () =>
            {
                RuleFor(x => x.Content)
                    .NotEmpty()
                    .WithMessage("Content is required for non-Cornell notes");
            });
        }
    }
}
