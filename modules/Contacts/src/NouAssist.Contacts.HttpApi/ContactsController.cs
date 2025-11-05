using NouAssist.Contacts.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace NouAssist.Contacts;

public abstract class ContactsController : AbpControllerBase
{
    protected ContactsController()
    {
        LocalizationResource = typeof(ContactsResource);
    }
}
