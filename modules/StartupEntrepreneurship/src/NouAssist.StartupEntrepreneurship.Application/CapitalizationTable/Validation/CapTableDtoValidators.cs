using FluentValidation;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.CapTables.Dtos;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.Constants;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.SecurityHolders.Dtos;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.SecurityIssuances.Dtos;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.FundingRounds.Dtos;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.OptionPools.Dtos;

namespace NouAssist.StartupEntrepreneurship.CapitalizationTable.Validation
{
    /// <summary>
    /// FluentValidation validators for CapTable DTOs
    /// </summary>
    public class CreateCapTableDtoValidator : AbstractValidator<CreateCapTableDto>
    {
        public CreateCapTableDtoValidator()
        {
            RuleFor(x => x.CompanyName)
                .NotEmpty()
                .Length(1, CapTableConsts.MaxCompanyNameLength);

            RuleFor(x => x.CompanyIdentifier)
                .MaximumLength(CapTableConsts.MaxNameLength);

            RuleFor(x => x.TotalAuthorizedShares)
                .GreaterThan(0)
                .LessThanOrEqualTo(CapTableConsts.MaxShareQuantity);

            RuleFor(x => x.AsOfDate)
                .LessThanOrEqualTo(DateTime.UtcNow)
                .When(x => x.AsOfDate.HasValue);
        }
    }

    public class CreateSecurityHolderDtoValidator : AbstractValidator<CreateSecurityHolderDto>
    {
        public CreateSecurityHolderDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .Length(1, CapTableConsts.MaxHolderNameLength);

            RuleFor(x => x.Email)
                .EmailAddress()
                .MaximumLength(256)
                .When(x => !string.IsNullOrEmpty(x.Email));

            RuleFor(x => x.HolderType)
                .IsInEnum();

            RuleFor(x => x.TaxId)
                .MaximumLength(50);

            RuleFor(x => x.Address)
                .MaximumLength(500);

            RuleFor(x => x.ExternalReference)
                .MaximumLength(100);

            RuleFor(x => x.DateJoined)
                .LessThanOrEqualTo(DateTime.UtcNow)
                .When(x => x.DateJoined.HasValue);
        }
    }

    public class CreateStockIssuanceDtoValidator : AbstractValidator<CreateStockIssuanceDto>
    {
        public CreateStockIssuanceDtoValidator()
        {
            RuleFor(x => x.CapTableId)
                .NotEmpty();

            RuleFor(x => x.Name)
                .NotEmpty()
                .Length(1, CapTableConsts.MaxNameLength);

            RuleFor(x => x.Description)
                .MaximumLength(CapTableConsts.MaxDescriptionLength);

            RuleFor(x => x.TotalAuthorized)
                .GreaterThan(0)
                .LessThanOrEqualTo(CapTableConsts.MaxShareQuantity);

            RuleFor(x => x.IssuePrice)
                .GreaterThanOrEqualTo(CapTableConsts.MinPricePerShare)
                .LessThanOrEqualTo(CapTableConsts.MaxPricePerShare);

            RuleFor(x => x.Currency)
                .NotEmpty()
                .Length(3)
                .When(x => !string.IsNullOrEmpty(x.Currency));

            RuleFor(x => x.IssuanceDate)
                .LessThanOrEqualTo(DateTime.UtcNow)
                .When(x => x.IssuanceDate.HasValue);
        }
    }

    public class CreateOptionIssuanceDtoValidator : AbstractValidator<CreateOptionIssuanceDto>
    {
        public CreateOptionIssuanceDtoValidator()
        {
            RuleFor(x => x.CapTableId)
                .NotEmpty();

            RuleFor(x => x.Name)
                .NotEmpty()
                .Length(1, CapTableConsts.MaxNameLength);

            RuleFor(x => x.Description)
                .MaximumLength(CapTableConsts.MaxDescriptionLength);

            RuleFor(x => x.TotalAuthorized)
                .GreaterThan(0)
                .LessThanOrEqualTo(CapTableConsts.MaxShareQuantity);

            RuleFor(x => x.StrikePrice)
                .GreaterThanOrEqualTo(CapTableConsts.MinPricePerShare)
                .LessThanOrEqualTo(CapTableConsts.MaxPricePerShare);

            RuleFor(x => x.Currency)
                .NotEmpty()
                .Length(3)
                .When(x => !string.IsNullOrEmpty(x.Currency));

            RuleFor(x => x.ExpiryDate)
                .GreaterThan(DateTime.UtcNow)
                .When(x => x.ExpiryDate.HasValue);

            RuleFor(x => x.IssuanceDate)
                .LessThanOrEqualTo(DateTime.UtcNow)
                .When(x => x.IssuanceDate.HasValue);
        }
    }

    public class CreateFundingRoundDtoValidator : AbstractValidator<CreateFundingRoundDto>
    {
        public CreateFundingRoundDtoValidator()
        {
            RuleFor(x => x.CapTableId)
                .NotEmpty();

            RuleFor(x => x.Name)
                .NotEmpty()
                .Length(1, CapTableConsts.MaxFundingRoundNameLength);

            RuleFor(x => x.Description)
                .MaximumLength(CapTableConsts.MaxDescriptionLength);

            RuleFor(x => x.PreMoneyValuation)
                .GreaterThan(0);

            RuleFor(x => x.TargetAmount)
                .GreaterThan(0);

            RuleFor(x => x.Currency)
                .NotEmpty()
                .Length(3);

            RuleFor(x => x.StartDate)
                .LessThanOrEqualTo(DateTime.UtcNow.AddDays(30)) // Allow some future dating
                .When(x => x.StartDate.HasValue);

            RuleFor(x => x.LeadInvestor)
                .MaximumLength(CapTableConsts.MaxHolderNameLength);
        }
    }

    public class CreateOptionPoolDtoValidator : AbstractValidator<CreateOptionPoolDto>
    {
        public CreateOptionPoolDtoValidator()
        {
            RuleFor(x => x.CapTableId)
                .NotEmpty();

            RuleFor(x => x.Name)
                .NotEmpty()
                .Length(1, CapTableConsts.MaxNameLength);

            RuleFor(x => x.Description)
                .MaximumLength(CapTableConsts.MaxDescriptionLength);

            RuleFor(x => x.TotalReserved)
                .GreaterThan(0)
                .LessThanOrEqualTo(CapTableConsts.MaxShareQuantity);

            RuleFor(x => x.PoolPercentage)
                .GreaterThan(0)
                .LessThanOrEqualTo(100);

            RuleFor(x => x.DefaultVestingPeriodMonths)
                .GreaterThan(0)
                .LessThanOrEqualTo(120); // 10 years max

            RuleFor(x => x.DefaultCliffMonths)
                .GreaterThanOrEqualTo(0)
                .LessThan(x => x.DefaultVestingPeriodMonths);

            RuleFor(x => x.DefaultVestingPercentage)
                .GreaterThan(0)
                .LessThanOrEqualTo(100);
        }
    }

    public class AddVestingScheduleDtoValidator : AbstractValidator<AddVestingScheduleDto>
    {
        public AddVestingScheduleDtoValidator()
        {
            RuleFor(x => x.SecurityIssuanceId)
                .NotEmpty();

            RuleFor(x => x.GrantDate)
                .LessThanOrEqualTo(DateTime.UtcNow);

            RuleFor(x => x.VestingPeriodMonths)
                .GreaterThan(0)
                .LessThanOrEqualTo(120); // 10 years max

            RuleFor(x => x.CliffMonths)
                .GreaterThanOrEqualTo(0)
                .LessThan(x => x.VestingPeriodMonths);

            RuleFor(x => x.VestingPercentage)
                .GreaterThan(0)
                .LessThanOrEqualTo(100);

            RuleFor(x => x.TotalShares)
                .GreaterThan(0)
                .LessThanOrEqualTo(CapTableConsts.MaxShareQuantity);
        }
    }

    public class RecordValuationDtoValidator : AbstractValidator<RecordValuationDto>
    {
        public RecordValuationDtoValidator()
        {
            RuleFor(x => x.ValuationDate)
                .LessThanOrEqualTo(DateTime.UtcNow);

            RuleFor(x => x.PreMoneyValuation)
                .GreaterThan(0);

            RuleFor(x => x.Investment)
                .GreaterThanOrEqualTo(0);

            RuleFor(x => x.Currency)
                .NotEmpty()
                .Length(3);

            RuleFor(x => x.Source)
                .NotEmpty()
                .MaximumLength(256);

            RuleFor(x => x.Notes)
                .MaximumLength(1000);
        }
    }
}
