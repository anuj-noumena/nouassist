using NouAssist.Samples;
using Xunit;

namespace NouAssist.EntityFrameworkCore.Applications;

[Collection(NouAssistTestConsts.CollectionDefinitionName)]
public class EfCoreSampleAppServiceTests : SampleAppServiceTests<NouAssistEntityFrameworkCoreTestModule>
{

}
