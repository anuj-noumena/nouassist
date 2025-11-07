using Volo.Abp.Modularity;

namespace NouAssist.Contacts;

[DependsOn(
    typeof(ContactsDomainModule),
    typeof(ContactsTestBaseModule)
)]
public class ContactsDomainTestModule : AbpModule
{

}
