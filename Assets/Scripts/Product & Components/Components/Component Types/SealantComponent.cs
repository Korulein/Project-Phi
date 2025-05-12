using UnityEngine;

[CreateAssetMenu(fileName = "Sealant Component", menuName = "Blueprint/Component/Sealant Component")]
public class SealantComponent : ComponentData
{
    [Header("Sealant Component Data")]
    public float operationalTemperatureMin;
    public float operationalTemperatureMax;
}
