using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace NouAssist.CollaborationHub.TeamCollaboration.Samples;

public class SampleAppService : TeamCollaborationAppService, ISampleAppService
{
    public Task<SampleDto> GetAsync()
    {
        return Task.FromResult(
            new SampleDto
            {
                Value = 42
            }
        );
    }

    [Authorize]
    public Task<SampleDto> GetAuthorizedAsync()
    {
        return Task.FromResult(
            new SampleDto
            {
                Value = 42
            }
        );
    }
}
