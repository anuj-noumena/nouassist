using NouAssist.FinancialManagement.InvestmentManagement.Aggregates;
using NouAssist.FinancialManagement.InvestmentManagement.Entities;
using NouAssist.FinancialManagement.InvestmentManagement.InvestmentGoals.Dtos;
using NouAssist.FinancialManagement.InvestmentManagement.Portfolios.Dtos;
using NouAssist.FinancialManagement.InvestmentManagement.ValueObjects;
using Riok.Mapperly.Abstractions;
using Volo.Abp.Mapperly;

namespace NouAssist.FinancialManagement.InvestmentManagement
{
    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class PortfolioToPortfolioDtoMapper : MapperBase<Portfolio, PortfolioDto>
    {
        [MapperIgnoreTarget(nameof(PortfolioDto.CurrentAllocation))]
        [MapperIgnoreTarget(nameof(PortfolioDto.RecentTransactions))]
        [MapperIgnoreTarget(nameof(PortfolioDto.Holdings))]
        public override partial PortfolioDto Map(Portfolio source);

        [MapperIgnoreTarget(nameof(PortfolioDto.CurrentAllocation))]
        [MapperIgnoreTarget(nameof(PortfolioDto.RecentTransactions))]
        [MapperIgnoreTarget(nameof(PortfolioDto.Holdings))]
        public override partial void Map(Portfolio source, PortfolioDto destination);

        public override void AfterMap(Portfolio source, PortfolioDto destination)
        {
            destination.CurrentAllocation = new PortfolioAllocationDto
            {
                AssetClassPercentages = source.GetCurrentAllocation().AssetClassPercentages
            };
        }
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class HoldingToHoldingDtoMapper : MapperBase<Holding, HoldingDto>
    {
        [MapperIgnoreTarget(nameof(HoldingDto.UnrealizedGainLoss))]
        [MapperIgnoreTarget(nameof(HoldingDto.UnrealizedGainLossPercentage))]
        public override partial HoldingDto Map(Holding source);

        [MapperIgnoreTarget(nameof(HoldingDto.UnrealizedGainLoss))]
        [MapperIgnoreTarget(nameof(HoldingDto.UnrealizedGainLossPercentage))]
        public override partial void Map(Holding source, HoldingDto destination);

        public override void AfterMap(Holding source, HoldingDto destination)
        {
            destination.UnrealizedGainLoss = source.GetUnrealizedGainLoss();
            destination.UnrealizedGainLossPercentage = source.GetUnrealizedGainLossPercentage();
        }
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class
        InvestmentTransactionToInvestmentTransactionDtoMapper : MapperBase<InvestmentTransaction,
        InvestmentTransactionDto>
    {
        public override partial InvestmentTransactionDto Map(InvestmentTransaction source);

        public override partial void Map(InvestmentTransaction source, InvestmentTransactionDto destination);
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class
        PortfolioAllocationToPortfolioAllocationDtoMapper : MapperBase<PortfolioAllocation, PortfolioAllocationDto>
    {
        public override partial PortfolioAllocationDto Map(PortfolioAllocation source);

        public override partial void Map(PortfolioAllocation source, PortfolioAllocationDto destination);
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class InvestmentGoalToInvestmentGoalDtoMapper : MapperBase<InvestmentGoal, InvestmentGoalDto>
    {
        [MapperIgnoreTarget(nameof(InvestmentGoalDto.MonthsRemaining))]
        [MapperIgnoreTarget(nameof(InvestmentGoalDto.RequiredMonthlyContribution))]
        public override partial InvestmentGoalDto Map(InvestmentGoal source);

        [MapperIgnoreTarget(nameof(InvestmentGoalDto.MonthsRemaining))]
        [MapperIgnoreTarget(nameof(InvestmentGoalDto.RequiredMonthlyContribution))]
        public override partial void Map(InvestmentGoal source, InvestmentGoalDto destination);

        public override void AfterMap(InvestmentGoal source, InvestmentGoalDto destination)
        {
            destination.MonthsRemaining = source.GetMonthsRemaining();
            destination.RequiredMonthlyContribution = source.GetRequiredMonthlyContribution();
        }
    }
}