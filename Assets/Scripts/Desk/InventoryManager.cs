using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance { get; private set; }

    [Header("Component Lists")]
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
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {

    }
    public void AddOrderedComponentToInventory(ComponentData component)
    {
        ComponentType componentType = component.componentType;
        switch (componentType)
        {
            case ComponentType.Structural:
                if (CheckIfComponentIsAlreadyInInventory(component))
                {
                    structuralComponents.Add(component);
                }
                else
                {
                    Debug.Log("Component has already been ordered!");
                }
                break;
            case ComponentType.Sealant:
                if (CheckIfComponentIsAlreadyInInventory(component))
                {
                    sealantComponents.Add(component);
                }
                else
                {
                    Debug.Log("Component has already been ordered!");
                }
                break;
            case ComponentType.Heating:
                if (CheckIfComponentIsAlreadyInInventory(component))
                {
                    heatingComponents.Add(component);
                }
                else
                {
                    Debug.Log("Component has already been ordered!");
                }
                break;
            case ComponentType.Cooling:
                if (CheckIfComponentIsAlreadyInInventory(component))
                {
                    coolingComponents.Add(component);
                }
                else
                {
                    Debug.Log("Component has already been ordered!");
                }
                break;
            case ComponentType.Power:
                if (CheckIfComponentIsAlreadyInInventory(component))
                {
                    powerSourceComponents.Add(component);
                }
                else
                {
                    Debug.Log("Component has already been ordered!");
                }
                break;
            case ComponentType.Sensor:
                if (CheckIfComponentIsAlreadyInInventory(component))
                {
                    electronicComponents.Add(component);
                }
                else
                {
                    Debug.Log("Component has already been ordered!");
                }
                break;
            case ComponentType.Chip:
                if (CheckIfComponentIsAlreadyInInventory(component))
                {
                    electronicComponents.Add(component);
                }
                else
                {
                    Debug.Log("Component has already been ordered!");
                }
                break;
            case ComponentType.Special:
                if (CheckIfComponentIsAlreadyInInventory(component))
                {
                    specialComponents.Add(component);
                }
                else
                {
                    Debug.Log("Component has already been ordered!");
                }
                break;
            case ComponentType.Filter:
                if (CheckIfComponentIsAlreadyInInventory(component))
                {
                    filterComponents.Add(component);
                }
                else
                {
                    Debug.Log("Component has already been ordered!");
                }
                break;
            default:
                Debug.Log("Component type not recognized");
                break;
        }
    }
    public bool CheckIfComponentIsAlreadyInInventory(ComponentData componentToCheck)
    {
        ComponentType componentType = componentToCheck.componentType;
        switch (componentType)
        {
            case ComponentType.Structural:
                foreach (var component in structuralComponents)
                {
                    if (component.componentID == componentToCheck.componentID)
                        return false;
                }
                break;
            case ComponentType.Sealant:
                foreach (var component in sealantComponents)
                {
                    if (component.componentID == componentToCheck.componentID)
                        return false;
                }
                break;
            case ComponentType.Heating:
                foreach (var component in heatingComponents)
                {
                    if (component.componentID == componentToCheck.componentID)
                        return false;
                }
                break;
            case ComponentType.Cooling:
                foreach (var component in coolingComponents)
                {
                    if (component.componentID == componentToCheck.componentID)
                        return false;
                }
                break;
            case ComponentType.Power:
                foreach (var component in powerSourceComponents)
                {
                    if (component.componentID == componentToCheck.componentID)
                        return false;
                }
                break;
            case ComponentType.Sensor:
                foreach (var component in electronicComponents)
                {
                    if (component.componentID == componentToCheck.componentID)
                        return false;
                }
                break;
            case ComponentType.Chip:
                foreach (var component in electronicComponents)
                {
                    if (component.componentID == componentToCheck.componentID)
                        return false;
                }
                break;
            case ComponentType.Special:
                foreach (var component in specialComponents)
                {
                    if (component.componentID == componentToCheck.componentID)
                        return false;
                }
                break;
            case ComponentType.Filter:
                foreach (var component in filterComponents)
                {
                    if (component.componentID == componentToCheck.componentID)
                        return false;
                }
                break;
            default:
                Debug.Log("Component type not recognized");
                break;
        }
        return true;
    }
    public List<ComponentData> GetComponentListType(ComponentData component)
    {
        ComponentType componentType = component.componentType;
        switch (componentType)
        {
            case ComponentType.Structural:
                return structuralComponents;
            case ComponentType.Sealant:
                return sealantComponents;
            case ComponentType.Heating:
                return heatingComponents;
            case ComponentType.Cooling:
                return coolingComponents;
            case ComponentType.Power:
                return powerSourceComponents;
            case ComponentType.Sensor:
                return electronicComponents;
            case ComponentType.Chip:
                return electronicComponents;
            case ComponentType.Special:
                return specialComponents;
            case ComponentType.Filter:
                return filterComponents;
            default:
                Debug.Log("Component type not recognized");
                break;
        }
        return null;
    }
    public void AddComponentToInventoryList(ComponentData component)
    {

    }
    public void RemoveComponentFromInventoryList(ComponentData component)
    {

    }
}
