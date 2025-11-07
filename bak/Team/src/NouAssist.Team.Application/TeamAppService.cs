using NouAssist.Team.Localization;
using Volo.Abp.Application.Services;

namespace NouAssist.Team;

public abstract class TeamAppService : ApplicationService
{
    protected TeamAppService()
    {
        LocalizationResource = typeof(TeamResource);
        ObjectMapperContext = typeof(TeamApplicationModule);
    }
}
