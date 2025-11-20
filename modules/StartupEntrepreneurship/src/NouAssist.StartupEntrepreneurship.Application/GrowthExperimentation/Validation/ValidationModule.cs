using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using NouAssist.StartupEntrepreneurship.GrowthExperimentation.ConversionFunnels.Dtos;
using NouAssist.StartupEntrepreneurship.GrowthExperimentation.GrowthExperiments.Dtos;
using NouAssist.StartupEntrepreneurship.GrowthExperimentation.MarketingChannels.Dtos;
using Volo.Abp.Modularity;
using Volo.Abp.Validation;

namespace NouAssist.StartupEntrepreneurship.GrowthExperimentation.Validation
{
    [DependsOn(
        typeof(AbpValidationModule)
    )]
    public class ValidationModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddValidatorsFromAssemblyContaining<ValidationModule>();
            
            // Register validators manually if needed
            context.Services.AddTransient<IValidator<CreateGrowthExperimentDto>, CreateGrowthExperimentDtoValidator>();
            context.Services.AddTransient<IValidator<CreateExperimentVariantDto>, CreateExperimentVariantDtoValidator>();
            context.Services.AddTransient<IValidator<CreateMarketingChannelDto>, CreateMarketingChannelDtoValidator>();
            context.Services.AddTransient<IValidator<UpdateCostPerAcquisitionDto>, UpdateCostPerAcquisitionDtoValidator>();
            context.Services.AddTransient<IValidator<CreateFunnelStageDto>, CreateFunnelStageDtoValidator>();
            context.Services.AddTransient<IValidator<UpdateStageConversionRateDto>, UpdateStageConversionRateDtoValidator>();
        }
    }
}
