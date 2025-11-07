using NouAssist.Infrastructure.SearchEngine.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace NouAssist.Infrastructure.SearchEngine;

public abstract class SearchController : AbpControllerBase
{
    protected SearchController()
    {
        LocalizationResource = typeof(SearchEngineResource);
    }
}
