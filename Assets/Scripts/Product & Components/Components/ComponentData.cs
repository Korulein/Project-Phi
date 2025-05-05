using UnityEngine;
public class ComponentData : ScriptableObject
{
    [Header("Component Information")]
    public string componentName;
    public Sprite componentSprite;
    [TextArea] public string componentDescription;
    public int componentID;
    public GameObject inventoryPrefab;
    public GameObject blueprintPrefab;

    [Header("Component Specifics")]
    public MaterialTypes materialType;
    public SlotSize slotSize;
    public int width;
    public int height;
    public string weight;
    public bool requiresPowerInput;

    [Header("RAMS Ratings")]
    public float reliabilityRating;
    public float availabilityRating;
    public float safetyRating;
    public float maintainabilityRating;
}
