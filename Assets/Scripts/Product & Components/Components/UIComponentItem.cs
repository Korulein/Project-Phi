using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//start here if you mess up
public class UIComponentItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("UI Element Setup")]
    private Image iconImage;
    private CanvasGroup canvasGroup;
    private RectTransform rectTransform;
    private Transform originalParent;
    private ComponentData component;

    [Header("Grid Tracking")]
    private bool isPlacedOnGrid = false;
    private int gridX = -1;
    private int gridY = -1;
    private BlueprintCellData.CellType cellType;

    public void InitializeComponent(ComponentData componentData)
    {
        component = componentData;
        iconImage.sprite = componentData.componentSprite;
    }
    public ComponentData GetComponentData()
    {
        return component;
    }
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        iconImage = GetComponent<Image>();
    }
    /*
    public void SetGridPosition(int x, int y, BlueprintData.CellType type, BlueprintData.CellSize size, int width, int height)
    {
        gridX = x;
        gridY = y;
        cellType = type;
        cellSize = size;
        isPlacedOnGrid = true;
    }
    */
    // code to drag and drop
    // subject to change and optimisation
    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent;
        transform.SetParent(transform.root);
        canvasGroup.blocksRaycasts = false;

        if (isPlacedOnGrid && gridX >= 0 && gridY >= 0)
        {
            //BlueprintManager.instance.RemoveComponent(gridX, gridY);
            isPlacedOnGrid = false;
            gridX = gridY = -1;
        }
    }
    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.position = Input.mousePosition;
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        if (BlueprintManager.instance.AttemptToPlaceComponent(this, Input.mousePosition))
        {
            canvasGroup.blocksRaycasts = true;
        }
        else
        {
            transform.SetParent(originalParent);
            canvasGroup.blocksRaycasts = true;
            rectTransform.localPosition = Vector3.zero;
        }
    }

}