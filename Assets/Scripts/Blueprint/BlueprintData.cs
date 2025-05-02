using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Blueprint", menuName = "Blueprint/Blueprint Data")]
public class BlueprintData : ScriptableObject
{
    [System.Serializable]
    public struct Cell
    {
        public int x;
        public int y;
        public CellType type;
        public CellSize size;
        public int width;
        public int height;
    }
    public enum CellType
    {
        Standard,
        Required,
        Special,
    }
    // 1x1, 2x2, 3x3, custom
    public enum CellSize
    {
        Small = 1,
        Medium = 2,
        Large = 3,
        Custom = 4
    }

    [Header("Blueprint Information")]
    public string blueprintName;
    public Sprite blueprintImage;
    [Range(1, 20)] public int gridWidth = 7;
    [Range(1, 20)] public int gridHeight = 7;
    [Tooltip("Size of each cell in the grid (in pixels)")]
    public float cellSize = 50f;

    [Header("Valid Cells")]
    [Tooltip("List of valid cells where components can be placed")]
    public List<Cell> validCells = new List<Cell>();

    //helper methods
    public bool TryToGetCell(int x, int y, out Cell foundCell)
    {
        foreach (Cell cell in validCells)
        {
            //gets cell Width and Height for every cell
            int cellWidth, cellHeight;
            if (cell.size == CellSize.Custom)
            {
                cellWidth = cell.width;
                cellHeight = cell.height;
            }
            else
            {
                cellWidth = (int)cell.size;
                cellHeight = (int)cell.size;
            }
            //checks if x,y are within the given cell's area
            if (x >= cell.x && x < cell.x + cellWidth && y >= cell.y && y < cell.y + cellHeight)
            {
                foundCell = cell;
                return true;
            }
        }
        foundCell = default;
        return false;
    }

    public bool IsCellValid(int x, int y)
    {
        return TryToGetCell(x, y, out _);
    }
    public CellType GetCellType(int x, int y)
    {
        if (TryToGetCell(x, y, out Cell cell))
        {
            return cell.type;
        }
        return CellType.Standard;
    }
    public CellSize GetCellSize(int x, int y)
    {
        if (TryToGetCell(x, y, out Cell cell))
        {
            return cell.size;
        }
        return CellSize.Small;
    }

}
