using UnityEngine;

[System.Serializable]
public class Requirement
{
    public string name;
    public RequirementType type;
    public float numericValue;
    public string componentTag;
    public int amount = 1;

    public enum RequirementType
    {
        Component,
        NumericValue,
        Custom
    }
}