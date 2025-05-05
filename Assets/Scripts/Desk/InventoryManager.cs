using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [Header("Inventory Setup")]
    [SerializeField] private Transform inventoryContainer; // horizontal layout group
    private void Start()
    {
        PopulateInventory();
    }
    private void Update()
    {
    }
    private void PopulateInventory()
    {
        // destroys previous objects when loading
        foreach (Transform child in inventoryContainer)
        {
            Destroy(child.gameObject);
        }
        // instantiates prefab with associated data
        foreach (var componentData in ComponentManager.instance.components)
        {
            GameObject component = Instantiate(componentData.inventoryPrefab, inventoryContainer);
            UIComponentItem uiItem = component.GetComponent<UIComponentItem>();
            uiItem.InitializeComponent(componentData);
        }
    }
}
