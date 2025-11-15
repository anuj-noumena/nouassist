using Volo.Abp;
using Volo.Abp.Testing;

namespace NouAssist.SkillDevelopment
{
    public abstract class SkillDevelopmentApplicationTestBase : AbpIntegratedTest<SkillDevelopmentApplicationTestModule>
    {
        protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
        {
            options.UseAutofac();
        }
    }
}
