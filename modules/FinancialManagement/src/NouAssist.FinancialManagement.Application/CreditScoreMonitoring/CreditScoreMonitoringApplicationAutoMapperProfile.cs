using AutoMapper;
using NouAssist.CreditScoreMonitoring.CreditDisputes.Dtos;
using NouAssist.CreditScoreMonitoring.CreditReports.Dtos;
using NouAssist.FinancialManagement.CreditScoreMonitoring.Aggregates;
using NouAssist.FinancialManagement.CreditScoreMonitoring.Entities;

namespace NouAssist.CreditScoreMonitoring
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
