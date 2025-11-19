using AutoMapper;
using NouAssist.FinancialManagement.TaxManagement.Aggregates;
using NouAssist.FinancialManagement.TaxManagement.Entities;
using NouAssist.FinancialManagement.TaxManagement.TaxProfiles.Dtos;
using NouAssist.FinancialManagement.TaxManagement.TaxReturns.Dtos;

namespace NouAssist.FinancialManagement.TaxManagement
{
    public class TaxManagementApplicationAutoMapperProfile : Profile
    {
        public TaxManagementApplicationAutoMapperProfile()
        {
            // TaxReturn mappings
            CreateMap<TaxReturn, TaxReturnDto>()
                .ForMember(dest => dest.EffectiveTaxRate, opt => opt.MapFrom(src => src.GetEffectiveTaxRate()));

            CreateMap<TaxDeduction, TaxDeductionDto>();
            CreateMap<TaxDocument, TaxDocumentDto>();
            CreateMap<EstimatedTaxPayment, EstimatedTaxPaymentDto>();

            // TaxProfile mappings
            CreateMap<TaxProfile, TaxProfileDto>();
        }
    }
}
