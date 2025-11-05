using Microsoft.AspNetCore.Builder;
using NouAssist;
using Volo.Abp.AspNetCore.TestBase;

var builder = WebApplication.CreateBuilder();
builder.Environment.ContentRootPath = GetWebProjectContentRootPathHelper.Get("NouAssist.Web.csproj"); 
await builder.RunAbpModuleAsync<NouAssistWebTestModule>(applicationName: "NouAssist.Web");

namespace NouAssist
{
    public partial class Program
    {
    }
}
