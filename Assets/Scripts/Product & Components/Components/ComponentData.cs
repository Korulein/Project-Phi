using UnityEngine;
public class ComponentData : ScriptableObject
{
    [Header("Component Information")]
    public string componentName;
    public Sprite componentSprite;
    [TextArea] public string componentDescription;
    public int componentID;
    public GameObject prefab;

    [Header("Component Specifics")]
    public MaterialTypes materialType;
    public SlotType slotType;
    public string weight;
    public bool requiresPowerInput;

    [Header("RAMS Ratings")]
    public float reliabilityRating;
    public float availabilityRating;
    public float safetyRating;
    public float maintainabilityRating;
}
