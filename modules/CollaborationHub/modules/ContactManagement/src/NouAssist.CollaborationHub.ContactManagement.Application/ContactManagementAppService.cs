using NouAssist.CollaborationHub.ContactManagement.Localization;
using Volo.Abp.Application.Services;

namespace NouAssist.CollaborationHub.ContactManagement;

public abstract class ContactManagementAppService : ApplicationService
{
    protected ContactManagementAppService()
    {
        LocalizationResource = typeof(ContactManagementResource);
        ObjectMapperContext = typeof(ContactManagementApplicationModule);
    }
}
