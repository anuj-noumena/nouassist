using NouAssist.CollaborationHub.ContactManagement.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace NouAssist.CollaborationHub.ContactManagement;

public abstract class ContactsController : AbpControllerBase
{
    protected ContactsController()
    {
        LocalizationResource = typeof(ContactManagementResource);
    }
}
