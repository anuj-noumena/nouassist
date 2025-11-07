using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace NouAssist.ReflectionAndSupport.ActivityTracking.Samples;

public interface ISampleAppService : IApplicationService
{
    Task<SampleDto> GetAsync();

    Task<SampleDto> GetAuthorizedAsync();
}
