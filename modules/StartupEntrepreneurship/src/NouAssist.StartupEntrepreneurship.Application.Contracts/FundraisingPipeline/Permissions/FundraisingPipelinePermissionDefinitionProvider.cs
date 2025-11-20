using NouAssist.StartupEntrepreneurship.FundraisingPipeline.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace NouAssist.StartupEntrepreneurship.FundraisingPipeline.Permissions
{
    public class FundraisingPipelinePermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var moduleGroup = context.AddGroup(
                FundraisingPipelinePermissions.GroupName,
                L("Permission:FundraisingPipeline"));

            // Prospect permissions
            var prospectPermission = moduleGroup.AddPermission(
                FundraisingPipelinePermissions.Prospect.Default,
                L("Permission:Prospect"));
            
            prospectPermission.AddChild(
                FundraisingPipelinePermissions.Prospect.Create,
                L("Permission:Prospect.Create"));
            
            prospectPermission.AddChild(
                FundraisingPipelinePermissions.Prospect.Update,
                L("Permission:Prospect.Update"));
            
            prospectPermission.AddChild(
                FundraisingPipelinePermissions.Prospect.Delete,
                L("Permission:Prospect.Delete"));
            
            prospectPermission.AddChild(
                FundraisingPipelinePermissions.Prospect.View,
                L("Permission:Prospect.View"));

            // FundraisingRound permissions
            var roundPermission = moduleGroup.AddPermission(
                FundraisingPipelinePermissions.FundraisingRound.Default,
                L("Permission:FundraisingRound"));
            
            roundPermission.AddChild(
                FundraisingPipelinePermissions.FundraisingRound.Create,
                L("Permission:FundraisingRound.Create"));
            
            roundPermission.AddChild(
                FundraisingPipelinePermissions.FundraisingRound.Update,
                L("Permission:FundraisingRound.Update"));
            
            roundPermission.AddChild(
                FundraisingPipelinePermissions.FundraisingRound.Delete,
                L("Permission:FundraisingRound.Delete"));
            
            roundPermission.AddChild(
                FundraisingPipelinePermissions.FundraisingRound.View,
                L("Permission:FundraisingRound.View"));

            // PitchDeck permissions
            var pitchDeckPermission = moduleGroup.AddPermission(
                FundraisingPipelinePermissions.PitchDeck.Default,
                L("Permission:PitchDeck"));
            
            pitchDeckPermission.AddChild(
                FundraisingPipelinePermissions.PitchDeck.Create,
                L("Permission:PitchDeck.Create"));
            
            pitchDeckPermission.AddChild(
                FundraisingPipelinePermissions.PitchDeck.Update,
                L("Permission:PitchDeck.Update"));
            
            pitchDeckPermission.AddChild(
                FundraisingPipelinePermissions.PitchDeck.Delete,
                L("Permission:PitchDeck.Delete"));
            
            pitchDeckPermission.AddChild(
                FundraisingPipelinePermissions.PitchDeck.View,
                L("Permission:PitchDeck.View"));

            // TermSheet permissions
            var termSheetPermission = moduleGroup.AddPermission(
                FundraisingPipelinePermissions.TermSheet.Default,
                L("Permission:TermSheet"));
            
            termSheetPermission.AddChild(
                FundraisingPipelinePermissions.TermSheet.Create,
                L("Permission:TermSheet.Create"));
            
            termSheetPermission.AddChild(
                FundraisingPipelinePermissions.TermSheet.Update,
                L("Permission:TermSheet.Update"));
            
            termSheetPermission.AddChild(
                FundraisingPipelinePermissions.TermSheet.Delete,
                L("Permission:TermSheet.Delete"));
            
            termSheetPermission.AddChild(
                FundraisingPipelinePermissions.TermSheet.View,
                L("Permission:TermSheet.View"));
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<FundraisingPipelineResource>(name);
        }
    }
}
