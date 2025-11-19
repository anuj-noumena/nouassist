using System;
using System.Threading.Tasks;
using NouAssist.FinancialManagement.TaxManagement.TaxReturns.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.FinancialManagement.TaxManagement.TaxReturns
{
    public interface ITaxReturnAppService : IApplicationService
    {
        Task<TaxReturnDto> GetAsync(Guid id);
        Task<PagedResultDto<TaxReturnDto>> GetListAsync(GetTaxReturnListDto input);
        Task<TaxReturnDto> CreateAsync(CreateTaxReturnDto input);
        Task<TaxReturnDto> UpdateIncomeAsync(Guid id, UpdateTaxReturnIncomeDto input);
        Task<TaxReturnDto> AddDeductionAsync(Guid id, AddTaxDeductionDto input);
        Task<TaxReturnDto> AttachDocumentAsync(Guid id, AttachTaxDocumentDto input);
        Task<TaxReturnDto> RecordWithholdingsAsync(Guid id, RecordWithholdingsDto input);
        Task<TaxReturnDto> AddEstimatedPaymentAsync(Guid id, AddEstimatedPaymentDto input);
        Task<TaxCalculationResultDto> CalculateTaxAsync(Guid id);
        Task<TaxReturnDto> FileAsync(Guid id, FileTaxReturnDto input);
        Task DeleteAsync(Guid id);
    }
}
