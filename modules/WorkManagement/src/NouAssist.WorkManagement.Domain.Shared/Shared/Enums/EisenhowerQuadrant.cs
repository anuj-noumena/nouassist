namespace NouAssist.WorkManagement.Shared.Enums
{
    public enum EisenhowerQuadrant : byte
    {
        Q1DoFirst = 1,      // Important & Urgent
        Q2Schedule = 2,     // Important, Not Urgent
        Q3Delegate = 3,     // Not Important, Urgent
        Q4Eliminate = 4     // Not Important, Not Urgent
    }
}
