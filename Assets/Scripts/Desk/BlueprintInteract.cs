using UnityEngine;
using UnityEngine.EventSystems;

public class BlueprintInteract : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("References")]
    private UIComponentItem selectedComponent;
    private RectTransform componentRectTransform;
    private UIComponentItem overlapComponent;
    private Vector2 mousePos;

    [Header("Flags")]
    private bool isPointerInside;
    private void Update()
    {
        ComponentIconDrag();
        if (!isPointerInside)
            return;

        mousePos = Input.mousePosition;
        if (Input.GetMouseButtonDown(0))
        {
            Vector2Int cell = BlueprintManager.instance.GetTileGridPosition(mousePos);
            if (BlueprintManager.instance.IsCellUseable(cell) && cell != null)
            {
                LeftMouseButtonPress(cell);
            }
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
            AttemptToPlaceComponent(cell);
            selectedComponent = null;
            componentRectTransform = null;

        }
    }
    private void AttemptToPlaceComponent(Vector2Int cell)
    {

        ComponentData component = selectedComponent.GetComponentData();

        if (selectedComponent.BoundaryCheck(cell.x, cell.y, component.width, component.height))
        {
            /*
            overlapComponent = selectedComponent.GetOverlapComponent();
            bool noOverlap = BlueprintManager.instance.OverlapCheck(
            cell.x,
            cell.y,
            component.width,
            component.height,
            ref overlapComponent);
            if (overlapComponent != null)
            {
                Debug.Log("Component overlaps with another component! Try to place again.");
                selectedComponent.ReturnToStartPosition();
                return;
            }
            */
            BlueprintManager.instance.PlaceComponent(selectedComponent, cell.x, cell.y);
        }
        else
        {
            Debug.Log("Component was out of bounds!");
            selectedComponent.ReturnToStartPosition();
        }
    }
    private void PickUpComponent(Vector2Int cell)
    {
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
        if (selectedComponent != null)
        {
            componentRectTransform.position = Input.mousePosition;
        }
    }
}

