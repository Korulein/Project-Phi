using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Heating Component", menuName = "Blueprint/Component/Heating Component")]
public class HeatingComponent : ComponentData
{
    [Header("Heating Component Data")]
    public float requiredPower;
    public float operationalTemperature;
    public float producedHeat;

    [Header("Adjacency Properties")]
    public List<ComponentType> beneficialNeighbors = new List<ComponentType>();
    public List<ComponentType> harmfulNeighbors = new List<ComponentType>();

}

