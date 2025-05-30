using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class UIComponentItem : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
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
    public bool isRotated = false;
    private ComponentLocation currentLocation;
    public ComponentRotation currentRotation;
    public ComponentRotation originalRotation;
    public Vector2 originalSizeDelta;
    private Vector2Int gridPos;
    [SerializeField] private float ghostAlpha = 0.5f;

    [Header("Tooltip setup")]
    private string componentName;
    private LTDescr delay;
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

        // Checks for player input
        transform.position = DeskUIManager.instance.MousePosition;
        if (DeskUIManager.instance.LeftClickDown && DeskUIManager.instance.TryConsumeLeftClick())
        {
            AttemptToPlaceInBlueprint();
        }
        if (DeskUIManager.instance.EscapeDown || DeskUIManager.instance.RightClickDown)
        {
            ReturnToStartPosition();
        }
        if (DeskUIManager.instance.RKeyDown)
        {
            Rotate();
        }
    }
    public void InitializeComponent(ComponentData componentData)
    {
        // Initializes component data and adjusts size
        currentLocation = ComponentLocation.Inventory;
        currentRotation = ComponentRotation.NotRotated;
        componentName = componentData.componentName;
        component = componentData;
        component.playTimeWidth = component.width;
        component.playTimeHeight = component.height;
        iconImage.sprite = componentData.componentSprite;
        //AdjustComponentSize(component);
    }
    private void IncreaseComponentSize(ComponentData componentData)
    {
        // Adjusts component size, will be changed later in development
        if (originalSizeDelta == Vector2.zero)
        {
            originalSizeDelta = rectTransform.sizeDelta;
        }
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
    private void PickUpComponent()
    {
        // Picking up component from inventory
        originalParent = transform.parent;
        startPosition = transform.position;
        transform.SetParent(DeskUIManager.instance.dragLayer);

        CreatePlaceHolder();

        IncreaseComponentSize(component);
        isPickedUp = true;
        canvasGroup.blocksRaycasts = false;
        currentLocation = ComponentLocation.DragLayer;
    }
    private void CreatePlaceHolder()
    {
        // Creates blurred placeholder at initial position
        placeholderCopy = Instantiate(gameObject, startPosition, Quaternion.identity, originalParent);
        CanvasGroup copyCanvasGroup = placeholderCopy.GetComponent<CanvasGroup>();

        copyCanvasGroup.alpha = ghostAlpha;
        copyCanvasGroup.blocksRaycasts = false;

        UIComponentItem componentItem = placeholderCopy.GetComponent<UIComponentItem>();
    }
    private void AttemptToPlaceInBlueprint()
    {
        // Converts mouse position to grid position & grabs BlueprintInteract script to call
        // suppress click coroutine.
        gridPos = BlueprintManager.instance.GetTileGridPosition(Input.mousePosition);
        BlueprintInteract blueprintInteract = FindFirstObjectByType<BlueprintInteract>();

        // Checks if cell is useable and within blueprint bounds
        if (BlueprintManager.instance.IsCellUseable(gridPos) && BoundaryCheck(gridPos.x, gridPos.y, component.playTimeWidth, component.playTimeHeight))
        {
            // Checks if cell is empty, otherwise returns component to inventory
            if (!BlueprintManager.instance.CheckCellOccupancy(gridPos, component.playTimeWidth, component.playTimeHeight))
            {
                blueprintInteract.StartCoroutine(blueprintInteract.SuppressClickForOneFrame());
                BlueprintManager.instance.PlaceComponent(this, gridPos.x, gridPos.y);
                isPickedUp = false;
                canvasGroup.blocksRaycasts = false;
            }
            else
            {
                blueprintInteract.StartCoroutine(blueprintInteract.SuppressClickForOneFrame());
                Debug.Log("Component overlap! Try again");
                ReturnToStartPosition();
            }
        }
        else
        {
            ReturnToStartPosition();
        }
    }
    public void Rotate()
    {
        if (component.slotSize == SlotSize.Small || component.slotSize == SlotSize.Medium || component.slotSize == SlotSize.Large)
            return;
        currentRotation = (currentRotation == ComponentRotation.Rotated) ? ComponentRotation.NotRotated : ComponentRotation.Rotated;
        isRotated = !isRotated;
        SwapDimensions();

        rectTransform = GetComponent<RectTransform>();
        rectTransform.rotation = Quaternion.Euler(0, 0, isRotated ? 90f : 0f);
    }
    private void SwapDimensions()
    {
        int aux = component.playTimeHeight;
        component.playTimeHeight = component.playTimeWidth;
        component.playTimeWidth = aux;
    }
    public void ReturnToStartPosition()
    {
        // Returns component to initial position in inventory
        if (isRotated)
            Rotate();
        transform.SetParent(originalParent);
        rectTransform.position = startPosition;
        currentLocation = ComponentLocation.Inventory;

        isPickedUp = false;
        canvasGroup.blocksRaycasts = true;

        rectTransform.sizeDelta = originalSizeDelta;
        Destroy(placeholderCopy);
    }
    public void ReturnToInventory()
    {
        // Called from BlueprintInteract, returns component to inventory
        canvasGroup.blocksRaycasts = true;
        isPickedUp = false;

        RectTransform rt = GetComponent<RectTransform>();
        rt.SetParent(DeskUIManager.instance.inventoryContainer, false);

        ReturnToStartPosition();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
            return;
        if (isPickedUp)
            return;
        if (currentLocation == ComponentLocation.Inventory)
        {
            PickUpComponent();
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        delay = LeanTween.delayedCall(0.3f, () =>
        {
            TooltipManager.instance.ShowTooltip(componentName);
        });
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        LeanTween.cancel(delay.uniqueId);
        TooltipManager.instance.HideTooltip();
    }
    public ComponentData GetComponentData()
    {
        return component;
    }
    public bool BoundaryCheck(int posX, int posY, int width, int height)
    {
        // Check if the starting position is valid
        if (!PositionCheck(posX, posY))
            return false;

        // Check if all cells in the rectangle are valid
        if (!BlueprintManager.instance.CheckCellBoundary(posX, posY, posX + width, posY + height))
            return false;

        posX += width;
        posY += height;

        // Check if the ending position is valid
        if (!PositionCheck(posX, posY))
            return false;

        return true;
    }
    private bool PositionCheck(int posX, int posY)
    {
        if (posX < 0 || posY < 0)
            return false;

        if (posX > BlueprintManager.instance.blueprintInUse.gridWidth || posY > BlueprintManager.instance.blueprintInUse.gridHeight)
            return false;

        return true;
    }
    private enum ComponentLocation
    {
        Inventory,
        Blueprint,
        DragLayer
    }
    public enum ComponentRotation
    {
        Rotated,
        NotRotated,
    }
}