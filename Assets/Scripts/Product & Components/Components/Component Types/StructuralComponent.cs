using UnityEngine;

[CreateAssetMenu(fileName = "New Structural Component", menuName = "Blueprint/Component/Structural Component")]
public class StructuralComponent : ComponentData
{
    [Header("Subtype")]
    public StructuralSubtype structuralSubtype;

    [Header("Modifiers")]
    public float reliabilityModifier;
    public float availabilityModifier;
    public float maintainabilityModifier;
    public float safetyModifier;
}
