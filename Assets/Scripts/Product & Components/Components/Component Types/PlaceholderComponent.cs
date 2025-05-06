using UnityEngine;

[CreateAssetMenu(fileName = "New Placeholder Component", menuName = "Blueprint/Component/Placeholder Component")]
public class PlaceholderComponent : ComponentData
{
    [Header("Placeholder Component Data")]
    public float requiredPower;
    public float operationalTemperature;
}