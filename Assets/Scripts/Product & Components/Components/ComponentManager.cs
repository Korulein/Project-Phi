using System.Collections.Generic;
using UnityEngine;

public class ComponentManager : MonoBehaviour
{

    [Header("Components")]
    [SerializeField] public List<ComponentData> structuralComponents = new List<ComponentData>();
    [SerializeField] public List<ComponentData> powerSourceComponents = new List<ComponentData>();
    [SerializeField] public List<ComponentData> electronicComponents = new List<ComponentData>();
    [SerializeField] public List<ComponentData> coolingComponents = new List<ComponentData>();
    [SerializeField] public List<ComponentData> heatingComponents = new List<ComponentData>();
    [SerializeField] public List<ComponentData> sealantComponents = new List<ComponentData>();
    [SerializeField] public List<ComponentData> filterComponents = new List<ComponentData>();
    [SerializeField] public List<ComponentData> specialComponents = new List<ComponentData>();

    private void Awake()
    {

    }
    // returns component by ID
    // SHOULD INCLUDE:
    // Methods for CompatibilityChecks
    // 
}
