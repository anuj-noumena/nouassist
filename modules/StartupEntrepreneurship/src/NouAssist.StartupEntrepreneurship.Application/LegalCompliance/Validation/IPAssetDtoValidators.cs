using FluentValidation;
using NouAssist.StartupEntrepreneurship.LegalCompliance.Constants;
using NouAssist.StartupEntrepreneurship.LegalCompliance.IntellectualProperty.Dtos;
using System;

namespace NouAssist.StartupEntrepreneurship.LegalCompliance.Validation
{
    public class CreateIPAssetDtoValidator : AbstractValidator<CreateIPAssetDto>
    {
        public CreateIPAssetDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MinimumLength(LegalComplianceConsts.MinIPAssetNameLength)
                .MaximumLength(LegalComplianceConsts.MaxIPAssetNameLength);

            RuleFor(x => x.Description)
                .NotEmpty()
                .MaximumLength(LegalComplianceConsts.MaxIPDescriptionLength);

            RuleFor(x => x.Identifier)
                .NotEmpty()
                .MaximumLength(LegalComplianceConsts.MaxIPIdentifierLength);

            RuleFor(x => x.IssuingAuthority)
                .NotEmpty()
                .MaximumLength(LegalComplianceConsts.MaxJurisdictionLength);

            RuleFor(x => x.Country)
                .NotEmpty()
                .MaximumLength(LegalComplianceConsts.MaxJurisdictionLength);

            RuleFor(x => x.State)
                .MaximumLength(LegalComplianceConsts.MaxJurisdictionLength);

            RuleFor(x => x.FilingDate)
                .NotEmpty()
                .LessThanOrEqualTo(DateTime.Now)
                .WithMessage("Filing date cannot be in the future");

            RuleFor(x => x.ExpirationDate)
                .GreaterThan(x => x.FilingDate)
                .When(x => x.ExpirationDate.HasValue)
                .WithMessage("Expiration date must be after filing date");

            RuleFor(x => x.RegistrationDate)
                .GreaterThanOrEqualTo(x => x.FilingDate)
                .When(x => x.RegistrationDate.HasValue)
                .WithMessage("Registration date must be on or after filing date");
        }
    }

    public class UpdateIPAssetDtoValidator : AbstractValidator<UpdateIPAssetDto>
    {
        public UpdateIPAssetDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MinimumLength(LegalComplianceConsts.MinIPAssetNameLength)
                .MaximumLength(LegalComplianceConsts.MaxIPAssetNameLength);

            RuleFor(x => x.Description)
                .NotEmpty()
                .MaximumLength(LegalComplianceConsts.MaxIPDescriptionLength);

            RuleFor(x => x.Country)
                .NotEmpty()
                .MaximumLength(LegalComplianceConsts.MaxJurisdictionLength);

            RuleFor(x => x.State)
                .MaximumLength(LegalComplianceConsts.MaxJurisdictionLength);
        }
    }

    public class IPValuationDtoValidator : AbstractValidator<IPValuationDto>
    {
        public IPValuationDtoValidator()
        {
            RuleFor(x => x.EstimatedValue)
                .GreaterThan(0)
                .WithMessage("Estimated value must be greater than zero");

            RuleFor(x => x.Currency)
                .NotEmpty()
                .Length(3)
                .WithMessage("Currency code must be 3 characters (e.g., USD, EUR)");

            RuleFor(x => x.ValuationMethod)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.ValuationNotes)
                .MaximumLength(LegalComplianceConsts.MaxNotesLength);
        }
    }

    public class CreateIPDeadlineDtoValidator : AbstractValidator<CreateIPDeadlineDto>
    {
        public CreateIPDeadlineDtoValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty()
                .MinimumLength(LegalComplianceConsts.MinObligationTitleLength)
                .MaximumLength(LegalComplianceConsts.MaxObligationTitleLength);

            RuleFor(x => x.Deadline)
                .NotEmpty();

            RuleFor(x => x.Description)
                .MaximumLength(LegalComplianceConsts.MaxObligationDescriptionLength);

            RuleFor(x => x.RecurrencePattern)
                .NotEmpty()
                .When(x => x.IsRecurring)
                .WithMessage("Recurrence pattern is required when IsRecurring is true");
        }
    }
}
