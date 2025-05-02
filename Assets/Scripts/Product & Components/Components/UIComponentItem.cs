using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIComponentItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("UI Element Setup")]
    private Image iconImage;
    private CanvasGroup canvasGroup;
    private RectTransform rectTransform;
    private Transform originalParent;
    private ComponentData component;

    public void InitializeComponent(ComponentData componentData)
    {
        component = componentData;
        iconImage.sprite = componentData.componentSprite;
    }
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        iconImage = GetComponent<Image>();
    }
    // code to drag and drop
    // subject to change and optimisation
    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent;
        transform.SetParent(transform.root);
        canvasGroup.blocksRaycasts = false;
    }
    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.position = Input.mousePosition;
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(originalParent);
        canvasGroup.blocksRaycasts = true;
        rectTransform.localPosition = Vector3.zero;
    }

}