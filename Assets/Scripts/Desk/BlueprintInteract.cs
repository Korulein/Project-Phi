using UnityEngine;
using UnityEngine.EventSystems;

public class BlueprintInteract : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("References")]
    private UIComponentItem selectedComponent;
    private RectTransform componentRectTransform;
    private Vector2 mousePos;
    private Vector2Int initialGridPos;

    [Header("Flags")]
    private bool isPointerInside;

    private void Update()
    {
        ComponentIconDrag();
        if (!isPointerInside)
            return;

        //Checks for player input
        mousePos = Input.mousePosition;
        if (Input.GetMouseButtonDown(0))
        {
            Vector2Int cell = BlueprintManager.instance.GetTileGridPosition(mousePos);
            if (BlueprintManager.instance.IsCellUseable(cell))
            {
                LeftMouseButtonPress(cell);
            }
        }
        else if (Input.GetMouseButtonDown(1))
        {
            ReturnComponentToInventory();
        }
    }
    private void LeftMouseButtonPress(Vector2Int cell)
    {
        //Picks up or places component
        if (selectedComponent == null)
        {
            // Checking if cell is empty and if so, doesn't call PickUpComponent
            if (!BlueprintManager.instance.HelperCheckCell(cell.x, cell.y))
                return;

            PickUpComponent(cell);
            initialGridPos = cell;

        }
        else if (selectedComponent != null)
        {
            AttemptToPlaceComponent(cell);
            selectedComponent = null;
            componentRectTransform = null;

        }
    }
    private void AttemptToPlaceComponent(Vector2Int cell)
    {
        //Checks if the component can be placed and if so, fills the appropriate cells
        ComponentData component = selectedComponent.GetComponentData();
        if (selectedComponent.BoundaryCheck(cell.x, cell.y, component.width, component.height))
        {
            if (!BlueprintManager.instance.CheckCellOccupancy(cell, component.width, component.height))
            {
                BlueprintManager.instance.PlaceComponent(selectedComponent, cell.x, cell.y);
            }
            else
            {
                Debug.Log("Component overlap! Try again");
                BlueprintManager.instance.PlaceComponent(selectedComponent, initialGridPos.x, initialGridPos.y);
            }
        }
        else
        {
            Debug.Log("Component was out of bounds!");
            selectedComponent.ReturnToStartPosition();
        }
    }
    private void PickUpComponent(Vector2Int cell)
    {
        //Picks up component and frees up cells
        selectedComponent = BlueprintManager.instance.PickUpComponent(cell.x, cell.y);
        if (selectedComponent != null)
        {
            componentRectTransform = selectedComponent.GetComponent<RectTransform>();
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        isPointerInside = true;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        isPointerInside = false;
    }
    private void ComponentIconDrag()
    {
        //Drags the component icon with the mouse 
        if (selectedComponent != null)
        {
            componentRectTransform.position = Input.mousePosition;
        }
    }
    public void ReturnComponentToInventory()
    {
        if (selectedComponent == null)
            return;
        selectedComponent.ReturnToInventory();
        selectedComponent = null;
        Debug.Log("Returned component to inventory");
    }
}

