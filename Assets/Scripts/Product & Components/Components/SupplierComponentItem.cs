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
            AudioManager.instance.PlayAudioClip(AudioManager.instance.orderComponent, transform, 0.3f);
            Debug.Log($"Ordered {componentData.componentName} successfully!");
        }
        else
        {
            AudioManager.instance.PlayAudioClip(AudioManager.instance.errorSound, transform, 0.3f);
            Debug.Log("Component has already been ordered!");
        }
    }
}
