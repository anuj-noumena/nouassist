using Volo.Abp.Modularity;

namespace NouAssist.Contacts;

[DependsOn(
    typeof(ContactsApplicationModule),
    typeof(ContactsDomainTestModule)
    )]
public class ContactsApplicationTestModule : AbpModule
{

}
