using UnityEngine;

[CreateAssetMenu(fileName = "Computer Component", menuName = "Blueprint/Component/Computer Component")]
public class ComputerComponent : ComponentData
{
    [Header("Computer Component Data")]
    public float operationalTemperature;
    public float operationalCPUCapacity;
    public float requiredPower;
    public float producedHeat;
}
