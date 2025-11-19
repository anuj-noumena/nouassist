using AutoMapper;
using NouAssist.FinancialManagement.CurrencyManagement.Aggregates;
using NouAssist.FinancialManagement.CurrencyManagement.Dtos;

namespace NouAssist.FinancialManagement.CurrencyManagement
{
    public class CurrencyManagementApplicationAutoMapperProfile : Profile
    {
        public CurrencyManagementApplicationAutoMapperProfile()
        {
            CreateMap<Currency, CurrencyDto>()
                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.Code.Code));

            CreateMap<CurrencyExchangeRate, ExchangeRateDto>()
                .ForMember(dest => dest.BaseCurrency, opt => opt.MapFrom(src => src.BaseCurrency.Code))
                .ForMember(dest => dest.TargetCurrency, opt => opt.MapFrom(src => src.TargetCurrency.Code))
                .ForMember(dest => dest.Rate, opt => opt.MapFrom(src => src.Rate.Rate))
                .ForMember(dest => dest.InverseRate, opt => opt.MapFrom(src => src.Rate.InverseRate))
                .ForMember(dest => dest.EffectiveDate, opt => opt.MapFrom(src => src.Rate.EffectiveDate));
        }
    }
}
