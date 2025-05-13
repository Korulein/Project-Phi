using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ProductManager : MonoBehaviour
{
    // Product manager will handle final calculations using the components in the blueprint.
    // It will calculate RAMS ratings and apply modifiers granted by component compatibility
    // or incompatibility. These aforementioned checks will be done in the component manager.
    public static ProductManager instance { get; private set; }
    [Header("References")]
    private List<ComponentData> componentsInBlueprint;

    [Header("Products")]
    [SerializeField] public List<ProductData> products = new List<ProductData>();
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
        // generate product IDs
        for (int i = 0; i < products.Count; i++)
        {
            products[i].productID = i;
        }
    }
    public void CheckProductAssembly()
    {
        // can be improved using hashsets, modify later for clarity
        componentsInBlueprint = BlueprintManager.instance.GetAllPlacedComponents();
        if (BlueprintManager.instance.blueprintInUse.blueprintID == 1)
        {
            ProductData coffeeMachine = products[0];
            int amountOfMandatoryRegularComponents = 0;
            int amountOfMandatorySpecialComponents = 0;
            foreach (var componentInBlueprint in componentsInBlueprint)
            {
                foreach (var componentToCheck in coffeeMachine.mandatoryRegularComponents)
                {
                    if (componentInBlueprint.componentType == componentToCheck.componentType)
                    {
                        amountOfMandatoryRegularComponents++;
                        break;
                    }
                }
            }
            if (amountOfMandatoryRegularComponents == coffeeMachine.mandatoryRegularComponents.Count() && coffeeMachine.mandatoryRegularComponents.Count() != 0)
                coffeeMachine.hasRegularComponents = true;
            foreach (var componentInBlueprint in componentsInBlueprint)
            {
                foreach (var componentToCheck in coffeeMachine.mandatorySpecialComponents)
                {
                    if (componentInBlueprint.componentType == componentToCheck.componentType)
                    {
                        amountOfMandatorySpecialComponents++;
                        break;
                    }
                }
            }
            if (amountOfMandatorySpecialComponents == coffeeMachine.mandatorySpecialComponents.Count())
                coffeeMachine.hasSpecialComponents = true;
            if (coffeeMachine.hasSpecialComponents && coffeeMachine.hasRegularComponents)
            {
                Debug.Log("Assembling product...");
                AssembleProduct();
                coffeeMachine.hasRegularComponents = false;
                coffeeMachine.hasSpecialComponents = false;
                DeskUIManager.instance.DisplayPopUp();
                BlueprintManager.instance.ClearBlueprint();
            }
            else
            {
                Debug.Log("Missing required components!");
                coffeeMachine.hasRegularComponents = false;
                coffeeMachine.hasSpecialComponents = false;
            }
        }
    }
    private void AssembleProduct()
    {
        return;
    }
    private void CalculateRAMS()
    {

    }
    public ProductData GetProductByID(int id)
    {
        return products.FirstOrDefault(product => product.productID == id);
    }
}
