using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
public class ProductManager : MonoBehaviour
{
    // Product manager will handle final calculations using the components in the blueprint.
    // It will calculate RAMS ratings and apply modifiers granted by component compatibility
    // or incompatibility. These aforementioned checks will be done in the component manager.
    public static ProductManager instance { get; private set; }
    [Header("References")]
    private Dictionary<ComponentData, int> componentsInBlueprint;
    private int numberOfCellsOccupied;

    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI ramsText;

    [Header("Products")]
    [SerializeField] public List<ProductData> products = new List<ProductData>();
    private void Awake()
    {
        // Singleton patroon
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (gameObject.name == "Ratings")
            {
                Debug.LogWarning("Cannot destroy 'Ratings' object. Skipping destruction.");
                return;
            }

            Destroy(gameObject);
        }

        // Genereer product IDs
        for (int i = 0; i < products.Count; i++)
        {
            products[i].productID = i;
        }

    }
    public void CheckProductAssembly()
    {
        // can be improved using hashsets, modify later for clarity and performance
        (componentsInBlueprint, numberOfCellsOccupied) = BlueprintManager.instance.GetAllPlacedComponents();
        if (BlueprintManager.instance.blueprintInUse.blueprintID == 1)
        {
            ProductData coffeeMachine = products[0];
            int amountOfMandatoryRegularComponents = 0;
            int amountOfMandatorySpecialComponents = 0;
            foreach (var componentInBlueprint in componentsInBlueprint)
            {
                ComponentData component = componentInBlueprint.Key;
                foreach (var componentToCheck in coffeeMachine.mandatoryRegularComponents)
                {
                    if (component.componentType == componentToCheck.componentType)
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
                    ComponentData component = componentInBlueprint.Key;
                    if (component.componentType == componentToCheck.componentType)
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
                AssembleProduct();
                AudioManager.instance.PlayAudioClip(AudioManager.instance.completeConstruction, transform, 0.9f);
                coffeeMachine.hasRegularComponents = false;
                coffeeMachine.hasSpecialComponents = false;
                DeskUIManager.instance.DisplayPopUp();
                BlueprintManager.instance.ClearBlueprint();

                BlueprintManager.ClearActiveMission();
                if (BlueprintManager.instance.activeOrderScreenUI != null)
                {
                    BlueprintManager.instance.activeOrderScreenUI.EndMission();
                }
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
        float reliabilityRating = 0, availabilityRating = 0, maintainabilityRating = 0, safetyRating = 0;
        KoruFormula(ref reliabilityRating, ref availabilityRating, ref maintainabilityRating, ref safetyRating);

        string ramsSummary = $"RAMS Ratings:\n" +
                             $"- Reliability: {reliabilityRating:F2}\n" +
                             $"- Availability: {availabilityRating:F2}\n" +
                             $"- Maintainability: {maintainabilityRating:F2}\n" +
                             $"- Safety: {safetyRating:F2}";

        if (ramsText != null)
        {
            ramsText.text = ramsSummary;
        }
        else
        {
            Debug.LogWarning("RAMS TextMeshPro is not assigned!");
        }
    }
    private (float, float, float, float) KoruFormula(ref float reliabilityRating, ref float availabilityRating, ref float maintainabilityRating, ref float safetyRating)
    {
        float reliabilityProduct = 1, availabilityProduct = 1, maintainabilityProduct = 1, safetyProduct = 1;
        SetModifiers(ref reliabilityProduct, ref availabilityProduct, ref maintainabilityProduct, ref safetyProduct);
        foreach (var componentInBlueprint in componentsInBlueprint)
        {
            ComponentData component = componentInBlueprint.Key;
            int cellsOccupied = componentInBlueprint.Value;
            reliabilityRating += component.reliabilityRating * cellsOccupied;
            availabilityRating += component.availabilityRating * cellsOccupied;
            maintainabilityRating += component.maintainabilityRating * cellsOccupied;
            safetyRating += component.safetyRating * cellsOccupied;
        }
        reliabilityRating = (reliabilityRating * reliabilityProduct) / numberOfCellsOccupied;
        availabilityRating = (availabilityRating * availabilityProduct) / numberOfCellsOccupied;
        maintainabilityRating = (maintainabilityRating * maintainabilityProduct) / numberOfCellsOccupied;
        safetyRating = (safetyRating * safetyProduct) / numberOfCellsOccupied;
        return (reliabilityRating, availabilityRating, maintainabilityRating, safetyRating);
    }
    private void SetModifiers(ref float reliabilityProduct, ref float availabilityProduct, ref float maintainabilityProduct, ref float safetyProduct)
    {
        foreach (var componentInBlueprint in componentsInBlueprint)
        {
            ComponentData component = componentInBlueprint.Key;
            if (component.componentType == ComponentType.Structural)
            {
                StructuralComponent structuralComponent = (StructuralComponent)component;
                reliabilityProduct *= structuralComponent.reliabilityModifier;
                availabilityProduct *= structuralComponent.availabilityModifier;
                maintainabilityProduct *= structuralComponent.maintainabilityModifier;
                safetyProduct *= structuralComponent.safetyModifier;
            }
        }
    }
    public ProductData GetProductByID(int id)
    {
        return products.FirstOrDefault(product => product.productID == id);
    }

    public void PlayButtonSFX() {

        AudioManager.instance.PlayAudioClip(AudioManager.instance.buttonPress1, transform,1f);

    }

}