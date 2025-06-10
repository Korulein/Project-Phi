using UnityEngine;
[CreateAssetMenu(fileName = "New Placeholder Component", menuName = "Blueprint/Component/Coffee Machine Component")]
public class CoffeeMachineComponent : ComponentData
{
    [Header("Coffee Machine Component Data")]
    public float requiredPower;
    public bool hasModifiers;

    [Header("Coffee Machine Component Modifiers")]
    public float reliabilityModifier;
    public float availabilityModifier;
    public float maintainabilityModifier;
    public float safetyModifier;
}
