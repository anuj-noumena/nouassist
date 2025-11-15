using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using NouAssist.TaxManagement.Aggregates;
using NouAssist.TaxManagement.Data;
using NouAssist.TaxManagement.Permissions;
using NouAssist.TaxManagement.Services;
using NouAssist.TaxManagement.TaxReturns.Dtos;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.TaxManagement.TaxReturns
{
    [Authorize(TaxManagementPermissions.TaxReturns.Default)]
    public class TaxReturnAppService : ApplicationService, ITaxReturnAppService
    {
        private readonly ITaxReturnRepository _taxReturnRepository;
        private readonly TaxCalculationService _taxCalculationService;
        private readonly DeductionValidationService _deductionValidationService;

        public TaxReturnAppService(
            ITaxReturnRepository taxReturnRepository,
            TaxCalculationService taxCalculationService,
            DeductionValidationService deductionValidationService)
        {
            _taxReturnRepository = taxReturnRepository;
            _taxCalculationService = taxCalculationService;
            _deductionValidationService = deductionValidationService;
        }

        public async Task<TaxReturnDto> GetAsync(Guid id)
        {
            var taxReturn = await _taxReturnRepository.GetWithDetailsAsync(id);
            return ObjectMapper.Map<TaxReturn, TaxReturnDto>(taxReturn);
        }

        public async Task<PagedResultDto<TaxReturnDto>> GetListAsync(GetTaxReturnListDto input)
        {
            var userId = CurrentUser.GetId();
            var returns = await _taxReturnRepository.GetUserTaxReturnsAsync(userId);

            if (input.Status.HasValue)
            {
                returns = returns.Where(r => r.Status == input.Status.Value).ToList();
            }

            var totalCount = returns.Count;
            var pagedReturns = returns
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount)
                .ToList();

            return new PagedResultDto<TaxReturnDto>(
                totalCount,
                ObjectMapper.Map<List<TaxReturn>, List<TaxReturnDto>>(pagedReturns)
            );
        }

        [Authorize(TaxManagementPermissions.TaxReturns.Create)]
        public async Task<TaxReturnDto> CreateAsync(CreateTaxReturnDto input)
        {
            var userId = CurrentUser.GetId();

            // Check if return already exists for this year
            var existingReturn = await _taxReturnRepository.GetByUserAndYearAsync(userId, input.TaxYear);
            if (existingReturn != null)
            {
                throw new BusinessException("TaxManagement:ReturnAlreadyExists")
                    .WithData("TaxYear", input.TaxYear);
            }

            var taxReturn = new TaxReturn(
                GuidGenerator.Create(),
                userId,
                CurrentTenant.Id,
                input.TaxYear,
                input.FilingStatus
            );

            await _taxReturnRepository.InsertAsync(taxReturn);

            return ObjectMapper.Map<TaxReturn, TaxReturnDto>(taxReturn);
        }

        [Authorize(TaxManagementPermissions.TaxReturns.Update)]
        public async Task<TaxReturnDto> UpdateIncomeAsync(Guid id, UpdateTaxReturnIncomeDto input)
        {
            var taxReturn = await _taxReturnRepository.GetAsync(id);

            taxReturn.UpdateIncome(
                input.W2Income,
                input.SelfEmploymentIncome,
                input.InvestmentIncome,
                input.OtherIncome
            );

            await _taxReturnRepository.UpdateAsync(taxReturn);

            return ObjectMapper.Map<TaxReturn, TaxReturnDto>(taxReturn);
        }

        [Authorize(TaxManagementPermissions.TaxReturns.Update)]
        public async Task<TaxReturnDto> AddDeductionAsync(Guid id, AddTaxDeductionDto input)
        {
            var taxReturn = await _taxReturnRepository.GetWithDetailsAsync(id);

            // Validate deduction
            var validation = _deductionValidationService.ValidateDeduction(
                input.Category,
                input.ClaimedAmount,
                taxReturn.GrossIncome
            );

            taxReturn.AddDeduction(
                input.Category,
                input.Description,
                input.ClaimedAmount,
                validation.AllowedAmount,
                input.DocumentId
            );

            await _taxReturnRepository.UpdateAsync(taxReturn);

            return ObjectMapper.Map<TaxReturn, TaxReturnDto>(taxReturn);
        }

        [Authorize(TaxManagementPermissions.TaxReturns.Update)]
        public async Task<TaxReturnDto> AttachDocumentAsync(Guid id, AttachTaxDocumentDto input)
        {
            var taxReturn = await _taxReturnRepository.GetWithDetailsAsync(id);

            taxReturn.AttachDocument(
                input.FormType,
                input.DocumentName,
                input.FileId
            );

            await _taxReturnRepository.UpdateAsync(taxReturn);

            return ObjectMapper.Map<TaxReturn, TaxReturnDto>(taxReturn);
        }

        [Authorize(TaxManagementPermissions.TaxReturns.Update)]
        public async Task<TaxReturnDto> RecordWithholdingsAsync(Guid id, RecordWithholdingsDto input)
        {
            var taxReturn = await _taxReturnRepository.GetAsync(id);

            taxReturn.RecordWithholdings(input.Amount);

            await _taxReturnRepository.UpdateAsync(taxReturn);

            return ObjectMapper.Map<TaxReturn, TaxReturnDto>(taxReturn);
        }

        [Authorize(TaxManagementPermissions.TaxReturns.Update)]
        public async Task<TaxReturnDto> AddEstimatedPaymentAsync(Guid id, AddEstimatedPaymentDto input)
        {
            var taxReturn = await _taxReturnRepository.GetWithDetailsAsync(id);

            taxReturn.AddEstimatedPayment(
                input.Quarter,
                input.DueDate,
                input.Amount
            );

            await _taxReturnRepository.UpdateAsync(taxReturn);

            return ObjectMapper.Map<TaxReturn, TaxReturnDto>(taxReturn);
        }

        public async Task<TaxCalculationResultDto> CalculateTaxAsync(Guid id)
        {
            var taxReturn = await _taxReturnRepository.GetWithDetailsAsync(id);

            var totalDeductions = taxReturn.Deductions.Sum(d => d.AllowedAmount);
            var standardDeduction = _taxCalculationService.GetStandardDeduction(taxReturn.FilingStatus);

            var taxCalculation = _taxCalculationService.CalculateTaxLiability(
                taxReturn,
                totalDeductions
            );

            taxReturn.CalculateTax(taxCalculation, standardDeduction);
            await _taxReturnRepository.UpdateAsync(taxReturn);

            return new TaxCalculationResultDto
            {
                GrossIncome = taxCalculation.GrossIncome,
                AdjustedGrossIncome = taxCalculation.AdjustedGrossIncome,
                TaxableIncome = taxCalculation.TaxableIncome,
                TotalTaxLiability = taxCalculation.TotalTaxLiability,
                EffectiveTaxRate = taxCalculation.EffectiveTaxRate,
                MarginalTaxRate = taxCalculation.MarginalTaxRate,
                RefundAmount = taxReturn.RefundAmount,
                AmountOwed = taxReturn.AmountOwed
            };
        }

        [Authorize(TaxManagementPermissions.TaxReturns.File)]
        public async Task<TaxReturnDto> FileAsync(Guid id, FileTaxReturnDto input)
        {
            var taxReturn = await _taxReturnRepository.GetWithDetailsAsync(id);

            taxReturn.MarkAsReadyToFile();
            taxReturn.File(input.ConfirmationNumber);

            await _taxReturnRepository.UpdateAsync(taxReturn);

            return ObjectMapper.Map<TaxReturn, TaxReturnDto>(taxReturn);
        }

        [Authorize(TaxManagementPermissions.TaxReturns.Delete)]
        public async Task DeleteAsync(Guid id)
        {
            await _taxReturnRepository.DeleteAsync(id);
        }
    }
}
