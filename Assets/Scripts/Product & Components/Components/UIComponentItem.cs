using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class UIComponentItem : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    // Based on User feedback, point and click dragging should be changed to regular dragging, where the player has to hold down left click

    [Header("UI Element Setup")]
    private ComponentData component;
    private Image iconImage;
    private CanvasGroup canvasGroup;
    private RectTransform rectTransform;
    private Transform originalParent;
    private Vector2 originalSizeDelta;
    public int originBlueprintID;

    [Header("UI Element Position & Rotation")]
    private Vector2 startPosition;
    private Vector2Int gridPos;
    private ComponentLocation currentLocation;
    public ComponentRotation currentRotation;
    public ComponentRotation originalRotation;

    [Header("Placeholder Setup")]
    [SerializeField] private float ghostAlpha = 0.5f;
    private GameObject placeholderCopy;

    [Header("Flags")]
    private bool isPickedUp = false;
    public bool isRotated = false;

    [Header("Tooltip setup")]
    private string componentName;
    private LTDescr delay;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        iconImage = GetComponent<Image>();
        originBlueprintID = BlueprintManager.instance.activeBlueprintID;
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

    #region Component Data
    public ComponentData GetComponentData()
    {
        return component;
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
    #endregion

    #region UI Component Manipulation
    private void PickUpComponent()
    {
        // Picking up component from inventory
        originalParent = transform.parent;
        startPosition = transform.position;
        transform.SetParent(DeskUIManager.instance.dragLayer);
        PlayMaterialSoundPickup();

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
        placeholderCopy.tag = "Placeholder";
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
                PlayMaterialSoundDrop();
                isPickedUp = false;
                canvasGroup.blocksRaycasts = false;
                InventoryManager.instance.RemoveComponentFromInventory(component);
            }
            else
            {
                blueprintInteract.StartCoroutine(blueprintInteract.SuppressClickForOneFrame());
                Debug.Log("Component overlap! Try again");
                ReturnToStartPosition();
                PlayMaterialSoundDrop();
            }
        }
        else
        {
            ReturnToStartPosition();
            PlayMaterialSoundDrop();
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
        bool wasOnBlueprint = (currentLocation == ComponentLocation.Blueprint);
        currentLocation = ComponentLocation.Inventory;

        isPickedUp = false;
        canvasGroup.blocksRaycasts = true;

        rectTransform.sizeDelta = originalSizeDelta;
        Destroy(placeholderCopy);

        if (wasOnBlueprint && BlueprintManager.instance.activeOrderScreenUI != null && BlueprintManager.instance.activeMission != null)
        {
            BlueprintManager.instance.activeOrderScreenUI.CheckRequirements();
        }
    }
    public void ReturnToInventory()
    {
        // Called from BlueprintInteract, returns component to inventory
        canvasGroup.blocksRaycasts = true;
        isPickedUp = false;

        RectTransform rt = GetComponent<RectTransform>();
        rt.SetParent(DeskUIManager.instance.inventoryContainer, false);

        InventoryManager.instance.AddComponentToInventory(component);

        ReturnToStartPosition();
    }
    #endregion

    #region Pointer events
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
    #endregion

    #region Position checks
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
    #endregion

    #region Enums
    public enum ComponentLocation
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
    #endregion

    #region SFX
    public void PlayMaterialSoundPickup()
    {
        AudioManager audioManager = AudioManager.instance;

        if (component.materialType == MaterialTypes.Steel || component.materialType == MaterialTypes.Copper
            || component.materialType == MaterialTypes.Aluminum || component.materialType == MaterialTypes.Brass)
        {
            audioManager.PlayAudioClip(audioManager.steelPickup, transform, 0.5f);
        }
        if (component.materialType == MaterialTypes.Plastic || component.materialType == MaterialTypes.Silicone
            || component.materialType == MaterialTypes.Rubber || component.materialType == MaterialTypes.Lithium)
        {
            audioManager.PlayAudioClip(audioManager.plasticPickup, transform, 1f);
        }

        if (component.materialType == MaterialTypes.Aerogel || component.materialType == MaterialTypes.Self_Healing_Polymer)
        {
            audioManager.PlayAudioClip(audioManager.aerogelPickup, transform, 1f);
        }
        if (component.materialType == MaterialTypes.CarbonFiber)
        {
            audioManager.PlayAudioClip(audioManager.carbonFiberPickup, transform, 1f);
        }
        if (component.materialType == MaterialTypes.Lead || component.materialType == MaterialTypes.Lead_Titanium_Alloy
            || component.materialType == MaterialTypes.Nickel_Chromium)
        {
            audioManager.PlayAudioClip(audioManager.leadPickup, transform, 0.6f);
        }
        if (component.materialType == MaterialTypes.Ceramic)
        {
            audioManager.PlayAudioClip(audioManager.ceramicPickup, transform, 1f);

        }
    }
    public void PlayMaterialSoundDrop()
    {
        AudioManager audioManager = AudioManager.instance;

        if (component.materialType == MaterialTypes.Steel || component.materialType == MaterialTypes.Copper
            || component.materialType == MaterialTypes.Aluminum || component.materialType == MaterialTypes.Brass)
        {
            audioManager.PlayAudioClip(audioManager.steelDrop, transform, 1f);

        }
        if (component.materialType == MaterialTypes.Plastic || component.materialType == MaterialTypes.Silicone
            || component.materialType == MaterialTypes.Rubber || component.materialType == MaterialTypes.Lithium)
        {
            audioManager.PlayAudioClip(audioManager.plasticDrop, transform, 1f);

        }

        if (component.materialType == MaterialTypes.Aerogel || component.materialType == MaterialTypes.Self_Healing_Polymer)
        {
            audioManager.PlayAudioClip(audioManager.aerogelDrop, transform, 1f);

        }
        if (component.materialType == MaterialTypes.CarbonFiber)
        {
            audioManager.PlayAudioClip(audioManager.carbonFiberDrop, transform, 1f);

        }
        if (component.materialType == MaterialTypes.Lead || component.materialType == MaterialTypes.Lead_Titanium_Alloy
            || component.materialType == MaterialTypes.Nickel_Chromium)
        {
            audioManager.PlayAudioClip(audioManager.leadDrop, transform, 1f);

        }
        if (component.materialType == MaterialTypes.Ceramic)
        {
            audioManager.PlayAudioClip(audioManager.ceramicDrop, transform, 1f);

        }
    }
    #endregion
}