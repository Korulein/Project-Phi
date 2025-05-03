using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class BlueprintManager : MonoBehaviour
{
    public static BlueprintManager instance { get; private set; }

    [Header("Blueprint Setup")]
    [SerializeField] private RectTransform gridContainer;
    [SerializeField] public List<BlueprintData> blueprints;
    [HideInInspector] public BlueprintData blueprintInUse;
    [SerializeField] private GameObject standardCell;
    [SerializeField] private GameObject requiredCell;
    [SerializeField] private GameObject specialCell;
    [SerializeField] private GameObject emptyCell;
    private const int CELL_PIXEL_SIZE = 90; //in pixels
    private float offsetX;
    private float offsetY;
    private BlueprintCellData[,] grid;

    [Header("Mouse Setup")]
    private Vector2Int tileGridPosition = new Vector2Int();
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
        for (int i = 0; i < blueprints.Count; i++)
        {
            blueprints[i].blueprintID = i;
        }
    }
    private void Start()
    {
        LoadBlueprint(1);
    }
    private void Update()
    {
        //Debug.Log(GetTileGridPosition(Input.mousePosition));
    }
    public BlueprintData GetBlueprintByID(int id)
    {
        return blueprints.FirstOrDefault(blueprint => blueprint.blueprintID == id);
    }
    public BlueprintCellData[,] GetBlueprintGrid()
    {
        return grid;
    }
    public void LoadBlueprint(int blueprintID)
    {
        //clears grid when loading
        foreach (Transform child in gridContainer)
        {
            if (child.GetComponent<UIComponentItem>() != null)
            {
                Destroy(child.gameObject);
            }
        }

        BlueprintData currentBlueprint = GetBlueprintByID(blueprintID);
        blueprintInUse = currentBlueprint;
        grid = new BlueprintCellData[currentBlueprint.gridWidth, currentBlueprint.gridHeight];

        //loading background
        Image backgroundImage = gridContainer.GetComponent<Image>();
        backgroundImage.sprite = currentBlueprint.blueprintImage;

        //offset calculation for centering
        float totalWidth = currentBlueprint.gridWidth * CELL_PIXEL_SIZE;
        float totalHeight = currentBlueprint.gridHeight * CELL_PIXEL_SIZE;

        offsetX = (gridContainer.rect.width - totalWidth) / 2f;
        offsetY = (gridContainer.rect.height - totalHeight) / 2f;

        //instantiates grid
        int cellCounter = 0;
        for (int j = 0; j < currentBlueprint.gridHeight; j++)
        {
            for (int i = 0; i < currentBlueprint.gridWidth; i++)
            {
                if (cellCounter >= currentBlueprint.allCells.Count)
                    return;

                BlueprintCellData currentCell = currentBlueprint.allCells[cellCounter];
                grid[i, j] = currentCell;

                //instantiating cells
                GameObject prefabToInstantiate;
                GameObject newCell;
                if (!currentCell.isUseable)
                {
                    prefabToInstantiate = emptyCell;
                }
                else
                {
                    prefabToInstantiate = currentCell.type == BlueprintCellData.CellType.Required ? requiredCell : standardCell;
                }

                newCell = Instantiate(prefabToInstantiate, gridContainer);
                newCell.name = $"Cell {j} {i}";

                //anchoring
                RectTransform cellRect = newCell.GetComponent<RectTransform>();
                cellRect.anchorMin = new Vector2(0, 1);
                cellRect.anchorMax = new Vector2(0, 1);
                cellRect.pivot = new Vector2(0, 1); // Top Left Pivot
                cellRect.sizeDelta = new Vector2(CELL_PIXEL_SIZE, CELL_PIXEL_SIZE);

                //positioning
                float posX = offsetX + (i * CELL_PIXEL_SIZE);
                float posY = -offsetY - (j * CELL_PIXEL_SIZE);
                cellRect.anchoredPosition = new Vector2(posX, posY);

                cellCounter++;
            }
        }

    }
    public Vector2Int GetTileGridPosition(Vector2 mousePosition)
    {
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            gridContainer,
            mousePosition,
            null,
            out localPoint
        );
        localPoint.x -= offsetX;
        localPoint.y += offsetY;

        tileGridPosition.x = Mathf.FloorToInt(localPoint.x / CELL_PIXEL_SIZE);
        tileGridPosition.y = Mathf.FloorToInt(-localPoint.y / CELL_PIXEL_SIZE);

        return tileGridPosition;
    }
    public bool AttemptToPlaceComponent(UIComponentItem componentItem, Vector3 mousePosition)
    {
        /*
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
        }*/
        return false;
    }

    /*RemoveComponent method
    public void RemoveComponent(int x, int y)
    {
        if (currentBlueprint != null &&
            x >= 0 && x < currentBlueprint.gridWidth &&
            y >= 0 && y < currentBlueprint.gridHeight)
        {
            grid[x, y] = null;
        }
    }
    */
}
