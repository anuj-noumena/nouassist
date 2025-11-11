using NouAssist.TimeManagement.Meetings.Localization;
using Volo.Abp.Application.Services;

namespace NouAssist.TimeManagement.Meetings;

public abstract class MeetingsAppService : ApplicationService
{
    protected MeetingsAppService()
    {
        LocalizationResource = typeof(MeetingsResource);
        ObjectMapperContext = typeof(MeetingsApplicationModule);
    }
}
