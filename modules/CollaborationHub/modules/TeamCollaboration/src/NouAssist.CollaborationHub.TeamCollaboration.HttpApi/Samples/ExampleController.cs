using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;

namespace NouAssist.CollaborationHub.TeamCollaboration.Samples;

[Area(TeamCollaborationRemoteServiceConsts.ModuleName)]
[RemoteService(Name = TeamCollaborationRemoteServiceConsts.RemoteServiceName)]
[Route("api/team-collaboration/example")]
public class ExampleController : TeamCollaborationController, ISampleAppService
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
