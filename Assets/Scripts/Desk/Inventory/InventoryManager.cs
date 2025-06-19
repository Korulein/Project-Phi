using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance { get; private set; }

    [Header("Components in Inventory")]
    [SerializeField] public List<ComponentData> structuralComponentsInInventory = new List<ComponentData>();
    [SerializeField] public List<ComponentData> powerSourceComponentsInInventory = new List<ComponentData>();
    [SerializeField] public List<ComponentData> powerTransformerComponentsInInventory = new List<ComponentData>();
    [SerializeField] public List<ComponentData> electronicComponentsInInventory = new List<ComponentData>();
    [SerializeField] public List<ComponentData> coolingComponentsInInventory = new List<ComponentData>();
    [SerializeField] public List<ComponentData> heatingComponentsInInventory = new List<ComponentData>();
    [SerializeField] public List<ComponentData> sealantComponentsInInventory = new List<ComponentData>();
    [SerializeField] public List<ComponentData> filterComponentsInInventory = new List<ComponentData>();
    [SerializeField] public List<ComponentData> specialComponentsInInventory = new List<ComponentData>();

    [Header("Components Ordered")]
    [SerializeField] public List<ComponentData> structuralComponentsOrdered = new List<ComponentData>();
    [SerializeField] public List<ComponentData> powerSourceComponentsOrdered = new List<ComponentData>();
    [SerializeField] public List<ComponentData> powerTransformerComponentsOrdered = new List<ComponentData>();
    [SerializeField] public List<ComponentData> electronicComponentsOrdered = new List<ComponentData>();
    [SerializeField] public List<ComponentData> coolingComponentsOrdered = new List<ComponentData>();
    [SerializeField] public List<ComponentData> heatingComponentsOrdered = new List<ComponentData>();
    [SerializeField] public List<ComponentData> sealantComponentsOrdered = new List<ComponentData>();
    [SerializeField] public List<ComponentData> filterComponentsOrdered = new List<ComponentData>();
    [SerializeField] public List<ComponentData> specialComponentsOrdered = new List<ComponentData>();

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
    public void AddComponentToOrderedComponents(ComponentData component)
    {
        ComponentType componentType = component.componentType;
        switch (componentType)
        {
            case ComponentType.Structural:
                structuralComponentsOrdered.Add(component);
                break;
            case ComponentType.Sealant:
                sealantComponentsOrdered.Add(component);
                break;
            case ComponentType.Heating:
                heatingComponentsOrdered.Add(component);
                break;
            case ComponentType.Cooling:
                coolingComponentsOrdered.Add(component);
                break;
            case ComponentType.Power:
                powerSourceComponentsOrdered.Add(component);
                break;
            case ComponentType.Sensor:
                electronicComponentsOrdered.Add(component);
                break;
            case ComponentType.Chip:
                electronicComponentsOrdered.Add(component);
                break;
            case ComponentType.Special:
                specialComponentsOrdered.Add(component);
                break;
            case ComponentType.Filter:
                filterComponentsOrdered.Add(component);
                break;
            case ComponentType.PowerTransformer:
                powerTransformerComponentsOrdered.Add(component);
                break;
            default:
                Debug.Log("Component type not recognized");
                break;
        }
    }
    public void AddComponentToInventory(ComponentData component)
    {
        ComponentType componentType = component.componentType;
        switch (componentType)
        {
            case ComponentType.Structural:
                structuralComponentsInInventory.Add(component);
                break;
            case ComponentType.Sealant:
                sealantComponentsInInventory.Add(component);
                break;
            case ComponentType.Heating:
                heatingComponentsInInventory.Add(component);
                break;
            case ComponentType.Cooling:
                coolingComponentsInInventory.Add(component);
                break;
            case ComponentType.Power:
                powerSourceComponentsInInventory.Add(component);
                break;
            case ComponentType.Sensor:
                electronicComponentsInInventory.Add(component);
                break;
            case ComponentType.Chip:
                electronicComponentsInInventory.Add(component);
                break;
            case ComponentType.Special:
                specialComponentsInInventory.Add(component);
                break;
            case ComponentType.Filter:
                filterComponentsInInventory.Add(component);
                break;
            case ComponentType.PowerTransformer:
                powerSourceComponentsInInventory.Add(component);
                break;
            default:
                Debug.Log("Component type not recognized");
                break;
        }
    }
    public void RemoveComponentFromInventory(ComponentData component)
    {
        ComponentType componentType = component.componentType;
        switch (componentType)
        {
            case ComponentType.Structural:
                structuralComponentsInInventory.Remove(component);
                break;
            case ComponentType.Sealant:
                sealantComponentsInInventory.Remove(component);
                break;
            case ComponentType.Heating:
                heatingComponentsInInventory.Remove(component);
                break;
            case ComponentType.Cooling:
                coolingComponentsInInventory.Remove(component);
                break;
            case ComponentType.Power:
                powerSourceComponentsInInventory.Remove(component);
                break;
            case ComponentType.Sensor:
                electronicComponentsInInventory.Remove(component);
                break;
            case ComponentType.Chip:
                electronicComponentsInInventory.Remove(component);
                break;
            case ComponentType.Special:
                specialComponentsInInventory.Remove(component);
                break;
            case ComponentType.Filter:
                filterComponentsInInventory.Remove(component);
                break;
            case ComponentType.PowerTransformer:
                powerSourceComponentsInInventory.Remove(component);
                break;
            default:
                Debug.Log("Component type not recognized");
                break;
        }
    }
    public bool CheckIfComponentIsAlreadyOrdered(ComponentData componentToCheck)
    {
        ComponentType componentType = componentToCheck.componentType;
        switch (componentType)
        {
            case ComponentType.Structural:
                foreach (var component in structuralComponentsOrdered)
                {
                    if (component.componentID == componentToCheck.componentID)
                        return false;
                }
                break;
            case ComponentType.Sealant:
                foreach (var component in sealantComponentsOrdered)
                {
                    if (component.componentID == componentToCheck.componentID)
                        return false;
                }
                break;
            case ComponentType.Heating:
                foreach (var component in heatingComponentsOrdered)
                {
                    if (component.componentID == componentToCheck.componentID)
                        return false;
                }
                break;
            case ComponentType.Cooling:
                foreach (var component in coolingComponentsOrdered)
                {
                    if (component.componentID == componentToCheck.componentID)
                        return false;
                }
                break;
            case ComponentType.Power:
                foreach (var component in powerSourceComponentsOrdered)
                {
                    if (component.componentID == componentToCheck.componentID)
                        return false;
                }
                break;
            case ComponentType.PowerTransformer:
                foreach (var component in powerTransformerComponentsOrdered)
                {
                    if (component.componentID == componentToCheck.componentID)
                        return false;
                }
                break;
            case ComponentType.Sensor:
                foreach (var component in electronicComponentsOrdered)
                {
                    if (component.componentID == componentToCheck.componentID)
                    {
                        return false;
                    }
                }
                break;
            case ComponentType.Chip:
                foreach (var component in electronicComponentsOrdered)
                {
                    if (component.componentID == componentToCheck.componentID)
                        return false;
                }
                break;
            case ComponentType.Special:
                foreach (var component in specialComponentsOrdered)
                {
                    if (component.componentID == componentToCheck.componentID)
                        return false;
                }
                break;
            case ComponentType.Filter:
                foreach (var component in filterComponentsOrdered)
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
}
