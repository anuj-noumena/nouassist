using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace NouAssist.ReflectionAndSupport.Reviews.Samples;

public interface ISampleAppService : IApplicationService
{
    Task<SampleDto> GetAsync();

    Task<SampleDto> GetAuthorizedAsync();
}
