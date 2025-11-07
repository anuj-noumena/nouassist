using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace NouAssist.CollaborationHub.Resources.Samples;

public interface ISampleAppService : IApplicationService
{
    Task<SampleDto> GetAsync();

    Task<SampleDto> GetAuthorizedAsync();
}
