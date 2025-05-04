using UnityEngine;

[System.Serializable]
public class BlueprintCellData
{
    public enum CellType
    {
        Standard, //blue
        Required, //purple
        Special, //red
    }
    [Header("Cell Data")]
    public UIComponentItem occupiedBy;
    public bool isOccupied;
    public bool isUseable;
    public CellType type;
    public int width = 1;
    public int height = 1;
}
