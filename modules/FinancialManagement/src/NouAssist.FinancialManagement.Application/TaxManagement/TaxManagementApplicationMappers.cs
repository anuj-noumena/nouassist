using NouAssist.FinancialManagement.TaxManagement.Aggregates;
using NouAssist.FinancialManagement.TaxManagement.Entities;
using NouAssist.FinancialManagement.TaxManagement.TaxProfiles.Dtos;
using NouAssist.FinancialManagement.TaxManagement.TaxReturns.Dtos;
using Riok.Mapperly.Abstractions;
using Volo.Abp.Mapperly;

namespace NouAssist.FinancialManagement.TaxManagement
{
    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class TaxReturnToTaxReturnDtoMapper : MapperBase<TaxReturn, TaxReturnDto>
    {
        [MapperIgnoreTarget(nameof(TaxReturnDto.EffectiveTaxRate))]
        public override partial TaxReturnDto Map(TaxReturn source);

        [MapperIgnoreTarget(nameof(TaxReturnDto.EffectiveTaxRate))]
        public override partial void Map(TaxReturn source, TaxReturnDto destination);

        public override void AfterMap(TaxReturn source, TaxReturnDto destination)
        {
            destination.EffectiveTaxRate = source.GetEffectiveTaxRate();
        }
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class TaxDeductionToTaxDeductionDtoMapper : MapperBase<TaxDeduction, TaxDeductionDto>
    {
        public override partial TaxDeductionDto Map(TaxDeduction source);

        public override partial void Map(TaxDeduction source, TaxDeductionDto destination);
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class TaxDocumentToTaxDocumentDtoMapper : MapperBase<TaxDocument, TaxDocumentDto>
    {
        public override partial TaxDocumentDto Map(TaxDocument source);

        public override partial void Map(TaxDocument source, TaxDocumentDto destination);
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class EstimatedTaxPaymentToEstimatedTaxPaymentDtoMapper : MapperBase<EstimatedTaxPayment, EstimatedTaxPaymentDto>
    {
        public override partial EstimatedTaxPaymentDto Map(EstimatedTaxPayment source);

        public override partial void Map(EstimatedTaxPayment source, EstimatedTaxPaymentDto destination);
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class TaxProfileToTaxProfileDtoMapper : MapperBase<TaxProfile, TaxProfileDto>
    {
        public override partial TaxProfileDto Map(TaxProfile source);

        public override partial void Map(TaxProfile source, TaxProfileDto destination);
    }
}
