using UnityEngine;
using UnityEngine.EventSystems;

public class BlueprintInteract : ComponentInteractionHandler, IPointerEnterHandler, IPointerExitHandler
{
    [Header("References")]
    private BlueprintData currentBlueprint;
    private BlueprintCellData[,] grid;

    [Header("Flags")]
    private bool isPointerInside;
    protected override void Update()
    {
        base.Update();
        if (!isPointerInside)
            return;

        Vector2 mousePos = Input.mousePosition;
        Vector2Int cell = BlueprintManager.instance.GetTileGridPosition(mousePos);

        if (IsCellUseable(cell) && Input.GetMouseButtonDown(0))
        {
            LeftMouseButtonPress(cell);
        }
    }
    private void LeftMouseButtonPress(Vector2Int cell)
    {
        if (selectedComponent == null)
        {
            PickUpComponent(cell);
        }
        else
        {
            PlaceComponent(cell);
            EndDrag();
        }
    }
    private void PlaceComponent(Vector2Int cell)
    {
        BlueprintManager.instance.PlaceComponent(selectedComponent, cell.x, cell.y);
    }
    private void PickUpComponent(Vector2Int cell)
    {
        selectedComponent = BlueprintManager.instance.PickUpComponent(cell.x, cell.y);
        if (selectedComponent != null)
        {
            componentRectTransform = selectedComponent.GetComponent<RectTransform>();
        }
    }
    public bool IsCellUseable(Vector2Int cell)
    {
        grid = BlueprintManager.instance.GetBlueprintGrid();
        currentBlueprint = BlueprintManager.instance.blueprintInUse;
        return cell.x >= 0 && cell.y >= 0 &&
               cell.x < currentBlueprint.gridWidth &&
               cell.y < currentBlueprint.gridHeight &&
               grid[cell.x, cell.y].isUseable;
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
