using NouAssist.FinancialManagement.CreditScoreMonitoring.Aggregates;
using NouAssist.FinancialManagement.CreditScoreMonitoring.CreditDisputes.Dtos;
using NouAssist.FinancialManagement.CreditScoreMonitoring.CreditReports.Dtos;
using NouAssist.FinancialManagement.CreditScoreMonitoring.Entities;
using Riok.Mapperly.Abstractions;
using Volo.Abp.Mapperly;

namespace NouAssist.FinancialManagement.CreditScoreMonitoring
{
    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class CreditReportToCreditReportDtoMapper : MapperBase<CreditReport, CreditReportDto>
    {
        [MapperIgnoreSource(nameof(CreditReport.TenantId))]
        public override partial CreditReportDto Map(CreditReport source);

        [MapperIgnoreSource(nameof(CreditReport.TenantId))]
        public override partial void Map(CreditReport source, CreditReportDto destination);
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class CreditAccountToCreditAccountDtoMapper : MapperBase<CreditAccount, CreditAccountDto>
    {
        [MapperIgnoreTarget(nameof(CreditAccountDto.HasDelinquentPayments))]
        public override partial CreditAccountDto Map(CreditAccount source);

        [MapperIgnoreTarget(nameof(CreditAccountDto.HasDelinquentPayments))]
        public override partial void Map(CreditAccount source, CreditAccountDto destination);

        public override void AfterMap(CreditAccount source, CreditAccountDto destination)
        {
            destination.HasDelinquentPayments = source.HasDelinquentPayments();
        }
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class CreditInquiryToCreditInquiryDtoMapper : MapperBase<CreditInquiry, CreditInquiryDto>
    {
        [MapperIgnoreTarget(nameof(CreditInquiryDto.IsRecent))]
        public override partial CreditInquiryDto Map(CreditInquiry source);

        [MapperIgnoreTarget(nameof(CreditInquiryDto.IsRecent))]
        public override partial void Map(CreditInquiry source, CreditInquiryDto destination);

        public override void AfterMap(CreditInquiry source, CreditInquiryDto destination)
        {
            destination.IsRecent = source.IsRecent();
        }
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class CreditAlertToCreditAlertDtoMapper : MapperBase<CreditAlert, CreditAlertDto>
    {
        public override partial CreditAlertDto Map(CreditAlert source);

        public override partial void Map(CreditAlert source, CreditAlertDto destination);
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class CreditDisputeToCreditDisputeDtoMapper : MapperBase<CreditDispute, CreditDisputeDto>
    {
        [MapperIgnoreSource(nameof(CreditDispute.TenantId))]
        public override partial CreditDisputeDto Map(CreditDispute source);

        [MapperIgnoreSource(nameof(CreditDispute.TenantId))]
        public override partial void Map(CreditDispute source, CreditDisputeDto destination);
    }
}
