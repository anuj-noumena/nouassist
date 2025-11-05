using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace NouAssist.Pages;

[Collection(NouAssistTestConsts.CollectionDefinitionName)]
public class Index_Tests : NouAssistWebTestBase
{
    [Fact]
    public async Task Welcome_Page()
    {
        var response = await GetResponseAsStringAsync("/");
        response.ShouldNotBeNull();
    }
}
