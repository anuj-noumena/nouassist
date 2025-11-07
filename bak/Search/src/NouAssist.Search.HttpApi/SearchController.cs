using NouAssist.Search.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace NouAssist.Search;

public abstract class SearchController : AbpControllerBase
{
    protected SearchController()
    {
        LocalizationResource = typeof(SearchResource);
    }
}
