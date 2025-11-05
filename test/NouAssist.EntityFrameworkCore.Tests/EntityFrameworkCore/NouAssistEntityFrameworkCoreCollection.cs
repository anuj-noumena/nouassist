using Xunit;

namespace NouAssist.EntityFrameworkCore;

[CollectionDefinition(NouAssistTestConsts.CollectionDefinitionName)]
public class NouAssistEntityFrameworkCoreCollection : ICollectionFixture<NouAssistEntityFrameworkCoreFixture>
{

}
