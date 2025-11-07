using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace NouAssist.CoreProductivity.HabitSystems.Samples;

public interface ISampleAppService : IApplicationService
{
    Task<SampleDto> GetAsync();

    Task<SampleDto> GetAuthorizedAsync();
}
