using AutoMapper;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.Aggregates;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.CapTables.Dtos;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.Entities;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.FundingRounds.Dtos;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.OptionPools.Dtos;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.Reports.Dtos;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.SecurityHolders.Dtos;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.SecurityIssuances.Dtos;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.Services;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.ValueObjects;

namespace NouAssist.StartupEntrepreneurship.CapitalizationTable.Mapping
{
    /// <summary>
    /// AutoMapper profile for CapTable application layer
    /// </summary>
    public class CapTableApplicationAutoMapperProfile : Profile
    {
        public CapTableApplicationAutoMapperProfile()
        {
            // --- Aggregate <-> DTO Mappings ---
            
            // CapTable mappings
            CreateMap<Aggregates.CapTable, CapTableDto>()
                .ForMember(dest => dest.TotalAuthorizedShares, opt => opt.MapFrom(src => src.TotalAuthorizedShares.Value))
                .ForMember(dest => dest.TotalIssuedShares, opt => opt.MapFrom(src => src.TotalIssuedShares.Value))
                .ForMember(dest => dest.TotalOutstandingShares, opt => opt.MapFrom(src => src.TotalOutstandingShares.Value));

            // SecurityHolder mappings
            CreateMap<SecurityHolder, SecurityHolderDto>()
                .ForMember(dest => dest.HolderType, opt => opt.MapFrom(src => src.HolderType.ToString()))
                .ForMember(dest => dest.VestingScheduleCount, opt => opt.MapFrom(src => src.VestingSchedules.Count));

            // SecurityIssuance mappings
            CreateMap<SecurityIssuance, SecurityIssuanceDto>()
                .ForMember(dest => dest.SecurityType, opt => opt.MapFrom(src => src.SecurityType.ToString()))
                .ForMember(dest => dest.TotalAuthorized, opt => opt.MapFrom(src => src.TotalAuthorized.Value))
                .ForMember(dest => dest.TotalIssued, opt => opt.MapFrom(src => src.TotalIssued.Value))
                .ForMember(dest => dest.TotalOutstanding, opt => opt.MapFrom(src => src.TotalOutstanding.Value))
                .ForMember(dest => dest.IssuePriceValue, opt => opt.MapFrom(src => src.IssuePrice.Value))
                .ForMember(dest => dest.IssuePriceCurrency, opt => opt.MapFrom(src => src.IssuePrice.Currency))
                .ForMember(dest => dest.OptionStrikePrice, opt => opt.MapFrom(src => src.OptionDetails != null ? src.OptionDetails.StrikePrice : (decimal?)null))
                .ForMember(dest => dest.OptionExpiryDate, opt => opt.MapFrom(src => src.OptionDetails != null ? src.OptionDetails.ExpiryDate : (DateTime?)null))
                .ForMember(dest => dest.OptionCurrency, opt => opt.MapFrom(src => src.OptionDetails != null ? src.OptionDetails.Currency : null));

            // FundingRound mappings
            CreateMap<FundingRound, FundingRoundDto>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.PreMoneyValuation, opt => opt.MapFrom(src => src.Valuation.PreMoneyValuation))
                .ForMember(dest => dest.PostMoneyValuation, opt => opt.MapFrom(src => src.Valuation.PostMoneyValuation))
                .ForMember(dest => dest.ValuationCurrency, opt => opt.MapFrom(src => src.Valuation.Currency))
                .ForMember(dest => dest.TargetAmount, opt => opt.MapFrom(src => src.TargetAmount.Value))
                .ForMember(dest => dest.RaisedAmount, opt => opt.MapFrom(src => src.RaisedAmount.Value))
                .ForMember(dest => dest.Currency, opt => opt.MapFrom(src => src.TargetAmount.Currency))
                .ForMember(dest => dest.SharesIssued, opt => opt.MapFrom(src => src.SharesIssued.Value))
                .ForMember(dest => dest.PricePerShare, opt => opt.MapFrom(src => src.PricePerShare.Value));

            // OptionPool mappings
            CreateMap<OptionPool, OptionPoolDto>()
                .ForMember(dest => dest.TotalReserved, opt => opt.MapFrom(src => src.TotalReserved.Value))
                .ForMember(dest => dest.TotalGranted, opt => opt.MapFrom(src => src.TotalGranted.Value))
                .ForMember(dest => dest.TotalExercised, opt => opt.MapFrom(src => src.TotalExercised.Value))
                .ForMember(dest => dest.TotalForfeited, opt => opt.MapFrom(src => src.TotalForfeited.Value))
                .ForMember(dest => dest.PoolPercentage, opt => opt.MapFrom(src => src.PoolPercentage.Value))
                .ForMember(dest => dest.DefaultVestingPeriodMonths, opt => opt.MapFrom(src => src.DefaultVestingTerms.VestingPeriodMonths))
                .ForMember(dest => dest.DefaultCliffMonths, opt => opt.MapFrom(src => src.DefaultVestingTerms.CliffMonths))
                .ForMember(dest => dest.DefaultVestingPercentage, opt => opt.MapFrom(src => src.DefaultVestingTerms.VestingPercentage));

            // --- Entity <-> DTO Mappings ---
            
            // ShareClass mappings
            CreateMap<ShareClass, ShareClassDto>()
                .ForMember(dest => dest.SecurityType, opt => opt.MapFrom(src => src.SecurityType.ToString()));

            // VestingSchedule mappings
            CreateMap<VestingSchedule, VestingScheduleDto>()
                .ForMember(dest => dest.VestingPeriodMonths, opt => opt.MapFrom(src => src.VestingTerms.VestingPeriodMonths))
                .ForMember(dest => dest.CliffMonths, opt => opt.MapFrom(src => src.VestingTerms.CliffMonths))
                .ForMember(dest => dest.VestingPercentage, opt => opt.MapFrom(src => src.VestingTerms.VestingPercentage))
                .ForMember(dest => dest.TotalShares, opt => opt.MapFrom(src => src.TotalShares.Value))
                .ForMember(dest => dest.VestedShares, opt => opt.MapFrom(src => src.VestedShares.Value))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.SecurityName, opt => opt.Ignore()) // Would need to be populated separately
                .ForMember(dest => dest.SecurityType, opt => opt.Ignore()); // Would need to be populated separately

            // --- Domain Service <-> DTO Mappings ---
            
            // VestingScheduleInfo mappings
            CreateMap<VestingScheduleInfo, VestingScheduleDto>()
                .ForMember(dest => dest.VestingPeriodMonths, opt => opt.MapFrom(src => src.VestingTerms.VestingPeriodMonths))
                .ForMember(dest => dest.CliffMonths, opt => opt.MapFrom(src => src.VestingTerms.CliffMonths))
                .ForMember(dest => dest.VestingPercentage, opt => opt.MapFrom(src => src.VestingTerms.VestingPercentage))
                .ForMember(dest => dest.TotalShares, opt => opt.MapFrom(src => src.TotalShares.Value))
                .ForMember(dest => dest.VestedShares, opt => opt.MapFrom(src => src.VestedShares.Value))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.SecurityName, opt => opt.Ignore())
                .ForMember(dest => dest.SecurityType, opt => opt.Ignore());

            // --- Report Mappings ---
            
            // OwnershipReport mappings
            CreateMap<OwnershipReport, OwnershipReportDto>()
                .ForMember(dest => dest.CompanyName, opt => opt.Ignore()) // Set separately
                .ForMember(dest => dest.TotalShares, opt => opt.MapFrom(src => src.TotalShares.Value));

            CreateMap<OwnershipReport.OwnershipEntry, OwnershipEntryDto>()
                .ForMember(dest => dest.Shares, opt => opt.MapFrom(src => src.Shares.Value))
                .ForMember(dest => dest.OwnershipPercentage, opt => opt.MapFrom(src => src.OwnershipPercentage.Value))
                .ForMember(dest => dest.VestedShares, opt => opt.MapFrom(src => src.VestedShares.Value))
                .ForMember(dest => dest.UnvestedShares, opt => opt.MapFrom(src => src.UnvestedShares.Value));

            // ValuationReport mappings
            CreateMap<ValuationReport, ValuationReportDto>()
                .ForMember(dest => dest.CompanyName, opt => opt.Ignore()); // Set separately

            CreateMap<ValuationReport.ValuationEntry, ValuationEntryDto>()
                .ForMember(dest => dest.PreMoneyValuation, opt => opt.MapFrom(src => src.Valuation.PreMoneyValuation))
                .ForMember(dest => dest.PostMoneyValuation, opt => opt.MapFrom(src => src.Valuation.PostMoneyValuation))
                .ForMember(dest => dest.Currency, opt => opt.MapFrom(src => src.Valuation.Currency));

            // Current valuation mapping
            CreateMap<ValuationHistory, ValuationEntryDto>()
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.ValuationDate))
                .ForMember(dest => dest.PreMoneyValuation, opt => opt.MapFrom(src => src.Valuation.PreMoneyValuation))
                .ForMember(dest => dest.PostMoneyValuation, opt => opt.MapFrom(src => src.Valuation.PostMoneyValuation))
                .ForMember(dest => dest.Currency, opt => opt.MapFrom(src => src.Valuation.Currency))
                .ForMember(dest => dest.Source, opt => opt.MapFrom(src => src.Source))
                .ForMember(dest => dest.Notes, opt => opt.MapFrom(src => src.Notes));

            // --- Value Object Mappings ---
            
            // Note: Value objects are typically mapped through their properties in the containing entity mappings above.
            // If needed, you can add specific value object mappings here.
        }
    }
}
