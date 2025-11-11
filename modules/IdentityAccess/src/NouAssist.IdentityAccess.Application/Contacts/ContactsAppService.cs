using NouAssist.IdentityAccess.Contacts.Localization;
using Volo.Abp.Application.Services;

namespace NouAssist.IdentityAccess.Contacts;

public abstract class ContactsAppService : ApplicationService
{
    protected ContactsAppService()
    {
        LocalizationResource = typeof(ContactsResource);
        ObjectMapperContext = typeof(ContactsApplicationModule);
    }
}
