using UnityEngine;
using UnityEngine.UI;

//start here if you mess up
public class UIComponentItem : MonoBehaviour
{
    [Header("UI Element Setup")]
    private Image iconImage;
    private CanvasGroup canvasGroup;
    private RectTransform rectTransform;
    private Transform originalParent;
    private ComponentData component;

    private BlueprintCellData.CellType cellType;
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        iconImage = GetComponent<Image>();
    }
    public void InitializeComponent(ComponentData componentData)
    {
        component = componentData;
        iconImage.sprite = componentData.componentSprite;
        AdjustComponentSize(component);
    }
    public void AdjustComponentSize(ComponentData componentData)
    {
        int pixelSize;
        switch (componentData.slotSize)
        {
            case SlotSize.Small:
                pixelSize = 90;
                rectTransform.sizeDelta = new Vector2(pixelSize, pixelSize);
                break;
            case SlotSize.Medium:
                break;
            case SlotSize.Large:
                break;
        }
    }
    public ComponentData GetComponentData()
    {
        return component;
    }
}