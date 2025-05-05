using UnityEngine;

public class ComponentInteractionHandler : MonoBehaviour
{
    [Header("Selected component")]
    protected UIComponentItem selectedComponent;
    protected RectTransform componentRectTransform;

    protected virtual void Start()
    {

    }
    protected virtual void Update()
    {
        ComponentIconDrag();
    }
    protected void EndDrag()
    {
        selectedComponent = null;
        componentRectTransform = null;
    }
    protected void ComponentIconDrag()
    {
        if (selectedComponent != null)
        {
            componentRectTransform.position = Input.mousePosition;
        }
    }
}
