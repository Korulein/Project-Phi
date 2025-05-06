using UnityEngine;

[CreateAssetMenu(fileName = "New Placeholder Component", menuName = "Blueprint/Component/Placeholder Component")]
public class PlaceholderComponent : ComponentData
{
    [Header("Placeholder Component Data")]
    public float requiredPower;
    public MaterialTypes materialType;
    public float operationalTemperature;
    public float weigt;
    public float width;
    public float height;
    
}