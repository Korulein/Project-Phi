using UnityEngine;

[CreateAssetMenu(fileName = "Electronic Component", menuName = "Blueprint/Component/Electronic Component")]
public class ElectronicComponent : ComponentData
{
    [Header("Electronic Component Data")]
    public float operationalTemperature;
    public int operationalCPUSlots;
    public float requiredPower;
    public float producedHeat;
}

