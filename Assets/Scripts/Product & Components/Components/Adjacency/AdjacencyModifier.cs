using UnityEngine;
[CreateAssetMenu(fileName = "Adjacency Modifier", menuName = "Adjacency")]
public class AdjacencyModifier : ScriptableObject
{
    [Header("Modifier Info")]
    public string modifierName;
    public string modifierDescription;

    [Header("Modifiers")]
    public float reliabilityModifier;
    public float availabilityModifier;
    public float maintainabilityModifier;
    public float safetyModifier;
}
