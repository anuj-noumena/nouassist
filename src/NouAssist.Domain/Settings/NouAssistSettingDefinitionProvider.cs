using Volo.Abp.Settings;

namespace NouAssist.Settings;

public class NouAssistSettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        //Define your own settings here. Example:
        //context.Add(new SettingDefinition(NouAssistSettings.MySetting1));
    }
}
