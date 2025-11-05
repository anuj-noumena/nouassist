using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace NouAssist.Projects.Samples;

public interface ISampleAppService : IApplicationService
{
    Task<SampleDto> GetAsync();

    Task<SampleDto> GetAuthorizedAsync();
}
