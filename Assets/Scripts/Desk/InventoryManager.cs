using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [Header("Inventory Setup")]
    [SerializeField] private Transform structuralComponentContainer;
    [SerializeField] private Transform powerSourceComponentContainer;
    [SerializeField] private Transform coolingComponentContainer;
    [SerializeField] private Transform heatingComponentContainer;
    [SerializeField] private Transform sensorComponentContainer;
    [SerializeField] private Transform filterComponentContainer;
    [SerializeField] private Transform sealantComponentContainer;

    private void Start()
    {
        PopulateInventory();
    }
    private void PopulateInventory()
    {
        // destroys previous objects when loading
        RefreshInventory();

        // instantiates prefab with associated data
        foreach (var componentData in ComponentManager.instance.structuralComponents)
        {
            GameObject component = Instantiate(componentData.inventoryPrefab, structuralComponentContainer);
            UIComponentItem uiItem = component.GetComponent<UIComponentItem>();
            uiItem.InitializeComponent(componentData);
        }
        foreach (var componentData in ComponentManager.instance.powerSourceComponents)
        {
            GameObject component = Instantiate(componentData.inventoryPrefab, powerSourceComponentContainer);
            UIComponentItem uiItem = component.GetComponent<UIComponentItem>();
            uiItem.InitializeComponent(componentData);
        }
        foreach (var componentData in ComponentManager.instance.coolingComponents)
        {
            GameObject component = Instantiate(componentData.inventoryPrefab, coolingComponentContainer);
            UIComponentItem uiItem = component.GetComponent<UIComponentItem>();
            uiItem.InitializeComponent(componentData);
        }
        foreach (var componentData in ComponentManager.instance.heatingComponents)
        {
            GameObject component = Instantiate(componentData.inventoryPrefab, heatingComponentContainer);
            UIComponentItem uiItem = component.GetComponent<UIComponentItem>();
            uiItem.InitializeComponent(componentData);
        }
        foreach (var componentData in ComponentManager.instance.sensorComponents)
        {
            GameObject component = Instantiate(componentData.inventoryPrefab, sensorComponentContainer);
            UIComponentItem uiItem = component.GetComponent<UIComponentItem>();
            uiItem.InitializeComponent(componentData);
        }
        foreach (var componentData in ComponentManager.instance.filterComponents)
        {
            GameObject component = Instantiate(componentData.inventoryPrefab, filterComponentContainer);
            UIComponentItem uiItem = component.GetComponent<UIComponentItem>();
            uiItem.InitializeComponent(componentData);
        }
        foreach (var componentData in ComponentManager.instance.sealantComponents)
        {
            GameObject component = Instantiate(componentData.inventoryPrefab, sealantComponentContainer);
            UIComponentItem uiItem = component.GetComponent<UIComponentItem>();
            uiItem.InitializeComponent(componentData);
        }
    }
    private void RefreshInventory()
    {
        foreach (Transform child in structuralComponentContainer)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in powerSourceComponentContainer)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in coolingComponentContainer)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in heatingComponentContainer)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in sensorComponentContainer)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in filterComponentContainer)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in sealantComponentContainer)
        {
            Destroy(child.gameObject);
        }
    }
}
