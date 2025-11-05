using NouAssist.Samples;
using Xunit;

namespace NouAssist.EntityFrameworkCore.Domains;

[Collection(NouAssistTestConsts.CollectionDefinitionName)]
public class EfCoreSampleDomainTests : SampleDomainTests<NouAssistEntityFrameworkCoreTestModule>
{

}
