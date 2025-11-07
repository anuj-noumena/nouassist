using NouAssist.Meetings.Localization;
using Volo.Abp.Application.Services;

namespace NouAssist.Meetings;

public abstract class MeetingsAppService : ApplicationService
{
    protected MeetingsAppService()
    {
        LocalizationResource = typeof(MeetingsResource);
        ObjectMapperContext = typeof(MeetingsApplicationModule);
    }
}
