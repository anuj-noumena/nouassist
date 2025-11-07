using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;

namespace NouAssist.WorkflowOrchestration.WorkflowEngine.Samples;

[Area(WorkflowEngineRemoteServiceConsts.ModuleName)]
[RemoteService(Name = WorkflowEngineRemoteServiceConsts.RemoteServiceName)]
[Route("api/workflow-engine/example")]
public class ExampleController : WorkflowEngineController, ISampleAppService
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
