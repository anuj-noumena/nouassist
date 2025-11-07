using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace NouAssist.Infrastructure.Notifications.Samples;

public interface ISampleAppService : IApplicationService
{
    Task<SampleDto> GetAsync();

    Task<SampleDto> GetAuthorizedAsync();
}
