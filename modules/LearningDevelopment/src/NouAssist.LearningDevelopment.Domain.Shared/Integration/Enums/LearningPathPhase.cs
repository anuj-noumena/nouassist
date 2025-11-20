namespace NouAssist.LearningDevelopment.Integration.Enums
{
    /// <summary>
    /// Represents the integrated learning path phases combining theory and practice
    /// </summary>
    public enum LearningPathPhase : byte
    {
        /// <summary>
        /// Initial planning and goal setting
        /// </summary>
        Planning = 1,

        /// <summary>
        /// Theory phase: Learning the concepts (Learning phases 1-4)
        /// </summary>
        TheoryLearning = 2,

        /// <summary>
        /// Transition phase: Bloom assessment and skill assessment
        /// </summary>
        Transition = 3,

        /// <summary>
        /// Practice phase: Developing practical skills (SkillDevelopment steps 2-5)
        /// </summary>
        PracticalApplication = 4,

        /// <summary>
        /// Combined mastery: Both theory and practice mastered
        /// </summary>
        IntegratedMastery = 5,

        /// <summary>
        /// Maintenance: Spaced repetition and ongoing practice
        /// </summary>
        Maintenance = 6
    }
}
