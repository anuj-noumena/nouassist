namespace NouAssist.RequirementsManagement.Elicitation.Constants
{
    /// <summary>
    /// Constants for the Elicitation module
    /// </summary>
    public static class ElicitationConsts
    {
        // StakeholderProfile constants
        public const int MaxStakeholderNameLength = 100;
        public const int MaxStakeholderDescriptionLength = 500;
        public const int MaxRoleLength = 50;
        public const int MaxOrganizationLength = 100;

        // ContactInfo constants
        public const int MaxEmailLength = 254;
        public const int MaxPhoneLength = 20;
        public const int MaxAddressLength = 500;

        // ElicitationSession constants
        public const int MaxSessionTitleLength = 200;
        public const int MaxSessionDescriptionLength = 1000;
        public const int MaxLocationLength = 200;

        // Survey constants
        public const int MaxSurveyTitleLength = 200;
        public const int MaxSurveyDescriptionLength = 1000;
        public const int MaxQuestionTextLength = 500;
        public const int MaxAnswerTextLength = 1000;

        // ElicitationArtifact constants
        public const int MaxArtifactTitleLength = 200;
        public const int MaxArtifactDescriptionLength = 2000;
        public const int MaxStoryTextLength = 500;
        public const int MaxUseCaseNameLength = 100;
        public const int MaxActorNameLength = 50;
        public const int MaxStepDescriptionLength = 300;
        public const int MaxEventNameLength = 100;
        public const int MaxEventDescriptionLength = 500;

        // IdeaTag constants
        public const int MaxTagNameLength = 30;
        public const int MaxTagColorLength = 7; // Hex color

        // Interview/Recording constants
        public const int MaxTranscriptLength = 10000;
        public const int MaxRecordingPathLength = 500;
        public const int MaxFileNameLength = 255;

        // Validation constants
        public const int MinInfluenceLevel = 1;
        public const int MaxInfluenceLevel = 10;
        public const int MinInterestLevel = 1;
        public const int MaxInterestLevel = 10;
    }
}
