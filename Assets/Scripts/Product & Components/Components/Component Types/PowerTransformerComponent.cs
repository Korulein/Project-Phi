using UnityEngine;
[CreateAssetMenu(fileName = "New Power Transformer Component", menuName = "Blueprint/Component/Power Transformer Component")]
public class PowerTransformerComponent : PowerSourceComponent
{
    [Header("Power Multiplier")]
    public float powerMultiplier;
}
