using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace NouAssist.ReflectionAndSupport.Journaling.Samples;

public interface ISampleAppService : IApplicationService
{
    Task<SampleDto> GetAsync();

    Task<SampleDto> GetAuthorizedAsync();
}
