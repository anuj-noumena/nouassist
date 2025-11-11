using NouAssist.IdentityAccess.Team.Localization;
using Volo.Abp.Application.Services;

namespace NouAssist.IdentityAccess.Team;

public abstract class TeamAppService : ApplicationService
{
    protected TeamAppService()
    {
        LocalizationResource = typeof(TeamResource);
        ObjectMapperContext = typeof(TeamApplicationModule);
    }
}
