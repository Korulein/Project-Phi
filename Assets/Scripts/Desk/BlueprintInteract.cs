using UnityEngine;
using UnityEngine.EventSystems;

public class BlueprintInteract : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("References")]
    private UIComponentItem selectedComponent;
    private RectTransform componentRectTransform;
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
            if (BlueprintManager.instance.IsCellUseable(cell))
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
            PlaceComponent(cell);
            selectedComponent = null;
            componentRectTransform = null;

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
