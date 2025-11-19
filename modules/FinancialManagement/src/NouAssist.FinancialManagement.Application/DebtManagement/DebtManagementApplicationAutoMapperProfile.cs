using AutoMapper;
using NouAssist.FinancialManagement.DebtManagement.Aggregates;
using NouAssist.FinancialManagement.DebtManagement.DebtAccounts.Dtos;
using NouAssist.FinancialManagement.DebtManagement.Entities;
using NouAssist.FinancialManagement.DebtManagement.PayoffPlans.Dtos;

namespace NouAssist.FinancialManagement.DebtManagement
{
    public class DebtManagementApplicationAutoMapperProfile : Profile
    {
        public DebtManagementApplicationAutoMapperProfile()
        {
            // DebtAccount mappings
            CreateMap<DebtAccount, DebtAccountDto>()
                .ForMember(dest => dest.TotalPaid, opt => opt.MapFrom(src => src.GetTotalPaid()))
                .ForMember(dest => dest.TotalInterestPaid, opt => opt.MapFrom(src => src.GetTotalInterestPaid()));

            CreateMap<DebtPayment, DebtPaymentDto>();

            // PayoffPlan mappings
            CreateMap<DebtPayoffPlan, DebtPayoffPlanDto>();
            CreateMap<PayoffPlanDebt, PayoffPlanDebtDto>();
        }
    }
}
