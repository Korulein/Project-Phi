using UnityEngine;

[CreateAssetMenu(fileName = "New Cooling Component", menuName = "Blueprint/Component/Cooling Component")]
public class CoolingComponent : ComponentData
{
    [Header("Cooling Component Data")]
    public float maxCoolingCapacity;
    public CoolantTypes coolantType;
    public float operationalTemperature;
}
