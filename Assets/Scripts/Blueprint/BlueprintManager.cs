using UnityEngine;
using static BlueprintData;

public class BlueprintManager : MonoBehaviour
{
    public static BlueprintManager instance { get; private set; }

    [Header("Blueprint Setup")]
    //[SerializeField] private Image blueprintImage;
    [SerializeField] private RectTransform gridContainer;
    [SerializeField] private BlueprintData currentBlueprint; // change later for multuple blueprints

    private ComponentData[,] grid;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        LoadBlueprint(currentBlueprint);
    }
    public void LoadBlueprint(BlueprintData blueprintData)
    {
        currentBlueprint = blueprintData;
        grid = new ComponentData[blueprintData.gridWidth, blueprintData.gridHeight];
        //blueprintImage.sprite = blueprintData.blueprintImage;

        foreach (Transform child in gridContainer)
        {
            if (child.GetComponent<UIComponentItem>() != null)
            {
                Destroy(child.gameObject);
            }
        }
    }
    public bool AttemptToPlaceComponent(UIComponentItem componentItem, Vector3 mousePosition)
    {
        if (currentBlueprint == null)
            return false;
        ComponentData component = componentItem.GetComponentData();

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            gridContainer, mousePosition, null, out Vector2 localPoint);


        float normalizedX = (localPoint.x / gridContainer.rect.width) + 0.5f;
        float normalizedY = (localPoint.y / gridContainer.rect.height) + 0.5f;

        int gridX = Mathf.FloorToInt(normalizedX * currentBlueprint.gridWidth);
        int gridY = Mathf.FloorToInt(normalizedY * currentBlueprint.gridHeight);
        if (currentBlueprint.TryToGetCell(gridX, gridY, out BlueprintData.Cell cell))
        {
            int originX = cell.x;
            int originY = cell.y;
            int width, height;
            if (cell.size == CellSize.Custom)
            {
                width = cell.width;
                height = cell.height;
            }
            else
            {
                width = (int)cell.size;
                height = (int)cell.size;
            }
            Debug.Log("Component slot size:" + (int)component.slotSize);
            Debug.Log("Cell Size" + (int)cell.size);
            if ((int)component.slotSize == (int)cell.size || ((int)component.slotSize == (int)BlueprintData.CellSize.Custom))
            {
                bool isEmpty = true;
                for (int x = originX; x < originX + width; x++)
                {
                    for (int y = originY; y < originY + height; y++)
                    {
                        if (x >= 0 && x < currentBlueprint.gridWidth && y >= 0 && y < currentBlueprint.gridHeight)
                        {
                            if (grid[x, y] != null)
                            {
                                isEmpty = false;
                                break;
                            }
                        }
                    }
                    if (!isEmpty)
                        break;
                }
                if (isEmpty)
                {
                    for (int x = originX; x < originX + width; x++)
                    {
                        for (int y = originY; y < originY + height; y++)
                        {
                            if (x >= 0 && x < currentBlueprint.gridWidth && y >= 0 && y < currentBlueprint.gridHeight)
                            {
                                grid[x, y] = componentItem.GetComponentData();
                            }
                        }
                    }
                }
                Vector2 cellCenter = GetCellCenter(originX, originY, width, height);
                componentItem.transform.SetParent(gridContainer);
                componentItem.GetComponent<RectTransform>().anchoredPosition = cellCenter;
                RectTransform rectTransform = componentItem.GetComponent<RectTransform>();
                float cellWidth = gridContainer.rect.width / currentBlueprint.gridWidth;
                float cellHeight = gridContainer.rect.height / currentBlueprint.gridHeight;
                rectTransform.sizeDelta = new Vector2(cellWidth * width, cellHeight * height);

                componentItem.SetGridPosition(originX, originY, cell.type, cell.size, width, height);

                return true;
            }
            else
            {
                Debug.Log($"Component size {component.slotSize} doesn't match cell size {cell.size}");
            }
        }
        return false;
    }
    private Vector2 GetCellCenter(int x, int y, int width = 1, int height = 1)
    {
        float cellWidth = gridContainer.rect.width / currentBlueprint.gridWidth;
        float cellHeight = gridContainer.rect.height / currentBlueprint.gridHeight;

        return new Vector2(
            (x + width + 0.5f) * cellWidth - gridContainer.rect.width * 0.5f,
            (y + height + 0.5f) * cellHeight - gridContainer.rect.height * 0.5f
        );
    }
    public void RemoveComponent(int x, int y)
    {
        if (currentBlueprint != null &&
            x >= 0 && x < currentBlueprint.gridWidth &&
            y >= 0 && y < currentBlueprint.gridHeight)
        {
            grid[x, y] = null;
        }
    }
}
