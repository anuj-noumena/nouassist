using NouAssist.Infrastructure.SearchEngine.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace NouAssist.Infrastructure.SearchEngine;

public abstract class SearchEngineController : AbpControllerBase
{
    protected SearchEngineController()
    {
        LocalizationResource = typeof(SearchEngineResource);
    }
}
