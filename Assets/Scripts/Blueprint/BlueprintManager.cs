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

    public GameObject blueprintPrefab;

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
        LoadBlueprint(0);
    }
    private void Update()
    {
    }
    public void LoadBlueprint(int blueprintID)
    {
        //clears grid when loading
        foreach (Transform child in DeskUIManager.instance.blueprintGridContainer)
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
        Image backgroundImage = DeskUIManager.instance.blueprintGridContainer.GetComponent<Image>();
        backgroundImage.sprite = currentBlueprint.blueprintImage;

        //offset calculation for centering
        float totalWidth = currentBlueprint.gridWidth * CELL_PIXEL_SIZE;
        float totalHeight = currentBlueprint.gridHeight * CELL_PIXEL_SIZE;

        offsetX = (DeskUIManager.instance.blueprintGridContainer.rect.width - totalWidth) / 2f;
        offsetY = (DeskUIManager.instance.blueprintGridContainer.rect.height - totalHeight) / 2f;

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
    public Vector2Int GetTileGridPosition(Vector2 mousePosition)
    {
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            DeskUIManager.instance.blueprintGridContainer,
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

        Vector2 cellCenter = GetCellCenterPosition(posX, posY, component);
        componentTransform.anchoredPosition = cellCenter;
    }
    public UIComponentItem PickUpComponent(int posX, int posY)
    {
        if (grid[posX, posY].occupiedBy == false)
            return null;

        UIComponentItem componentToReturn = grid[posX, posY].occupiedBy;
        ComponentData component = componentToReturn.GetComponentData();

        for (int i = 0; i < component.width; i++)
        {
            for (int j = 0; j < component.height; j++)
            {
                grid[onGridPositionX + i, onGridPositionY + j].occupiedBy = null;
                grid[onGridPositionX + i, onGridPositionY + j].isOccupied = false;
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
}
