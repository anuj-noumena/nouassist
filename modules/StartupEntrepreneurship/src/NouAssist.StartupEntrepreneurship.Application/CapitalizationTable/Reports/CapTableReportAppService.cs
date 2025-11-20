using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.Localization;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.Permissions;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.Reports.Dtos;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.Repositories;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.Services;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.ValueObjects;
using Volo.Abp;
using Volo.Abp.Application.Services;

namespace NouAssist.StartupEntrepreneurship.CapitalizationTable.Reports
{
    /// <summary>
    /// Application service implementation for CapTable reporting
    /// </summary>
    [Authorize(CapTablePermissions.Reports.Default)]
    public class CapTableReportAppService : ApplicationService, ICapTableReportAppService
    {
        // --- Dependencies ---
        private readonly ICapTableReportingService _reportingService;
        private readonly IDilutionCalculationService _dilutionService;
        private readonly IScenarioModelingService _scenarioService;
        private readonly ICapTableRepository _capTableRepository;

        public CapTableReportAppService(
            ICapTableReportingService reportingService,
            IDilutionCalculationService dilutionService,
            IScenarioModelingService scenarioService,
            ICapTableRepository capTableRepository)
        {
            _reportingService = reportingService;
            _dilutionService = dilutionService;
            _scenarioService = scenarioService;
            _capTableRepository = capTableRepository;

            LocalizationResource = typeof(CapTableResource);
        }

        [Authorize(CapTablePermissions.Reports.ViewOwnership)]
        public virtual async Task<OwnershipReportDto> GetOwnershipReportAsync(Guid capTableId, DateTime asOfDate, bool includeOptions = false)
        {
            try
            {
                var report = await _reportingService.GenerateOwnershipReportAsync(capTableId, asOfDate, includeOptions);
                return ObjectMapper.Map<OwnershipReport, OwnershipReportDto>(report);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["CapTable:ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(CapTablePermissions.Reports.ViewOwnership)]
        public virtual async Task<OwnershipReportDto> GetFullyDilutedOwnershipReportAsync(Guid capTableId, DateTime asOfDate)
        {
            try
            {
                var report = await _reportingService.GenerateFullyDilutedReportAsync(capTableId, asOfDate);
                return ObjectMapper.Map<OwnershipReport, OwnershipReportDto>(report);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["CapTable:ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(CapTablePermissions.Reports.ViewValuation)]
        public virtual async Task<ValuationReportDto> GetValuationReportAsync(Guid capTableId)
        {
            try
            {
                var report = await _reportingService.GenerateValuationReportAsync(capTableId);
                return ObjectMapper.Map<ValuationReport, ValuationReportDto>(report);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["CapTable:ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(CapTablePermissions.Reports.ViewDilution)]
        public virtual async Task<DilutionAnalysisDto> CalculateDilutionImpactAsync(Guid capTableId, decimal newSharesIssued, DateTime effectiveDate)
        {
            try
            {
                var dilutionResults = await _dilutionService.CalculateDilutionForAllHoldersAsync(
                    capTableId, 
                    new ShareQuantity(newSharesIssued), 
                    effectiveDate);

                var capTable = await _capTableRepository.GetAsync(capTableId);

                return new DilutionAnalysisDto
                {
                    CapTableId = capTableId,
                    CompanyName = capTable.CompanyName,
                    NewSharesIssued = newSharesIssued,
                    EffectiveDate = effectiveDate,
                    DilutionImpacts = dilutionResults.Select(kvp => new DilutionImpactDto
                    {
                        HolderId = kvp.Key,
                        // Note: Would need holder names - this would require additional data fetch
                        HolderName = "Unknown", // TODO: Fetch from holder repository
                        HolderType = "Unknown", // TODO: Fetch from holder repository
                        OwnershipBefore = kvp.Value.OwnershipBefore.Value,
                        OwnershipAfter = kvp.Value.OwnershipAfter.Value,
                        DilutionPercentage = kvp.Value.DilutionPercentage,
                        SharesBefore = kvp.Value.SharesBefore.Value,
                        SharesAfter = kvp.Value.SharesAfter.Value
                    }).ToList()
                };
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["CapTable:ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(CapTablePermissions.Reports.ViewDilution)]
        public virtual async Task<DilutionAnalysisDto> CalculateFundingRoundDilutionAsync(Guid capTableId, Guid fundingRoundId)
        {
            try
            {
                var dilutionResults = await _dilutionService.CalculateFundingRoundDilutionAsync(capTableId, fundingRoundId);
                var capTable = await _capTableRepository.GetAsync(capTableId);

                return new DilutionAnalysisDto
                {
                    CapTableId = capTableId,
                    CompanyName = capTable.CompanyName,
                    EffectiveDate = DateTime.UtcNow,
                    DilutionImpacts = dilutionResults.Select(kvp => new DilutionImpactDto
                    {
                        HolderId = kvp.Key,
                        // Note: Would need holder names - this would require additional data fetch
                        HolderName = "Unknown", // TODO: Fetch from holder repository
                        HolderType = "Unknown", // TODO: Fetch from holder repository
                        OwnershipBefore = kvp.Value.OwnershipBefore.Value,
                        OwnershipAfter = kvp.Value.OwnershipAfter.Value,
                        DilutionPercentage = kvp.Value.DilutionPercentage,
                        SharesBefore = kvp.Value.SharesBefore.Value,
                        SharesAfter = kvp.Value.SharesAfter.Value
                    }).ToList()
                };
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["CapTable:ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(CapTablePermissions.Reports.ViewScenarios)]
        public virtual async Task<ScenarioAnalysisDto> ModelFundingRoundScenariosAsync(Guid capTableId, List<FundingRoundScenarioInputDto> scenarios)
        {
            try
            {
                var capTable = await _capTableRepository.GetAsync(capTableId);
                var scenarioResults = new List<ScenarioResultDto>();

                foreach (var scenarioInput in scenarios)
                {
                    var investmentAmount = new InvestmentAmount(scenarioInput.InvestmentAmount, scenarioInput.Currency);
                    var valuation = new Valuation(scenarioInput.PreMoneyValuation, scenarioInput.InvestmentAmount, scenarioInput.Currency);

                    var scenarioResult = await _scenarioService.ModelFundingRoundAsync(
                        capTableId,
                        investmentAmount,
                        valuation,
                        scenarioInput.ScenarioName
                    );

                    scenarioResults.Add(new ScenarioResultDto
                    {
                        ScenarioName = scenarioResult.ScenarioName,
                        InvestmentAmount = scenarioResult.Investment.Value,
                        PreMoneyValuation = scenarioResult.Valuation.PreMoneyValuation,
                        PostMoneyValuation = scenarioResult.Valuation.PostMoneyValuation,
                        PricePerShare = scenarioResult.PricePerShare.Value,
                        NewSharesIssued = scenarioResult.NewSharesIssued.Value,
                        PostInvestmentOwnership = ObjectMapper.Map<OwnershipReport, OwnershipReportDto>(scenarioResult.PostInvestmentOwnership),
                        DilutionAnalysis = new DilutionAnalysisDto
                        {
                            CapTableId = capTableId,
                            CompanyName = capTable.CompanyName,
                            NewSharesIssued = scenarioResult.NewSharesIssued.Value,
                            EffectiveDate = DateTime.UtcNow,
                            DilutionImpacts = scenarioResult.DilutionImpact.Select(kvp => new DilutionImpactDto
                            {
                                HolderId = kvp.Key,
                                OwnershipBefore = kvp.Value.OwnershipBefore.Value,
                                OwnershipAfter = kvp.Value.OwnershipAfter.Value,
                                DilutionPercentage = kvp.Value.DilutionPercentage,
                                SharesBefore = kvp.Value.SharesBefore.Value,
                                SharesAfter = kvp.Value.SharesAfter.Value
                            }).ToList()
                        }
                    });
                }

                return new ScenarioAnalysisDto
                {
                    CapTableId = capTableId,
                    CompanyName = capTable.CompanyName,
                    ScenarioType = "FundingRound",
                    Scenarios = scenarioResults,
                    Comparison = new ScenarioComparisonDto
                    {
                        Metrics = scenarioResults.Select(s => new ComparisonMetricDto
                        {
                            ScenarioName = s.ScenarioName,
                            PostMoneyValuation = s.PostMoneyValuation,
                            // Note: FounderDilution and EmployeeDilution would need more complex calculation
                            FounderDilution = s.DilutionAnalysis.DilutionImpacts.FirstOrDefault()?.DilutionPercentage ?? 0,
                            EmployeeDilution = s.DilutionAnalysis.DilutionImpacts.Skip(1).FirstOrDefault()?.DilutionPercentage ?? 0,
                            OwnershipRetained = 100 - (s.DilutionAnalysis.DilutionImpacts.Sum(d => d.DilutionPercentage))
                        }).ToList()
                    }
                };
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["CapTable:ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(CapTablePermissions.Reports.ViewScenarios)]
        public virtual async Task<ScenarioAnalysisDto> ModelOptionGrantScenariosAsync(Guid capTableId, List<OptionGrantScenarioInputDto> scenarios)
        {
            try
            {
                var capTable = await _capTableRepository.GetAsync(capTableId);
                var scenarioResults = new List<ScenarioResultDto>();

                foreach (var scenarioInput in scenarios)
                {
                    var scenarioResult = await _scenarioService.ModelOptionGrantsAsync(
                        capTableId,
                        new ShareQuantity(scenarioInput.OptionsToGrant),
                        scenarioInput.ScenarioName
                    );

                    scenarioResults.Add(new ScenarioResultDto
                    {
                        ScenarioName = scenarioResult.ScenarioName,
                        NewSharesIssued = scenarioResult.OptionsGranted.Value,
                        PostInvestmentOwnership = ObjectMapper.Map<OwnershipReport, OwnershipReportDto>(scenarioResult.FullyDilutedOwnership),
                        DilutionAnalysis = new DilutionAnalysisDto
                        {
                            CapTableId = capTableId,
                            CompanyName = capTable.CompanyName,
                            NewSharesIssued = scenarioResult.OptionsGranted.Value,
                            EffectiveDate = DateTime.UtcNow,
                            DilutionImpacts = scenarioResult.DilutionImpact.Select(kvp => new DilutionImpactDto
                            {
                                HolderId = kvp.Key,
                                OwnershipBefore = kvp.Value.OwnershipBefore.Value,
                                OwnershipAfter = kvp.Value.OwnershipAfter.Value,
                                DilutionPercentage = kvp.Value.DilutionPercentage,
                                SharesBefore = kvp.Value.SharesBefore.Value,
                                SharesAfter = kvp.Value.SharesAfter.Value
                            }).ToList()
                        }
                    });
                }

                return new ScenarioAnalysisDto
                {
                    CapTableId = capTableId,
                    CompanyName = capTable.CompanyName,
                    ScenarioType = "OptionGrant",
                    Scenarios = scenarioResults,
                    Comparison = new ScenarioComparisonDto
                    {
                        Metrics = scenarioResults.Select(s => new ComparisonMetricDto
                        {
                            ScenarioName = s.ScenarioName,
                            PostMoneyValuation = 0, // Not applicable for option grants
                            FounderDilution = s.DilutionAnalysis.DilutionImpacts.FirstOrDefault()?.DilutionPercentage ?? 0,
                            EmployeeDilution = s.DilutionAnalysis.DilutionImpacts.Skip(1).FirstOrDefault()?.DilutionPercentage ?? 0,
                            OwnershipRetained = 100 - (s.DilutionAnalysis.DilutionImpacts.Sum(d => d.DilutionPercentage))
                        }).ToList()
                    }
                };
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["CapTable:ErrorMessage:DefaultErrorMessage"]);
            }
        }

        // --- Error Handling Helper ---
        protected virtual Task HandleErrorAsync(Exception exception, LogLevel logLevel = LogLevel.Error)
        {
            Logger.Log(logLevel, exception, "Error in CapTableReportAppService: {ErrorMessage}", exception.Message);
            return Task.CompletedTask;
        }
    }
}
