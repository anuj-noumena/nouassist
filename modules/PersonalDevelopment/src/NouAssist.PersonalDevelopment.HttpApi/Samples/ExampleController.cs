using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;

namespace NouAssist.PersonalDevelopment.Samples;

[Area(PersonalDevelopmentRemoteServiceConsts.ModuleName)]
[RemoteService(Name = PersonalDevelopmentRemoteServiceConsts.RemoteServiceName)]
[Route("api/personal-development/example")]
public class ExampleController : PersonalDevelopmentController, ISampleAppService
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
