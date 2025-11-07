using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace NouAssist.Infrastructure.Samples;

public interface ISampleAppService : IApplicationService
{
    Task<SampleDto> GetAsync();

    Task<SampleDto> GetAuthorizedAsync();
}
