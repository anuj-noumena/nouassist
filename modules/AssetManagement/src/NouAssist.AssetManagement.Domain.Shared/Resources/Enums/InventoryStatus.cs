namespace NouAssist.AssetManagement.Resources.Enums
{
    public enum InventoryStatus : byte
    {
        Available = 1,
        CheckedOut = 2,
        Reserved = 3,
        Maintenance = 4,
        Repair = 5,
        Lost = 6,
        Retired = 7
    }
}
