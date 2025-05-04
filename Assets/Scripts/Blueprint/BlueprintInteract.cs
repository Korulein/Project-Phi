using UnityEngine;
using UnityEngine.EventSystems;

public class BlueprintInteract : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Blueprint cell")]
    private BlueprintCellData selectedBlueprintCell;

    [Header("References")]
    private BlueprintData currentBlueprint;
    private BlueprintCellData[,] grid;
    private UIComponentItem selectedComponent;
    private RectTransform componentRectTransform;

    [Header("Flags")]
    private bool isPointerInside;
    private void Update()
    {
        if (selectedComponent != null)
        {
            componentRectTransform.position = Input.mousePosition;
        }
        if (!isPointerInside)
            return;

        Vector2 mousePos = Input.mousePosition;
        Vector2Int cell = BlueprintManager.instance.GetTileGridPosition(mousePos);

        if (IsCellUseable(cell))
        {
            selectedBlueprintCell = GetUseableCell(cell);
            if (Input.GetMouseButtonDown(0))
            {
                BlueprintCellData[,] tempGrid = BlueprintManager.instance.GetBlueprintGrid();

                if (selectedComponent == null)
                {
                    selectedComponent = BlueprintManager.instance.PickUpComponent(cell.x, cell.y);
                    if (selectedComponent != null)
                    {
                        componentRectTransform = selectedComponent.GetComponent<RectTransform>();
                    }
                }
                else
                {
                    BlueprintManager.instance.PlaceComponent(selectedComponent, cell.x, cell.y);
                    selectedComponent = null;
                }
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
