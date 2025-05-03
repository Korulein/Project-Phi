using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Blueprint", menuName = "Blueprint/Blueprint Data")]
public class BlueprintData : ScriptableObject
{
    [Header("Blueprint Information")]
    public string blueprintName;
    public Sprite blueprintImage;
    public int blueprintID;
    [Range(1, 20)] public int gridWidth;
    [Range(1, 20)] public int gridHeight;

    [Header("All Cells")]
    [Tooltip("All cells in the blueprint")]
    public List<BlueprintCellData> allCells = new List<BlueprintCellData>();
}
