using System.ComponentModel;
using UnityEngine;

public class RequirementUIManager : MonoBehaviour
{
    public OrderScreenUI activeOrderScreenUI;
    public void RefreshRequirementColors(ComponentData componentData)
    {
        string categoryName = componentData.categoryName;
        activeOrderScreenUI.NotifyComponentPurchased(categoryName);
    }
}