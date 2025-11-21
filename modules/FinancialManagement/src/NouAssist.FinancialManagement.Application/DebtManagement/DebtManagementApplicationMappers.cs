using NouAssist.FinancialManagement.DebtManagement.Aggregates;
using NouAssist.FinancialManagement.DebtManagement.DebtAccounts.Dtos;
using NouAssist.FinancialManagement.DebtManagement.Entities;
using NouAssist.FinancialManagement.DebtManagement.PayoffPlans.Dtos;
using Riok.Mapperly.Abstractions;
using Volo.Abp.Mapperly;

namespace NouAssist.FinancialManagement.DebtManagement
{
    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class DebtAccountToDebtAccountDtoMapper : MapperBase<DebtAccount, DebtAccountDto>
    {
        [MapperIgnoreSource(nameof(DebtAccount.TenantId))]
        [MapperIgnoreTarget(nameof(DebtAccountDto.TotalPaid))]
        [MapperIgnoreTarget(nameof(DebtAccountDto.TotalInterestPaid))]
        public override partial DebtAccountDto Map(DebtAccount source);

        [MapperIgnoreSource(nameof(DebtAccount.TenantId))]
        [MapperIgnoreTarget(nameof(DebtAccountDto.TotalPaid))]
        [MapperIgnoreTarget(nameof(DebtAccountDto.TotalInterestPaid))]
        public override partial void Map(DebtAccount source, DebtAccountDto destination);

        public override void AfterMap(DebtAccount source, DebtAccountDto destination)
        {
            destination.TotalPaid = source.GetTotalPaid();
            destination.TotalInterestPaid = source.GetTotalInterestPaid();
        }
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class DebtPaymentToDebtPaymentDtoMapper : MapperBase<DebtPayment, DebtPaymentDto>
    {
        public override partial DebtPaymentDto Map(DebtPayment source);

        public override partial void Map(DebtPayment source, DebtPaymentDto destination);
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class DebtPayoffPlanToDebtPayoffPlanDtoMapper : MapperBase<DebtPayoffPlan, DebtPayoffPlanDto>
    {
        [MapperIgnoreSource(nameof(DebtPayoffPlan.TenantId))]
        [MapperIgnoreTarget(nameof(DebtPayoffPlanDto.Analysis))]
        public override partial DebtPayoffPlanDto Map(DebtPayoffPlan source);

        [MapperIgnoreSource(nameof(DebtPayoffPlan.TenantId))]
        [MapperIgnoreTarget(nameof(DebtPayoffPlanDto.Analysis))]
        public override partial void Map(DebtPayoffPlan source, DebtPayoffPlanDto destination);
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class PayoffPlanDebtToPayoffPlanDebtDtoMapper : MapperBase<PayoffPlanDebt, PayoffPlanDebtDto>
    {
        [MapperIgnoreTarget(nameof(PayoffPlanDebtDto.DebtName))]
        public override partial PayoffPlanDebtDto Map(PayoffPlanDebt source);

        [MapperIgnoreTarget(nameof(PayoffPlanDebtDto.DebtName))]
        public override partial void Map(PayoffPlanDebt source, PayoffPlanDebtDto destination);
    }
}
