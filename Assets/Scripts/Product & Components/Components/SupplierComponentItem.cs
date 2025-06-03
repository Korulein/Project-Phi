using UnityEngine;

public class SupplierComponentItem : MonoBehaviour
{
    [Header("Component Data")]
    [SerializeField] public ComponentData componentData;
    public void OrderComponent()
    {
        InventoryManager.instance.AddOrderedComponentToInventory(componentData);
        Debug.Log($"Ordered {componentData.componentName} successfully!");
    }
}
