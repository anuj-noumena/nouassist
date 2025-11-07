using NouAssist.CollaborationHub.ContactManagement.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace NouAssist.CollaborationHub.ContactManagement;

public abstract class ContactManagementController : AbpControllerBase
{
    protected ContactManagementController()
    {
        LocalizationResource = typeof(ContactManagementResource);
    }
}
