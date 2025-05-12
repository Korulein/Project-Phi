using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ComponentManager : MonoBehaviour
{
    public static ComponentManager instance { get; private set; }

    [Header("Components")]
    [SerializeField] public List<ComponentData> components = new List<ComponentData>();
    [SerializeField] public List<ComponentData> structuralComponents = new List<ComponentData>();
    [SerializeField] public List<ComponentData> powerSourceComponents = new List<ComponentData>();
    [SerializeField] public List<ComponentData> coolingComponents = new List<ComponentData>();
    [SerializeField] public List<ComponentData> heatingComponents = new List<ComponentData>();
    [SerializeField] public List<ComponentData> sensorComponents = new List<ComponentData>();
    [SerializeField] public List<ComponentData> sealantComponents = new List<ComponentData>();
    [SerializeField] public List<ComponentData> filterComponents = new List<ComponentData>();

    private void Awake()
    {
        // instance declaration
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        // generates component IDs
        for (int i = 0; i < components.Count; i++)
        {
            components[i].componentID = i;
        }
    }
    // returns component by ID
    public ComponentData GetComponentByID(int id)
    {
        return components.FirstOrDefault(component => component.componentID == id);
    }
    // SHOULD INCLUDE:
    // Methods for CompatibilityChecks
    // 
}
