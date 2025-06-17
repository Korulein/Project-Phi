using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Cooling Component", menuName = "Blueprint/Component/Cooling Component")]
public class CoolingComponent : ComponentData
{
    [Header("Cooling Component Data")]
    public float requiredPower;
    public CoolantTypes coolantType;
    public float operationalTemperature;
    public float reducedHeat;

    [Header("Adjacency Properties")]
    public List<ComponentType> beneficialNeighbors = new List<ComponentType>();
    public List<ComponentType> harmfulNeighbors = new List<ComponentType>();
}
