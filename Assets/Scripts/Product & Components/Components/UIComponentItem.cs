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
    private BlueprintInteract blueprint;
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        iconImage = GetComponent<Image>();
        blueprint = DeskUIManager.instance.blueprintGridContainer.GetComponentInParent<BlueprintInteract>();
    }
    private void Update()
    {
        if (!isPickedUp)
            return;

        // Checks for player input
        transform.position = Input.mousePosition;
        if (Input.GetMouseButtonDown(0))
        {
            AttemptToPlaceInBlueprint();
        }
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(1))
        {
            ReturnToStartPosition();
        }
    }
    public void InitializeComponent(ComponentData componentData)
    {
        // Initializes component data and adjusts size
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
    private void AttemptToPlaceInBlueprint()
    {
        Vector2Int gridPos = BlueprintManager.instance.GetTileGridPosition(Input.mousePosition);
        if (BlueprintManager.instance.IsCellUseable(gridPos) && BoundaryCheck(gridPos.x, gridPos.y, component.width, component.height))
        {
            if (!BlueprintManager.instance.CheckCellOccupancy(gridPos, component.width, component.height))
            {
                BlueprintManager.instance.PlaceComponent(this, gridPos.x, gridPos.y);
                isPickedUp = false;
                canvasGroup.blocksRaycasts = false;
            }
            else
            {
                Debug.Log("Component overlap! Try again");
                ReturnToStartPosition();
            }
        }
        else
        {
            Debug.Log("Component was out of bounds!");
            ReturnToStartPosition();
        }
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (isPickedUp)
            return;
        if (currentLocation == ComponentLocation.Inventory)
        {
            PickUpComponent();
        }
    }
    private void PickUpComponent()
    {
        // Picking up component from inventory
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
        // Creates blurred placeholder at initial position
        placeholderCopy = Instantiate(gameObject, startPosition, Quaternion.identity, originalParent);
        CanvasGroup copyCanvasGroup = placeholderCopy.GetComponent<CanvasGroup>();

        copyCanvasGroup.alpha = ghostAlpha;
        copyCanvasGroup.blocksRaycasts = false;

        UIComponentItem componentItem = placeholderCopy.GetComponent<UIComponentItem>();
    }
    public void ReturnToStartPosition()
    {
        // Returns component to initial position in inventory
        transform.SetParent(originalParent);
        rectTransform.position = startPosition;
        currentLocation = ComponentLocation.Inventory;

        isPickedUp = false;
        canvasGroup.blocksRaycasts = true;

        Destroy(placeholderCopy);
    }
    public ComponentData GetComponentData()
    {
        return component;
    }
    private enum ComponentLocation
    {
        Inventory,
        Blueprint,
        DragLayer
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
    bool PositionCheck(int posX, int posY)
    {
        if (posX < 0 || posY < 0)
            return false;

        if (posX > BlueprintManager.instance.blueprintInUse.gridWidth || posY > BlueprintManager.instance.blueprintInUse.gridHeight)
            return false;

        return true;
    }
}