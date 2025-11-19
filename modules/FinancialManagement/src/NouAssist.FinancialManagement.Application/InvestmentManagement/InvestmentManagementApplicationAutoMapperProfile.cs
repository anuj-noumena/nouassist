using AutoMapper;
using NouAssist.FinancialManagement.InvestmentManagement.Aggregates;
using NouAssist.FinancialManagement.InvestmentManagement.Entities;
using NouAssist.FinancialManagement.InvestmentManagement.InvestmentGoals.Dtos;
using NouAssist.FinancialManagement.InvestmentManagement.Portfolios.Dtos;
using NouAssist.FinancialManagement.InvestmentManagement.ValueObjects;

namespace NouAssist.FinancialManagement.InvestmentManagement
{
    public class InvestmentManagementApplicationAutoMapperProfile : Profile
    {
        public InvestmentManagementApplicationAutoMapperProfile()
        {
            // Portfolio mappings
            CreateMap<Portfolio, PortfolioDto>()
                .ForMember(dest => dest.CurrentAllocation, opt => opt.MapFrom(src => src.GetCurrentAllocation()))
                .ForMember(dest => dest.RecentTransactions, opt => opt.Ignore());

            CreateMap<Holding, HoldingDto>()
                .ForMember(dest => dest.UnrealizedGainLoss, opt => opt.MapFrom(src => src.GetUnrealizedGainLoss()))
                .ForMember(dest => dest.UnrealizedGainLossPercentage, opt => opt.MapFrom(src => src.GetUnrealizedGainLossPercentage()));

            CreateMap<InvestmentTransaction, InvestmentTransactionDto>();

            CreateMap<PortfolioAllocation, PortfolioAllocationDto>();

            // InvestmentGoal mappings
            CreateMap<InvestmentGoal, InvestmentGoalDto>()
                .ForMember(dest => dest.MonthsRemaining, opt => opt.MapFrom(src => src.GetMonthsRemaining()))
                .ForMember(dest => dest.RequiredMonthlyContribution, opt => opt.MapFrom(src => src.GetRequiredMonthlyContribution()));
        }
    }
}
