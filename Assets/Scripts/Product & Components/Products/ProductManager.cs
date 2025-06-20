using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Playables;

public class ProductManager : MonoBehaviour
{
    public static ProductManager instance { get; private set; }

    [Header("References")]
    private Dictionary<ComponentData, int> componentsInBlueprint;
    private int numberOfCellsOccupied;

    [Header("Animator Director")]
    public PlayableDirector director;

    [Header("Products")]
    [SerializeField] public List<ProductData> products = new List<ProductData>();

    [Header("Product Requirements")]
    [SerializeField] private float heatOutput = 0;
    [SerializeField] private float coolingOutput = 0;
    [SerializeField] private int operationalCPUSlots = 0;
    [SerializeField] private float powerWattage = 0;
    private void Awake()
    {
        // Singleton setup
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (gameObject.name == "Ratings")
            {
                return;
            }

            Destroy(gameObject);
        }

        // Generate product IDs
        for (int i = 0; i < products.Count; i++)
        {
            products[i].productID = i;
        }

    }
    private ProductData GetProductData(int blueprintID)
    {
        // Gets product data by blueprint ID
        switch (blueprintID)
        {
            case 1:
                return products[0];
        }
        return null;
    }
    public void CheckProductAssembly()
    {
        // Blueprint references
        if (BlueprintManager.instance.blueprintInUse.blueprintID == 0)
        {
            Debug.Log("No mission has been started!");
            return;
        }
        var blueprint = BlueprintManager.instance.blueprintInUse;
        (componentsInBlueprint, numberOfCellsOccupied) = BlueprintManager.instance.GetAllPlacedComponents();

        // Product references
        ProductData product = GetProductData(blueprint.blueprintID);
        int matchedRegularComponents = 0;
        int matchedSpecialComponents = 0;

        // HashSet declaration
        var regularSet = new HashSet<ComponentType>(product.mandatoryRegularComponents.Select(c => c.componentType));
        var specialSet = new HashSet<ComponentType>(product.mandatorySpecialComponents.Select(c => c.componentType));

        // Counts matched component types
        // Will also need to calculate power wattage, heat output and other factors in the future
        foreach (var component in componentsInBlueprint)
        {
            ComponentType componentType = component.Key.componentType;

            // Matching components
            if (regularSet.Contains(componentType))
            {
                matchedRegularComponents++;
            }
            else if (specialSet.Contains(componentType))
            {
                matchedSpecialComponents++;
            }

            // Heating output
            if (component.Key.componentType == ComponentType.Heating)
            {
                HeatingComponent heatingComponent = component.Key as HeatingComponent;
                heatOutput += heatingComponent.producedHeat;
            }
            else if (component.Key.componentType == ComponentType.Chip || component.Key.componentType == ComponentType.Sensor)
            {
                ElectronicComponent electronicComponent = component.Key as ElectronicComponent;
                heatOutput += electronicComponent.producedHeat;
            }
            else if (component.Key.componentType == ComponentType.Power)
            {
                PowerSourceComponent powerComponent = component.Key as PowerSourceComponent;
                heatOutput += powerComponent.producedHeat;
            }

            // Cooling output
            if (component.Key.componentType == ComponentType.Cooling)
            {
                CoolingComponent coolingComponent = component.Key as CoolingComponent;
                coolingOutput += coolingComponent.reducedHeat;
            }

            // CPU slots
            if (component.Key.componentType == ComponentType.Chip)
            {
                ElectronicComponent electronicComponent = component.Key as ElectronicComponent;
                operationalCPUSlots += electronicComponent.operationalCPUSlots;
            }
        }

        // Checks if the product is below the heat threshold
        float finalHeat = heatOutput - coolingOutput;
        Debug.Log(finalHeat);
        if (finalHeat <= product.maxSustainedHeat)
            product.hasSufficientCooling = true;
        Debug.Log(product.maxSustainedHeat);
        if (!product.hasSufficientCooling)
        {
            Debug.Log("Product is past the heat threshold!");
            return;
        }

        // Checks if the amount of components in the blueprint matches the required amount
        if (matchedRegularComponents >= product.mandatoryRegularComponents.Count)
            product.hasRegularComponents = true;
        if (matchedSpecialComponents >= product.mandatorySpecialComponents.Count)
            product.hasSpecialComponents = true;

        if (product.hasSpecialComponents && product.hasRegularComponents)
        {
            AssembleProduct();
            AudioManager.instance.PlayAudioClip(AudioManager.instance.completeConstruction, transform, 0.9f);
            DeskUIManager.instance.DisplayProductPopup();
            BlueprintManager.instance.ClearBlueprint();

            BlueprintManager.ClearActiveMission();
            if (BlueprintManager.instance.activeOrderScreenUI != null)
            {
                BlueprintManager.instance.activeOrderScreenUI.EndMission();
            }
            ResetRequirements();
        }
        else
        {
            Debug.Log("Missing required components!");
        }

        // Reset flags
        ResetFlags(ref product);
    }
    private void AssembleProduct()
    {
        float reliabilityRating = 0, availabilityRating = 0, maintainabilityRating = 0, safetyRating = 0;
        KoruFormula(ref reliabilityRating, ref availabilityRating, ref maintainabilityRating, ref safetyRating);

        string ramsSummary = $"RAMS Ratings:\n" +
                             $"- Reliability: {reliabilityRating}\n" +
                             $"- Availability: {availabilityRating}\n" +
                             $"- Maintainability: {maintainabilityRating}\n" +
                             $"- Safety: {safetyRating}";
        DeskUIManager.instance.RAMSRatings.text = ramsSummary;
    }
    private (float, float, float, float) KoruFormula(ref float reliabilityRating, ref float availabilityRating, ref float maintainabilityRating, ref float safetyRating)
    {
        float reliabilityModifier = 1, availabilityModifier = 1, maintainabilityModifier = 1, safetyModifier = 1;
        SetModifiers(ref reliabilityModifier, ref availabilityModifier, ref maintainabilityModifier, ref safetyModifier);

        // Sums up the ratings of each product's rating multiplied by the amount of cells it occupies in the blueprint
        foreach (var componentInBlueprint in componentsInBlueprint)
        {
            ComponentData component = componentInBlueprint.Key;
            int cellsOccupied = componentInBlueprint.Value;
            reliabilityRating += component.reliabilityRating * cellsOccupied;
            availabilityRating += component.availabilityRating * cellsOccupied;
            maintainabilityRating += component.maintainabilityRating * cellsOccupied;
            safetyRating += component.safetyRating * cellsOccupied;
        }

        // Multiplies the final sum by its modifier and divides the product's final rating by the total amount of cells occupied in the blueprint
        reliabilityRating = (reliabilityRating * reliabilityModifier) / numberOfCellsOccupied;
        availabilityRating = (availabilityRating * availabilityModifier) / numberOfCellsOccupied;
        maintainabilityRating = (maintainabilityRating * maintainabilityModifier) / numberOfCellsOccupied;
        safetyRating = (safetyRating * safetyModifier) / numberOfCellsOccupied;

        return (reliabilityRating, availabilityRating, maintainabilityRating, safetyRating);
    }
    private void SetModifiers(ref float reliabilityProduct, ref float availabilityProduct, ref float maintainabilityProduct, ref float safetyProduct)
    {
        // Calculates modifiers based on the structural components placed in the blueprint
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
    private void ResetFlags(ref ProductData product)
    {
        product.hasRegularComponents = false;
        product.hasSpecialComponents = false;
    }
    private void ResetRequirements()
    {
        heatOutput = 0;
        coolingOutput = 0;
        operationalCPUSlots = 0;
        powerWattage = 0;
    }
    public void PlayButtonSFX()
    {

        AudioManager.instance.PlayAudioClip(AudioManager.instance.buttonPress1, transform, 1f);

    }

    public void PlayTimeline()
    {
        // Plays the timeline animation for product assembly
        director.Play();
    }

}