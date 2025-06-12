using UnityEngine;
public class ComponentData : ScriptableObject
{
    [Header("Component Information")]
    public string componentName;
    public string categoryName;
    public Sprite componentSprite;
    public Sprite companyLogo;
    [TextArea] public string componentDescription;
    public int componentID;
    public GameObject inventoryPrefab;
    public GameObject blueprintPrefab;
    public ComponentType componentType;

    [Header("Component Specifics")]
    public MaterialTypes materialType;
    public Ratings componentRating;
    public SlotSize slotSize;
    public int width;
    public int height;
    [HideInInspector] public int playTimeWidth;
    [HideInInspector] public int playTimeHeight;
    public string weight;
    public bool requiresPowerInput;

    [Header("Adjacency")]
    public int adjacencyRange;
    public HeatTolerance heatTolerance;
    public HeatResistance heatResistance;

    [Header("RAMS Ratings")]
    public float reliabilityRating;
    public float availabilityRating;
    public float safetyRating;
    public float maintainabilityRating;
}
