using AutoMapper;
using NouAssist.DebtManagement.Aggregates;
using NouAssist.DebtManagement.DebtAccounts.Dtos;
using NouAssist.DebtManagement.Entities;
using NouAssist.DebtManagement.PayoffPlans.Dtos;

namespace NouAssist.DebtManagement
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
