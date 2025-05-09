using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class BlueprintManager : MonoBehaviour
{
    public static BlueprintManager instance { get; private set; }

    [Header("Blueprint Setup")]
    [SerializeField] public List<BlueprintData> blueprints;
    [HideInInspector] public BlueprintData blueprintInUse;
    [SerializeField] private GameObject standardCell;
    [SerializeField] private GameObject requiredCell;
    [SerializeField] private GameObject specialCell;
    [SerializeField] private GameObject emptyCell;
    private const int CELL_PIXEL_SIZE = 90; //in pixels
    private float offsetX;
    private float offsetY;
    private int onGridPositionX;
    private int onGridPositionY;
    private BlueprintCellData[,] grid;

    [Header("Debug")]
    public bool callMethod = false;

    [Header("Mouse Setup")]
    private Vector2Int tileGridPosition = new Vector2Int();
    private void Awake()
    {
        // Instance and ID setup
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
        //Loads a blueprint, will be changed in development
        LoadBlueprint(1);
    }
    private void Update()
    {
        // Debugging
        if (callMethod)
        {
            callMethod = false;
            ShowOccupiedCells();
        }
    }
    public void LoadBlueprint(int blueprintID)
    {
        // Clears grid when loading
        foreach (Transform child in DeskUIManager.instance.blueprintGridContainer)
        {
            if (child.GetComponent<UIComponentItem>() != null)
            {
                Destroy(child.gameObject);
            }
        }

        // Gets blueprint and initializes grid
        BlueprintData currentBlueprint = GetBlueprintByID(blueprintID);
        blueprintInUse = currentBlueprint;
        grid = new BlueprintCellData[currentBlueprint.gridWidth, currentBlueprint.gridHeight];

        // Loading background
        Image backgroundImage = DeskUIManager.instance.blueprintGridContainer.GetComponent<Image>();
        backgroundImage.sprite = currentBlueprint.blueprintImage;

        // Offset calculation for centering
        float totalWidth = currentBlueprint.gridWidth * CELL_PIXEL_SIZE;
        float totalHeight = currentBlueprint.gridHeight * CELL_PIXEL_SIZE;

        // Offset calculation
        offsetX = (DeskUIManager.instance.blueprintGridContainer.rect.width - totalWidth) / 2f;
        offsetY = (DeskUIManager.instance.blueprintGridContainer.rect.height - totalHeight) / 2f;

        // Instantiates grid cells
        int cellCounter = 0;
        for (int j = 0; j < currentBlueprint.gridHeight; j++)
        {
            for (int i = 0; i < currentBlueprint.gridWidth; i++)
            {
                if (cellCounter >= currentBlueprint.allCells.Count)
                    return;

                BlueprintCellData currentCell = currentBlueprint.allCells[cellCounter];
                grid[i, j] = currentCell;
                grid[i, j].isOccupied = false;

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

                newCell = Instantiate(prefabToInstantiate, DeskUIManager.instance.blueprintGridContainer);
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
    public BlueprintData GetBlueprintByID(int id)
    {
        return blueprints.FirstOrDefault(blueprint => blueprint.blueprintID == id);
    }
    public BlueprintCellData[,] GetBlueprintGrid()
    {
        return grid;
    }
    public int GetCellPixelSize()
    {
        return CELL_PIXEL_SIZE;
    }
    public Vector2Int GetTileGridPosition(Vector2 mousePosition)
    {
        // Converts mouse position to grid position
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            DeskUIManager.instance.blueprintGridContainer,
            mousePosition,
            null,
            out localPoint
        );
        // Adds offset
        localPoint.x -= offsetX;
        localPoint.y += offsetY;

        tileGridPosition.x = Mathf.FloorToInt(localPoint.x / CELL_PIXEL_SIZE);
        tileGridPosition.y = Mathf.FloorToInt(-localPoint.y / CELL_PIXEL_SIZE);

        return tileGridPosition;
    }
    public Vector2 GetCellCenterPosition(int posX, int posY, ComponentData component)
    {
        float centerX = offsetX + (posX * CELL_PIXEL_SIZE + CELL_PIXEL_SIZE * component.width / 2);
        float centerY = -(offsetY + (posY * CELL_PIXEL_SIZE + CELL_PIXEL_SIZE * component.height / 2));

        return new Vector2(centerX, centerY);
    }
    public void PlaceComponent(UIComponentItem componentItem, int posX, int posY)
    {
        // IMPORTANT
        // MAKE SURE PREFAB MIN, MAX ANCHORS ARE SET TO [0, 1] AND PIVOT to [0.5, 0.5];

        ComponentData component = componentItem.GetComponentData();
        RectTransform componentTransform = componentItem.GetComponent<RectTransform>();
        componentTransform.SetParent(DeskUIManager.instance.blueprintGridContainer, false);

        // Sets the grid cell values to be occupied
        for (int i = 0; i < component.width; i++)
        {
            for (int j = 0; j < component.height; j++)
            {
                grid[posX + i, posY + j].occupiedBy = componentItem;
                grid[posX + i, posY + j].isOccupied = true;
            }
        }
        onGridPositionX = posX;
        onGridPositionY = posY;

        // Fixes position to cell center
        Vector2 cellCenter = GetCellCenterPosition(posX, posY, component);
        componentTransform.anchoredPosition = cellCenter;
    }
    public UIComponentItem PickUpComponent(int posX, int posY)
    {
        if (!grid[posX, posY].occupiedBy)
            return null;

        UIComponentItem componentToReturn = grid[posX, posY].occupiedBy;
        ComponentData component = componentToReturn.GetComponentData();

        // Frees up grid cell values
        for (int i = posX; i < posX + component.width; i++)
        {
            for (int j = posY; j < posY + component.height; j++)
            {
                grid[i, j].occupiedBy = null;
                grid[i, j].isOccupied = false;
            }
        }
        return componentToReturn;
    }
    public bool IsCellUseable(Vector2Int cell)
    {
        return cell.x >= 0 && cell.y >= 0 &&
               cell.x < blueprintInUse.gridWidth &&
               cell.y < blueprintInUse.gridHeight &&
               grid[cell.x, cell.y].isUseable;
    }
    public bool CheckCellBoundary(int posX, int posY, int x, int y)
    {
        for (int j = posY; j < y; j++)
        {
            for (int i = posX; i < x; i++)
            {
                if (j >= blueprintInUse.gridHeight || i >= blueprintInUse.gridWidth)
                    break;
                if (!grid[i, j].isUseable)
                    return false;
            }
        }
        return true;
    }
    public bool CheckCellOccupancy(Vector2Int cell, int width, int height)
    {
        for (int x = cell.x; x < cell.x + width; x++)
        {
            for (int y = cell.y; y < cell.y + height; y++)
            {
                if (grid[x, y].isOccupied)
                {
                    return true;
                }
            }
        }
        return false;
    }
    public void ShowOccupiedCells()
    {
        // Debug method
        bool foundCells = false;
        for (int i = 0; i < blueprintInUse.gridWidth; i++)
        {
            for (int j = 0; j < blueprintInUse.gridHeight; j++)
            {
                if (grid[i, j].isOccupied && grid[i, j].isUseable)
                {
                    foundCells = true;
                    Debug.Log($"Found occupied cell at: [{i}], [{j}]");
                }
            }
        }
        if (!foundCells)
            Debug.Log("No occupied cells were found");
    }
}
