using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NouAssist.IntelligenceStrategy.DecisionSupport.Samples;
using Volo.Abp;

namespace NouAssist.IntelligenceStrategy.Samples;

[Area(IntelligenceStrategyRemoteServiceConsts.ModuleName)]
[RemoteService(Name = IntelligenceStrategyRemoteServiceConsts.RemoteServiceName)]
[Route("api/intelligence-strategy/example")]
public class ExampleController : IntelligenceStrategyController, ISampleAppService
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
