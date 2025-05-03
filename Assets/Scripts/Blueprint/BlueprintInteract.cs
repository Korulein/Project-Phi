using UnityEngine;
using UnityEngine.EventSystems;

public class BlueprintInteract : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Blueprint cells")]
    private BlueprintCellData selectedBlueprintCell;

    [Header("Blueprint References")]
    private BlueprintData currentBlueprint;
    private BlueprintCellData[,] grid;

    [Header("Flags")]
    private bool isPointerInside;
    private void Update()
    {
        if (!isPointerInside)
            return;

        Vector2 mousePos = Input.mousePosition;
        Vector2Int cell = BlueprintManager.instance.GetTileGridPosition(mousePos);

        if (IsCellUseable(cell))
        {
            selectedBlueprintCell = GetUseableCell(cell);
            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log($"Hovering over useable cell at [{cell.y}], [{cell.x}].");
            }
        }
    }
    public bool IsCellUseable(Vector2Int cell)
    {
        currentBlueprint = BlueprintManager.instance.blueprintInUse;
        if (cell.x < 0 || cell.y < 0 || cell.x >= currentBlueprint.gridWidth || cell.y >= currentBlueprint.gridHeight)
        {
            return false;
        }
        grid = BlueprintManager.instance.GetBlueprintGrid();
        return grid[cell.x, cell.y].isUseable;
    }
    public BlueprintCellData GetUseableCell(Vector2Int cell)
    {
        currentBlueprint = BlueprintManager.instance.blueprintInUse;
        grid = BlueprintManager.instance.GetBlueprintGrid();
        return grid[cell.x, cell.y];
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        isPointerInside = true;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        isPointerInside = false;
    }
}
