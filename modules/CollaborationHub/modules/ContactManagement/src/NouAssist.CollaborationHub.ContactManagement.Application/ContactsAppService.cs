using NouAssist.CollaborationHub.ContactManagement.Localization;
using Volo.Abp.Application.Services;

namespace NouAssist.CollaborationHub.ContactManagement;

public abstract class ContactsAppService : ApplicationService
{
    protected ContactsAppService()
    {
        LocalizationResource = typeof(ContactManagementResource);
        ObjectMapperContext = typeof(ContactManagementApplicationModule);
    }
}
