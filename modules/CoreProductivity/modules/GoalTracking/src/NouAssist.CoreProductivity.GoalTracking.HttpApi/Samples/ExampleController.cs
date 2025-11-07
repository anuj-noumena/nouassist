using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;

namespace NouAssist.CoreProductivity.GoalTracking.Samples;

[Area(GoalTrackingRemoteServiceConsts.ModuleName)]
[RemoteService(Name = GoalTrackingRemoteServiceConsts.RemoteServiceName)]
[Route("api/goal-tracking/example")]
public class ExampleController : GoalTrackingController, ISampleAppService
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
