using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class UIComponentItem : MonoBehaviour, IPointerClickHandler
{
    [Header("UI Element Setup")]
    private Image iconImage;
    private CanvasGroup canvasGroup;
    private RectTransform rectTransform;
    private Transform originalParent;
    private GameObject placeholderCopy;
    private ComponentData component;
    private Vector2 startPosition;
    private bool isPickedUp = false;
    private ComponentLocation currentLocation;
    [SerializeField] private float ghostAlpha = 0.5f;
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        iconImage = GetComponent<Image>();
    }
    private void Update()
    {
        if (!isPickedUp)
            return;

        transform.position = Input.mousePosition;
        if (Input.GetMouseButtonDown(0))
        {
            TryToPlaceInBlueprint();
        }
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(1))
        {
            ReturnToStartPosition();
        }
    }
    public void InitializeComponent(ComponentData componentData)
    {
        currentLocation = ComponentLocation.Inventory;
        component = componentData;
        iconImage.sprite = componentData.componentSprite;
        AdjustComponentSize(component);
    }
    public void AdjustComponentSize(ComponentData componentData)
    {
        int pixelSize = 90;
        switch (componentData.slotSize)
        {
            case SlotSize.Small:
                rectTransform.sizeDelta = new Vector2(pixelSize, pixelSize);
                break;
            case SlotSize.Medium:
                rectTransform.sizeDelta = new Vector2(pixelSize * 2, pixelSize * 2);
                break;
            case SlotSize.Large:
                rectTransform.sizeDelta = new Vector2(pixelSize * 3, pixelSize * 3);
                break;
            case SlotSize.Custom:
                rectTransform.sizeDelta = new Vector2(pixelSize * componentData.width, pixelSize * componentData.height);
                break;
        }
    }
    private void TryToPlaceInBlueprint()
    {
        Vector2Int gridPos = BlueprintManager.instance.GetTileGridPosition(Input.mousePosition);
        if (BlueprintManager.instance.IsCellUseable(gridPos))
        {
            BlueprintManager.instance.PlaceComponent(this, gridPos.x, gridPos.y);
            isPickedUp = false;
            canvasGroup.blocksRaycasts = false;
        }
        else
        {
            ReturnToStartPosition();
        }
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (isPickedUp)
            return;
        if (currentLocation == ComponentLocation.Inventory)
        {
            PickUp();
        }
    }
    private void PickUp()
    {
        originalParent = transform.parent;
        startPosition = transform.position;
        transform.SetParent(DeskUIManager.instance.dragLayer);

        CreatePlaceHolder();

        isPickedUp = true;
        canvasGroup.blocksRaycasts = false;
        currentLocation = ComponentLocation.DragLayer;
    }
    private void CreatePlaceHolder()
    {
        placeholderCopy = Instantiate(gameObject, startPosition, Quaternion.identity, originalParent);
        CanvasGroup copyCanvasGroup = placeholderCopy.GetComponent<CanvasGroup>();

        copyCanvasGroup.alpha = ghostAlpha;
        copyCanvasGroup.blocksRaycasts = false;

        UIComponentItem componentItem = placeholderCopy.GetComponent<UIComponentItem>();
    }
    public void ReturnToStartPosition()
    {
        transform.SetParent(originalParent);
        rectTransform.position = startPosition;
        currentLocation = ComponentLocation.Inventory;

        isPickedUp = false;
        canvasGroup.blocksRaycasts = true;

        Destroy(placeholderCopy);
    }

    private enum ComponentLocation
    {
        Inventory,
        Blueprint,
        DragLayer
    }
}