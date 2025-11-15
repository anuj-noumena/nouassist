using Volo.Abp;
using Volo.Abp.Testing;

namespace NouAssist.Learning
{
    public abstract class LearningApplicationTestBase : AbpIntegratedTest<LearningApplicationTestModule>
    {
        protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
        {
            options.UseAutofac();
        }
    }
}
