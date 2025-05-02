using UnityEngine;

[CreateAssetMenu(fileName = "New Power Source Component", menuName = "Blueprint/Component/Power Source Component")]
public class PowerSourceComponent : ComponentData
{
    [Header("Power Source Component Data")]
    public EnergyTypes energyType;
    public float maxPowerOutput; // In Watts
}
