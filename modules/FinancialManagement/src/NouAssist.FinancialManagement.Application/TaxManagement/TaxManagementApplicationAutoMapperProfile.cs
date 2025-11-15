using AutoMapper;
using NouAssist.TaxManagement.Aggregates;
using NouAssist.TaxManagement.Entities;
using NouAssist.TaxManagement.TaxProfiles.Dtos;
using NouAssist.TaxManagement.TaxReturns.Dtos;

namespace NouAssist.TaxManagement
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
