using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using static MissionManager;

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
    private BlueprintCellData[,] grid;
    public Vector2Int lastPickUpOrigin;

    public Missions activeMission;
    public OrderScreenUI activeOrderScreenUI;
    public bool isMissionActive = false;

    public BlueprintData activeBlueprintData;
    public int activeBlueprintID;
    public static bool HasActiveMission()
    {
        return BlueprintManager.instance.activeMission != null &&
               !string.IsNullOrEmpty(BlueprintManager.instance.activeMission.missionTitle);
    }
    public static void ClearActiveMission()
    {
        BlueprintManager.instance.activeMission = null;
    }

    [Header("Debug")]
    public bool callMethod = false;
    [SerializeField] public int blueprintID;
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
            return;
        }

        for (int i = 0; i < blueprints.Count; i++)
        {
            blueprints[i].blueprintID = i;
        }

        LoadBlueprint(0);
    }

    public void ActivateBlueprint(int blueprintID)
    {
        if (blueprintID >= 0 && blueprintID <= blueprints.Count)
        {
            LoadBlueprint(blueprintID);
        }
        else
        {
            Debug.LogWarning("Invalid blueprintID: " + blueprintID);
        }
    }

    private void Start()
    {
        //Loads a blueprint, will be changed in development
        //LoadBlueprint(blueprintID);
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

    private void LoadBlueprint(int blueprintID)
    {
        if (isMissionActive == false)
        {
            DeskUIManager.instance.UpdateEmailButtonVisual();
        }
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
    private BlueprintData GetBlueprintByID(int id)
    {
        return blueprints.FirstOrDefault(blueprint => blueprint.blueprintID == id);
    }

    private Vector2Int FindComponentOrigin(UIComponentItem component, int width, int height)
    {
        for (int x = 0; x <= blueprintInUse.gridWidth - width; x++)
        {
            for (int y = 0; y <= blueprintInUse.gridHeight - height; y++)
            {
                bool matches = true;

                for (int i = 0; i < width && matches; i++)
                {
                    for (int j = 0; j < height && matches; j++)
                    {
                        if (grid[x + i, y + j].occupiedBy != component)
                        {
                            matches = false;
                        }
                    }
                }

                if (matches)
                    return new Vector2Int(x, y);
            }
        }

        return Vector2Int.one * -1; // not found
    }
    private Vector2 GetCellCenterPosition(int posX, int posY, ComponentData component)
    {
        float centerX = offsetX + (posX * CELL_PIXEL_SIZE + CELL_PIXEL_SIZE * component.playTimeWidth / 2);
        float centerY = -(offsetY + (posY * CELL_PIXEL_SIZE + CELL_PIXEL_SIZE * component.playTimeHeight / 2));

        return new Vector2(centerX, centerY);
    }

    public int CountComponentsWithTag(string componentTag)
    {
        int count = 0;

        // Iterate over all placed components
        var (components, _) = GetAllPlacedComponents();

        foreach (var component in components.Keys)
        {
            if (component.categoryName == componentTag)
            {
                count++;
            }
        }

        return count;
    }

    public bool CheckNumericRequirement(string componentTag, int requiredValue)
    {
        int currentValue = 0;

        var (components, _) = GetAllPlacedComponents();

        foreach (var component in components.Keys)
        {
            if (component.categoryName == componentTag)
            {
                currentValue += components[component];
            }
        }

        return currentValue >= requiredValue;
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

        Vector2Int tileGridPosition = new Vector2Int();
        tileGridPosition.x = Mathf.FloorToInt(localPoint.x / CELL_PIXEL_SIZE);
        tileGridPosition.y = Mathf.FloorToInt(-localPoint.y / CELL_PIXEL_SIZE);

        return tileGridPosition;
    }
    public BlueprintCellData[,] GetCurrentGrid()
    {
        return grid;
    }
    public void PlaceComponent(UIComponentItem componentItem, int posX, int posY)
    {
        if (componentItem.originBlueprintID != activeBlueprintID)
        {
            return;
        }

        if (activeMission.missionTitle != null)
        {
            ComponentData component = componentItem.GetComponentData();
            RectTransform componentTransform = componentItem.GetComponent<RectTransform>();
            componentTransform.SetParent(DeskUIManager.instance.blueprintGridContainer, false);

            // Grid update
            for (int i = 0; i < component.playTimeWidth; i++)
            {
                for (int j = 0; j < component.playTimeHeight; j++)
                {
                    grid[posX + i, posY + j].occupiedBy = componentItem;
                    grid[posX + i, posY + j].isOccupied = true;
                }
            }

            Vector2 cellCenter = GetCellCenterPosition(posX, posY, component);
            componentTransform.anchoredPosition = cellCenter;

            string compName = component.categoryName;
            activeOrderScreenUI.NotifyComponentPlaced(compName);

            float totalHeat = GetTotalProducedHeat();

            foreach (var reqUI in activeOrderScreenUI.requirementUIs)
            {
                if (!reqUI.HasData()) continue;

                bool isMet = reqUI.Evaluate(totalHeat);
                reqUI.SetColor(isMet ? Color.green : Color.red);
            }
        }
    }
    public float GetTotalProducedHeat()
    {
        float totalHeat = 0f;
        var (components, _) = GetAllPlacedComponents();

        foreach (var component in components.Keys)
        {
            HeatingComponent heatingComp = component as HeatingComponent;
            if (heatingComp != null)
            {
                totalHeat += heatingComp.producedHeat;
            }
        }
        return totalHeat;
    }

    public UIComponentItem PickUpComponent(int posX, int posY)
    {
        UIComponentItem componentToReturn = grid[posX, posY].occupiedBy;
        ComponentData component = componentToReturn.GetComponentData();

        Vector2Int origin = FindComponentOrigin(componentToReturn, component.playTimeWidth, component.playTimeHeight);
        //Safety check
        if (origin == Vector2Int.one * -1)
        {
            Debug.Log("Component origin not found");
            return null;
        }

        // Frees up grid cell values
        for (int i = origin.x; i < origin.x + component.playTimeWidth; i++)
        {
            for (int j = origin.y; j < origin.y + component.playTimeHeight; j++)
            {
                grid[i, j].occupiedBy = null;
                grid[i, j].isOccupied = false;
            }
        }
        lastPickUpOrigin = origin;

        if (activeOrderScreenUI != null && activeMission != null)
        {
            activeOrderScreenUI.CheckRequirements();
        }
        else
        {
            Debug.LogWarning("No active mission or OrderScreenUI set. Requirements not updated.");
        }

        return componentToReturn;
    }
    public void ClearBlueprint()
    {
        HashSet<UIComponentItem> alreadyCleared = new HashSet<UIComponentItem>();
        for (int i = 0; i < blueprintInUse.gridWidth; i++)
        {
            for (int j = 0; j < blueprintInUse.gridHeight; j++)
            {
                UIComponentItem componentItem = grid[i, j].occupiedBy;
                if (componentItem != null && !alreadyCleared.Contains(componentItem))
                {
                    // Return to inventory only once
                    componentItem.ReturnToInventory();
                    alreadyCleared.Add(componentItem);
                }
                grid[i, j].occupiedBy = null;
                grid[i, j].isOccupied = false;
            }
        }
    }
    public (Dictionary<ComponentData, int>, int) GetAllPlacedComponents()
    {
        // Returns all the components that are placed in the grid
        int numberOfCellsOccupied = 0;
        Dictionary<ComponentData, int> components = new Dictionary<ComponentData, int>();
        HashSet<UIComponentItem> seenComponents = new HashSet<UIComponentItem>();

        for (int x = 0; x < blueprintInUse.gridWidth; x++)
        {
            for (int y = 0; y < blueprintInUse.gridHeight; y++)
            {
                UIComponentItem componentItem = grid[x, y].occupiedBy;
                if (grid[x, y].isOccupied && componentItem != null && !seenComponents.Contains(componentItem))
                {
                    seenComponents.Add(componentItem);
                    ComponentData component = componentItem.GetComponentData();

                    int width = component.playTimeWidth;  // or component.width if you don't use runtime rotation
                    int height = component.playTimeHeight;

                    int cellCount = width * height;
                    numberOfCellsOccupied += cellCount;

                    components.Add(component, cellCount);
                }
            }
        }
        return (components, numberOfCellsOccupied);
    }
    public bool IsCellUseable(Vector2Int cell)
    {
        // Checks if a cell is within grid bounds and is useable
        return cell.x >= 0 && cell.y >= 0 &&
               cell.x < blueprintInUse.gridWidth &&
               cell.y < blueprintInUse.gridHeight &&
               grid[cell.x, cell.y].isUseable;
    }
    public bool CheckCellBoundary(int posX, int posY, int x, int y)
    {
        // Checks the blueprint boundaries in relation to component placement
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
        // Checks if the cells where the component would be placed are occupied.
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
    public bool HelperCheckCell(int posX, int posY)
    {
        // Only called once when dragging from inventory to blueprint
        return grid[posX, posY].occupiedBy;
    }
    public void ShowOccupiedCells()
    {
        // Debug method to show all occupied cells
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
    } // Debug method
}
