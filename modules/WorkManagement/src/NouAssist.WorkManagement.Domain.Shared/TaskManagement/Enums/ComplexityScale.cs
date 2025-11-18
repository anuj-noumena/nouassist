namespace NouAssist.WorkManagement.TaskManagement.Enums
{
    /// <summary>
    /// Defines the type of complexity estimation scale being used
    /// </summary>
    public enum ComplexityScale : byte
    {
        /// <summary>
        /// Fibonacci sequence: 1, 2, 3, 5, 8, 13, 21, etc.
        /// </summary>
        Fibonacci = 1,

        /// <summary>
        /// T-shirt sizing: XS, S, M, L, XL, XXL (represented as 1-6)
        /// </summary>
        TShirt = 2,

        /// <summary>
        /// Numeric scale: 1-10
        /// </summary>
        Numeric = 3,
        
        Unknown = 4
    }
}
