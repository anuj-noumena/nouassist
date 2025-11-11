using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace NouAssist.IdentityAccess.Contacts;

[DependsOn(
    typeof(AbpDddDomainModule),
    typeof(ContactsDomainSharedModule)
)]
public class ContactsDomainModule : AbpModule
{

}
