using NouAssist.Team.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace NouAssist.Team;

public abstract class TeamController : AbpControllerBase
{
    protected TeamController()
    {
        LocalizationResource = typeof(TeamResource);
    }
}
