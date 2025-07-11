using UnityEngine;

public class DisplayInventoryComponents : MonoBehaviour
{
    [Header("Inventory Setup")]
    [SerializeField] private Transform structuralComponentContainer;
    [SerializeField] private Transform powerSourceComponentContainer;
    [SerializeField] private Transform coolingComponentContainer;
    [SerializeField] private Transform heatingComponentContainer;
    [SerializeField] private Transform electronicComponentContainer;
    [SerializeField] private Transform filterComponentContainer;
    [SerializeField] private Transform sealantComponentContainer;
    [SerializeField] private Transform specialComponentContainer;
    public void PopulateInventory()
    {
        // destroys previous objects when loading
        RefreshInventory();

        // instantiates prefab with associated data
        foreach (var componentData in InventoryManager.instance.structuralComponentsInInventory)
        {
            GameObject component = Instantiate(componentData.componentPrefab, structuralComponentContainer);
            UIComponentItem uiItem = component.GetComponent<UIComponentItem>();
            uiItem.InitializeComponent(componentData);
        }
        foreach (var componentData in InventoryManager.instance.powerSourceComponentsInInventory)
        {
            GameObject component = Instantiate(componentData.componentPrefab, powerSourceComponentContainer);
            UIComponentItem uiItem = component.GetComponent<UIComponentItem>();
            uiItem.InitializeComponent(componentData);
        }
        foreach (var componentData in InventoryManager.instance.powerTransformerComponentsInInventory)
        {
            GameObject component = Instantiate(componentData.componentPrefab, powerSourceComponentContainer);
            UIComponentItem uiItem = component.GetComponent<UIComponentItem>();
            uiItem.InitializeComponent(componentData);
        }
        foreach (var componentData in InventoryManager.instance.coolingComponentsInInventory)
        {
            GameObject component = Instantiate(componentData.componentPrefab, coolingComponentContainer);
            UIComponentItem uiItem = component.GetComponent<UIComponentItem>();
            uiItem.InitializeComponent(componentData);
        }
        foreach (var componentData in InventoryManager.instance.heatingComponentsInInventory)
        {
            GameObject component = Instantiate(componentData.componentPrefab, heatingComponentContainer);
            UIComponentItem uiItem = component.GetComponent<UIComponentItem>();
            uiItem.InitializeComponent(componentData);
        }
        foreach (var componentData in InventoryManager.instance.electronicComponentsInInventory)
        {
            GameObject component = Instantiate(componentData.componentPrefab, electronicComponentContainer);
            UIComponentItem uiItem = component.GetComponent<UIComponentItem>();
            uiItem.InitializeComponent(componentData);
        }
        foreach (var componentData in InventoryManager.instance.filterComponentsInInventory)
        {
            GameObject component = Instantiate(componentData.componentPrefab, filterComponentContainer);
            UIComponentItem uiItem = component.GetComponent<UIComponentItem>();
            uiItem.InitializeComponent(componentData);
        }
        foreach (var componentData in InventoryManager.instance.sealantComponentsInInventory)
        {
            GameObject component = Instantiate(componentData.componentPrefab, sealantComponentContainer);
            UIComponentItem uiItem = component.GetComponent<UIComponentItem>();
            uiItem.InitializeComponent(componentData);
        }
        foreach (var componentData in InventoryManager.instance.specialComponentsInInventory)
        {
            GameObject component = Instantiate(componentData.componentPrefab, specialComponentContainer);
            UIComponentItem uiItem = component.GetComponent<UIComponentItem>();
            uiItem.InitializeComponent(componentData);
        }
    }
    private void RefreshInventory()
    {
        foreach (Transform child in structuralComponentContainer)
        {
            if (child.tag == "Placeholder") continue;
            Destroy(child.gameObject);
        }
        foreach (Transform child in powerSourceComponentContainer)
        {
            if (child.tag == "Placeholder") continue;
            Destroy(child.gameObject);
        }
        foreach (Transform child in coolingComponentContainer)
        {
            if (child.tag == "Placeholder") continue;
            Destroy(child.gameObject);
        }
        foreach (Transform child in heatingComponentContainer)
        {
            if (child.tag == "Placeholder") continue;
            Destroy(child.gameObject);
        }
        foreach (Transform child in electronicComponentContainer)
        {
            if (child.tag == "Placeholder") continue;
            Destroy(child.gameObject);
        }
        foreach (Transform child in filterComponentContainer)
        {
            if (child.tag == "Placeholder") continue;
            Destroy(child.gameObject);
        }
        foreach (Transform child in sealantComponentContainer)
        {
            if (child.tag == "Placeholder") continue;
            Destroy(child.gameObject);
        }
        foreach (Transform child in specialComponentContainer)
        {
            if (child.tag == "Placeholder") continue;
            Destroy(child.gameObject);
        }
    }
}
