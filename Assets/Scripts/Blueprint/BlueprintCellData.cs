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
    public ComponentData occupiedBy;
    public bool isOccupied => occupiedBy != null;
    public bool isUseable;
    public CellType type;
    public int width = 1;
    public int height = 1;
}
