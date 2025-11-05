using NouAssist.Meetings.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace NouAssist.Meetings;

public abstract class MeetingsController : AbpControllerBase
{
    protected MeetingsController()
    {
        LocalizationResource = typeof(MeetingsResource);
    }
}
