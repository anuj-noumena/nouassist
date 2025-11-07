using NouAssist.Infrastructure.SearchEngine.Localization;
using Volo.Abp.Application.Services;

namespace NouAssist.Infrastructure.SearchEngine;

public abstract class SearchEngineAppService : ApplicationService
{
    protected SearchEngineAppService()
    {
        LocalizationResource = typeof(SearchEngineResource);
        ObjectMapperContext = typeof(SearchEngineApplicationModule);
    }
}
