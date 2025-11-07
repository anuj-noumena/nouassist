using NouAssist.Infrastructure.SearchEngine.Localization;
using Volo.Abp.Application.Services;

namespace NouAssist.Infrastructure.SearchEngine;

public abstract class SearchAppService : ApplicationService
{
    protected SearchAppService()
    {
        LocalizationResource = typeof(SearchEngineResource);
        ObjectMapperContext = typeof(SearchEngineApplicationModule);
    }
}
