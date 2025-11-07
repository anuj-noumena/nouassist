using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;

namespace NouAssist.ReflectionAndSupport.Samples;

[Area(ReflectionAndSupportRemoteServiceConsts.ModuleName)]
[RemoteService(Name = ReflectionAndSupportRemoteServiceConsts.RemoteServiceName)]
[Route("api/reflection-and-support/example")]
public class ExampleController : ReflectionAndSupportController, ISampleAppService
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
