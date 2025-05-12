using UnityEngine;

[CreateAssetMenu(fileName = "Eletronic Component", menuName = "Blueprint/Component/Eletronic Component")]
public class EletronicComponent : ComponentData
{
    [Header("Eletronic Component Data")]
    public float operationalTemperatureMin;
    public float operationalTemperatureMax;
    public float operationalCPUUsage;
    public float requiredPower;
}

