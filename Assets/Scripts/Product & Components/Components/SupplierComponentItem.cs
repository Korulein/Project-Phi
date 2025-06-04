using UnityEngine;

public class SupplierComponentItem : MonoBehaviour
{
    [Header("Component Data")]
    [SerializeField] public ComponentData componentData;
    public void OrderComponent()
    {
        if (InventoryManager.instance.CheckIfComponentIsAlreadyOrdered(componentData))
        {

            InventoryManager.instance.AddComponentToOrderedComponents(componentData);
            InventoryManager.instance.AddComponentToInventory(componentData);
            Debug.Log($"Ordered {componentData.componentName} successfully!");
        }
        else
        {
            Debug.Log("Component has already been ordered!");
        }
    }
}
