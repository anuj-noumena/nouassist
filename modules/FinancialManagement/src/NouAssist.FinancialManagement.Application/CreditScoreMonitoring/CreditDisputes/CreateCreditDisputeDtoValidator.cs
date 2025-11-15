using FluentValidation;
using NouAssist.CreditScoreMonitoring.CreditDisputes.Dtos;

namespace NouAssist.CreditScoreMonitoring.CreditDisputes
{
    public class CreateCreditDisputeDtoValidator : AbstractValidator<CreateCreditDisputeDto>
    {
        public CreateCreditDisputeDtoValidator()
        {
            RuleFor(x => x.Bureau)
                .IsInEnum()
                .WithMessage("Invalid credit bureau");

            RuleFor(x => x.ItemType)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.ItemDescription)
                .NotEmpty()
                .MaximumLength(500);

            RuleFor(x => x.DisputeReason)
                .NotEmpty()
                .MaximumLength(1000);

            RuleFor(x => x.SupportingDocumentation)
                .MaximumLength(2000);
        }
    }
}
