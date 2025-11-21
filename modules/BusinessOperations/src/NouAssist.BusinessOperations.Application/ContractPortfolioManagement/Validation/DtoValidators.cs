using System;
using FluentValidation;
using NouAssist.BusinessOperations.ContractPortfolioManagement.Constants;
using NouAssist.ContractPortfolioManagement.Constants;
using NouAssist.ContractPortfolioManagement.Contracts.Dtos;
using NouAssist.ContractPortfolioManagement.Portfolios.Dtos;
using NouAssist.ContractPortfolioManagement.PortfolioRoadmaps.Dtos;

namespace NouAssist.ContractPortfolioManagement.Validation;

public class CreateContractDtoValidator : AbstractValidator<CreateContractDto>
{
    public CreateContractDtoValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(ContractPortfolioManagementConsts.Contract.MaxTitleLength);

        RuleFor(x => x.ReferenceNumber)
            .NotEmpty()
            .MaximumLength(ContractPortfolioManagementConsts.Contract.MaxReferenceNumberLength);

        RuleFor(x => x.Description)
            .MaximumLength(ContractPortfolioManagementConsts.Contract.MaxDescriptionLength);

        RuleFor(x => x.EffectiveDate)
            .NotNull();

        RuleFor(x => x.ExpirationDate)
            .GreaterThan(x => x.EffectiveDate)
            .When(x => x.ExpirationDate.HasValue)
            .WithMessage("Expiration date must be after effective date");

        RuleFor(x => x.TotalValue)
            .GreaterThanOrEqualTo(0);

        RuleFor(x => x.Currency)
            .NotEmpty()
            .MaximumLength(10);

        RuleFor(x => x.TemplateReference)
            .NotNull();

        RuleFor(x => x.MainDocument)
            .NotNull();
    }
}

public class UpdateContractDtoValidator : AbstractValidator<UpdateContractDto>
{
    public UpdateContractDtoValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(ContractPortfolioManagementConsts.Contract.MaxTitleLength);

        RuleFor(x => x.ReferenceNumber)
            .NotEmpty()
            .MaximumLength(ContractPortfolioManagementConsts.Contract.MaxReferenceNumberLength);

        RuleFor(x => x.Description)
            .MaximumLength(ContractPortfolioManagementConsts.Contract.MaxDescriptionLength);

        RuleFor(x => x.EffectiveDate)
            .NotNull();

        RuleFor(x => x.ExpirationDate)
            .GreaterThan(x => x.EffectiveDate)
            .When(x => x.ExpirationDate.HasValue)
            .WithMessage("Expiration date must be after effective date");

        RuleFor(x => x.TotalValue)
            .GreaterThanOrEqualTo(0);

        RuleFor(x => x.Currency)
            .NotEmpty()
            .MaximumLength(10);
    }
}

public class CreateContractPartyDtoValidator : AbstractValidator<CreateContractPartyDto>
{
    public CreateContractPartyDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(ContractPortfolioManagementConsts.ContractParty.MaxNameLength);

        RuleFor(x => x.Identifier)
            .NotEmpty()
            .MaximumLength(ContractPortfolioManagementConsts.ContractParty.MaxIdentifierLength);

        RuleFor(x => x.ContactInfo)
            .MaximumLength(ContractPortfolioManagementConsts.ContractParty.MaxContactInfoLength);
    }
}

public class CreateContractVersionDtoValidator : AbstractValidator<CreateContractVersionDto>
{
    public CreateContractVersionDtoValidator()
    {
        RuleFor(x => x.VersionLabel)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.DocumentId)
            .NotNull();
    }
}

public class CreateContractAlertDtoValidator : AbstractValidator<CreateContractAlertDto>
{
    public CreateContractAlertDtoValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.Description)
            .MaximumLength(1000);

        RuleFor(x => x.TriggerDate)
            .NotNull();

        RuleFor(x => x.DaysInAdvance)
            .GreaterThanOrEqualTo(0);

        RuleFor(x => x.Frequency)
            .NotEmpty()
            .When(x => x.IsRecurring)
            .WithMessage("Frequency is required when alert is recurring");
    }
}

public class CreateContractAmendmentDtoValidator : AbstractValidator<CreateContractAmendmentDto>
{
    public CreateContractAmendmentDtoValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.Description)
            .NotEmpty()
            .MaximumLength(2000);

        RuleFor(x => x.EffectiveDate)
            .NotNull();

        RuleFor(x => x.DocumentId)
            .NotNull();
    }
}

public class CreatePortfolioDtoValidator : AbstractValidator<CreatePortfolioDto>
{
    public CreatePortfolioDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(ContractPortfolioManagementConsts.Portfolio.MaxNameLength);

        RuleFor(x => x.Description)
            .MaximumLength(ContractPortfolioManagementConsts.Portfolio.MaxDescriptionLength);
    }
}

public class UpdatePortfolioDtoValidator : AbstractValidator<UpdatePortfolioDto>
{
    public UpdatePortfolioDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(ContractPortfolioManagementConsts.Portfolio.MaxNameLength);

        RuleFor(x => x.Description)
            .MaximumLength(ContractPortfolioManagementConsts.Portfolio.MaxDescriptionLength);
    }
}

public class AddPortfolioItemDtoValidator : AbstractValidator<AddPortfolioItemDto>
{
    public AddPortfolioItemDtoValidator()
    {
        RuleFor(x => x.ItemId)
            .NotEmpty();

        RuleFor(x => x.ItemName)
            .NotEmpty()
            .MaximumLength(256);

        RuleFor(x => x.Notes)
            .MaximumLength(1000);
    }
}

public class UpdatePortfolioItemDtoValidator : AbstractValidator<UpdatePortfolioItemDto>
{
    public UpdatePortfolioItemDtoValidator()
    {
        RuleFor(x => x.Progress)
            .InclusiveBetween(0, 100)
            .When(x => x.Progress.HasValue);

        RuleFor(x => x.Notes)
            .MaximumLength(1000);
    }
}

public class CreatePortfolioCategoryDtoValidator : AbstractValidator<CreatePortfolioCategoryDto>
{
    public CreatePortfolioCategoryDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(ContractPortfolioManagementConsts.PortfolioCategory.MaxNameLength);

        RuleFor(x => x.Description)
            .MaximumLength(ContractPortfolioManagementConsts.PortfolioCategory.MaxDescriptionLength);

        RuleFor(x => x.ColorCode)
            .MaximumLength(30);
    }
}

public class CreatePortfolioRiskDtoValidator : AbstractValidator<CreatePortfolioRiskDto>
{
    public CreatePortfolioRiskDtoValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.Description)
            .NotEmpty()
            .MaximumLength(1000);

        RuleFor(x => x.MitigationPlan)
            .MaximumLength(2000);

        RuleFor(x => x.Probability)
            .InclusiveBetween(0, 1);

        RuleFor(x => x.Impact)
            .InclusiveBetween(1, 10);
    }
}

public class CreateSharingLinkDtoValidator : AbstractValidator<CreateSharingLinkDto>
{
    public CreateSharingLinkDtoValidator()
    {
        RuleFor(x => x.ExpirationDate)
            .NotNull()
            .GreaterThan(DateTime.Now)
            .WithMessage("Expiration date must be in the future");
    }
}

public class CreatePortfolioRoadmapDtoValidator : AbstractValidator<CreatePortfolioRoadmapDto>
{
    public CreatePortfolioRoadmapDtoValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(ContractPortfolioManagementConsts.PortfolioRoadmap.MaxTitleLength);

        RuleFor(x => x.Description)
            .MaximumLength(ContractPortfolioManagementConsts.PortfolioRoadmap.MaxDescriptionLength);

        RuleFor(x => x.PortfolioId)
            .NotEmpty();

        RuleFor(x => x.StartDate)
            .NotNull();

        RuleFor(x => x.EndDate)
            .NotNull()
            .GreaterThan(x => x.StartDate)
            .WithMessage("End date must be after start date");
    }
}

public class UpdatePortfolioRoadmapDtoValidator : AbstractValidator<UpdatePortfolioRoadmapDto>
{
    public UpdatePortfolioRoadmapDtoValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(ContractPortfolioManagementConsts.PortfolioRoadmap.MaxTitleLength);

        RuleFor(x => x.Description)
            .MaximumLength(ContractPortfolioManagementConsts.PortfolioRoadmap.MaxDescriptionLength);

        RuleFor(x => x.PortfolioId)
            .NotEmpty();

        RuleFor(x => x.StartDate)
            .NotNull();

        RuleFor(x => x.EndDate)
            .NotNull()
            .GreaterThan(x => x.StartDate)
            .WithMessage("End date must be after start date");
    }
}

public class CreateRoadmapMilestoneDtoValidator : AbstractValidator<CreateRoadmapMilestoneDto>
{
    public CreateRoadmapMilestoneDtoValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.Description)
            .MaximumLength(1000);

        RuleFor(x => x.PlannedDate)
            .NotNull();
    }
}

public class UpdateRoadmapMilestoneDtoValidator : AbstractValidator<UpdateRoadmapMilestoneDto>
{
    public UpdateRoadmapMilestoneDtoValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.Description)
            .MaximumLength(1000);

        RuleFor(x => x.PlannedDate)
            .NotNull();
    }
}

public class CreateCrossItemDependencyDtoValidator : AbstractValidator<CreateCrossItemDependencyDto>
{
    public CreateCrossItemDependencyDtoValidator()
    {
        RuleFor(x => x.SourceMilestoneId)
            .NotEmpty();

        RuleFor(x => x.TargetMilestoneId)
            .NotEmpty();

        RuleFor(x => x.TargetMilestoneId)
            .NotEqual(x => x.SourceMilestoneId)
            .WithMessage("Source and target milestones cannot be the same");
    }
}

public class CreateResourceSkillRequirementDtoValidator : AbstractValidator<CreateResourceSkillRequirementDto>
{
    public CreateResourceSkillRequirementDtoValidator()
    {
        RuleFor(x => x.MilestoneId)
            .NotEmpty();

        RuleFor(x => x.SkillName)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.RequiredLevel)
            .InclusiveBetween(1, 5);

        RuleFor(x => x.Description)
            .MaximumLength(500);
    }
}
