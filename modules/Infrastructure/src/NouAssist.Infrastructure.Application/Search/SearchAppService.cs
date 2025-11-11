using NouAssist.Search.Localization;
using Volo.Abp.Application.Services;

namespace NouAssist.Search;

public abstract class SearchAppService : ApplicationService
{
    protected SearchAppService()
    {
        LocalizationResource = typeof(SearchResource);
        ObjectMapperContext = typeof(SearchApplicationModule);
    }
}
