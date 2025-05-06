using UnityEngine;

[CreateAssetMenu(fileName = "New Heating Component", menuName = "Blueprint/Component/Heating Component")]
public class HeatingComponent : ComponentData
{
    [Header("Heating Component Data")]
    public float requiredPower;
    public float operationalTemperature;
}

