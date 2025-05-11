using System.Collections;
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
    private bool suppressClickOneFrame = false;
    private void Update()
    {
        if (suppressClickOneFrame)
            return;

        ComponentIconDrag();
        //Checks if mouse is inside blueprint rect transform
        if (isPointerInside)
        {
            // Checks for player input
            mousePos = DeskUIManager.instance.MousePosition;
            if (DeskUIManager.instance.LeftClickDown && DeskUIManager.instance.TryConsumeLeftClick())
            {
                // Converts mouse position to grid cell & checks for useability
                Vector2Int cell = BlueprintManager.instance.GetTileGridPosition(mousePos);
                if (BlueprintManager.instance.IsCellUseable(cell))
                {
                    LeftMouseButtonPress(cell);
                }
            }
            else if (DeskUIManager.instance.RightClickDown)
            {
                ReturnComponentToInventory();
            }
            else if (DeskUIManager.instance.RKeyDown)
            {
                RotateComponent();
            }
        }
        else if (selectedComponent != null && Input.GetMouseButton(0))
        {
            // Returns component to inventory if click is pressed outside the blueprint
            ReturnComponentToInventory();
        }
    }
    private void LeftMouseButtonPress(Vector2Int cell)
    {
        // Checks for click suppression
        if (suppressClickOneFrame)
            return;

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
        if (selectedComponent.BoundaryCheck(cell.x, cell.y, component.playTimeWidth, component.playTimeHeight))
        {
            if (!BlueprintManager.instance.CheckCellOccupancy(cell, component.playTimeWidth, component.playTimeHeight))
            {
                BlueprintManager.instance.PlaceComponent(selectedComponent, cell.x, cell.y);
            }
            else
            {
                if (selectedComponent.currentRotation != selectedComponent.originalRotation)
                {
                    RotateComponent();
                }
                Debug.Log("Component overlap! Try again");
                Vector2Int origin = BlueprintManager.instance.lastPickUpOrigin;
                BlueprintManager.instance.PlaceComponent(selectedComponent, origin.x, origin.y);

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
            selectedComponent.originalRotation = selectedComponent.currentRotation;
            componentRectTransform = selectedComponent.GetComponent<RectTransform>();
        }
    }
    private void RotateComponent()
    {
        // Rotates component
        if (selectedComponent == null)
            return;
        selectedComponent.Rotate();
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
    private void ReturnComponentToInventory()
    {
        // Returns component from blueprint to inventory
        if (selectedComponent == null)
            return;
        if (selectedComponent.isRotated)
            RotateComponent();
        selectedComponent.ReturnToInventory();
        selectedComponent = null;
    }
    public IEnumerator SuppressClickForOneFrame()
    {
        // Suppresses left click for one frame to prevent methods from different scripts 
        // triggering at the same time.
        suppressClickOneFrame = true;
        yield return null; // Waits one frame
        suppressClickOneFrame = false;
    }
}

