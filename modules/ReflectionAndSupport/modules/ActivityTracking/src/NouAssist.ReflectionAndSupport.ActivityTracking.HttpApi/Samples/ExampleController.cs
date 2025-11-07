using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;

namespace NouAssist.ReflectionAndSupport.ActivityTracking.Samples;

[Area(ActivityTrackingRemoteServiceConsts.ModuleName)]
[RemoteService(Name = ActivityTrackingRemoteServiceConsts.RemoteServiceName)]
[Route("api/activity-tracking/example")]
public class ExampleController : ActivityTrackingController, ISampleAppService
{
    private readonly ISampleAppService _sampleAppService;

    public ExampleController(ISampleAppService sampleAppService)
    {
        _sampleAppService = sampleAppService;
    }

    [HttpGet]
    public async Task<SampleDto> GetAsync()
    {
        return await _sampleAppService.GetAsync();
    }

    [HttpGet]
    [Route("authorized")]
    [Authorize]
    public async Task<SampleDto> GetAuthorizedAsync()
    {
        return await _sampleAppService.GetAsync();
    }
}
