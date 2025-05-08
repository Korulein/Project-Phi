using UnityEngine;

[CreateAssetMenu(fileName = "New Structural Component", menuName = "Blueprint/Component/Structural Component")]
public class StructuralComponent : ComponentData
{
    public float reliabilityModifier;
    public float avaliblityModifier;
    public float maintainabilityModifier;
    public float safteyModifier;
}
