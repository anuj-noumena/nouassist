using NouAssist.Contacts.Localization;
using Volo.Abp.Application.Services;

namespace NouAssist.Contacts;

public abstract class ContactsAppService : ApplicationService
{
    protected ContactsAppService()
    {
        LocalizationResource = typeof(ContactsResource);
        ObjectMapperContext = typeof(ContactsApplicationModule);
    }
}
