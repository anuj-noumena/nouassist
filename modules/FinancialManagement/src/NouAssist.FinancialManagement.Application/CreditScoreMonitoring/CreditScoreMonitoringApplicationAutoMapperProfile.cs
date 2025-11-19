using AutoMapper;
using NouAssist.FinancialManagement.CreditScoreMonitoring.Aggregates;
using NouAssist.FinancialManagement.CreditScoreMonitoring.CreditDisputes.Dtos;
using NouAssist.FinancialManagement.CreditScoreMonitoring.CreditReports.Dtos;
using NouAssist.FinancialManagement.CreditScoreMonitoring.Entities;

namespace NouAssist.FinancialManagement.CreditScoreMonitoring
{
    public class CreditScoreMonitoringApplicationAutoMapperProfile : Profile
    {
        public CreditScoreMonitoringApplicationAutoMapperProfile()
        {
            // CreditReport mappings
            CreateMap<CreditReport, CreditReportDto>();
            CreateMap<CreditAccount, CreditAccountDto>()
                .ForMember(dest => dest.HasDelinquentPayments, opt => opt.MapFrom(src => src.HasDelinquentPayments()));
            CreateMap<CreditInquiry, CreditInquiryDto>()
                .ForMember(dest => dest.IsRecent, opt => opt.MapFrom(src => src.IsRecent()));
            CreateMap<CreditAlert, CreditAlertDto>();

            // CreditDispute mappings
            CreateMap<CreditDispute, CreditDisputeDto>();
        }
    }
}
