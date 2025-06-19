using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Playables;

public class ProductManager : MonoBehaviour
{
    public static ProductManager instance { get; private set; }

    [Header("References")]
    private Dictionary<ComponentData, int> componentsInBlueprintAtAssemblyTime;
    public List<ComponentData> componentsInBlueprintAtRuntime;
    private int numberOfCellsOccupied;

    [Header("Animator Director")]
    public PlayableDirector director;

    [Header("Products")]
    [SerializeField] public List<ProductData> products = new List<ProductData>();

    [Header("Product Requirements")]
    [SerializeField] private float heatOutput = 0;
    [SerializeField] private float coolingOutput = 0;
    [SerializeField] private float productHeatThreshold = 0;
    [SerializeField] private int electronicComponentsInProduct = 0;
    [SerializeField] private int operationalCPUSlots = 0;
    [SerializeField] private float requiredPowerWattage = 0;
    [SerializeField] private float powerInProduct = 0;
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

    #region Assemble Product
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
        (componentsInBlueprintAtAssemblyTime, numberOfCellsOccupied) = BlueprintManager.instance.GetAllPlacedComponents();

        // Product references
        ProductData product = GetProductData(blueprint.blueprintID);
        int matchedRegularComponents = 0;
        int matchedSpecialComponents = 0;
        productHeatThreshold = product.maxSustainedHeat;

        ResetRequirements();
        ResetFlags(ref product);

        // HashSet declaration
        var regularSet = new HashSet<ComponentType>(product.mandatoryRegularComponents.Select(c => c.componentType));
        var specialSet = new HashSet<ComponentType>(product.mandatorySpecialComponents.Select(c => c.componentType));

        // Counts matched component types
        // Calculates power wattage, heat output and operational electronic component slots
        foreach (var component in componentsInBlueprintAtAssemblyTime)
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

            // Heating output && Power
            if (component.Key.componentType == ComponentType.Heating)
            {
                HeatingComponent heatingComponent = component.Key as HeatingComponent;
                heatOutput += heatingComponent.producedHeat;
                requiredPowerWattage += heatingComponent.requiredPower;
            }
            else if (component.Key.componentType == ComponentType.Chip || component.Key.componentType == ComponentType.Sensor)
            {
                ElectronicComponent electronicComponent = component.Key as ElectronicComponent;
                heatOutput += electronicComponent.producedHeat;
                requiredPowerWattage += electronicComponent.requiredPower;
            }
            else if (component.Key.componentType == ComponentType.Power || component.Key.componentType == ComponentType.PowerTransformer)
            {
                PowerSourceComponent powerComponent = component.Key as PowerSourceComponent;
                heatOutput += powerComponent.producedHeat;
                powerInProduct += powerComponent.maxPowerOutput;
            }

            // Cooling output && Power
            if (component.Key.componentType == ComponentType.Cooling)
            {
                CoolingComponent coolingComponent = component.Key as CoolingComponent;
                coolingOutput += coolingComponent.reducedHeat;
                requiredPowerWattage += coolingComponent.requiredPower;
            }

            // CPU slots
            if (component.Key.componentType == ComponentType.Chip)
            {
                ElectronicComponent electronicComponent = component.Key as ElectronicComponent;
                operationalCPUSlots += electronicComponent.operationalCPUSlots;
            }
            else if (component.Key.componentType == ComponentType.Sensor)
            {
                electronicComponentsInProduct++;
            }
        }

        // Power Multiplier
        foreach (var powerTransformer in BlueprintManager.instance.powerTransformersInBlueprint)
        {
            powerInProduct = powerInProduct * powerTransformer.powerMultiplier;
            Debug.Log(powerInProduct);
        }

        // Checks if the product is below the heat threshold
        float finalHeat = heatOutput - coolingOutput;
        if (finalHeat > product.maxSustainedHeat)
        {
            Debug.Log("Product is past the heat threshold!");
            return;
        }

        float finalPower = powerInProduct - requiredPowerWattage;
        if (finalPower < 0)
        {
            Debug.Log("Product does not have enough power!");
            return;
        }

        // Checks electronic component slot availability
        if (electronicComponentsInProduct > operationalCPUSlots)
        {
            Debug.Log("Not enough operational electronic component slots!");
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
            ResetFlags(ref product);
        }
        else
        {
            Debug.Log("Missing required components!");
        }
    }
    private void AssembleProduct()
    {
        float reliabilityRating = 0, availabilityRating = 0, maintainabilityRating = 0, safetyRating = 0;
        KoruFormula(ref reliabilityRating, ref availabilityRating, ref maintainabilityRating, ref safetyRating);
        RoundFloatsToOneDecimal(ref reliabilityRating, ref availabilityRating, ref maintainabilityRating, ref safetyRating);
        reliabilityRating = reliabilityRating > 100 ? 100 : reliabilityRating;
        availabilityRating = availabilityRating > 100 ? 100 : availabilityRating;
        maintainabilityRating = maintainabilityRating > 100 ? 100 : maintainabilityRating;
        safetyRating = safetyRating > 100 ? 100 : safetyRating;

        string ramsSummary = $"RAMS Ratings:\n" +
                             $"- Reliability: {reliabilityRating}\n" +
                             $"- Availability: {availabilityRating}\n" +
                             $"- Maintainability: {maintainabilityRating}\n" +
                             $"- Safety: {safetyRating}";
        DeskUIManager.instance.RAMSRatings.text = ramsSummary;
    }
    #endregion

    #region Calculations
    private (float, float, float, float) KoruFormula(ref float reliabilityRating, ref float availabilityRating, ref float maintainabilityRating, ref float safetyRating)
    {
        float reliabilityModifier = BlueprintManager.instance.finalReliabilityModifier;
        float availabilityModifier = BlueprintManager.instance.finalAvailabilityModifier;
        float maintainabilityModifier = BlueprintManager.instance.finalMaintainabilityModifier;
        float safetyModifier = BlueprintManager.instance.finalSafetyModifier;

        // Sums up the ratings of each product's rating multiplied by the amount of cells it occupies in the blueprint
        foreach (var componentInBlueprint in componentsInBlueprintAtAssemblyTime)
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
    public void UpdateModifiers(ref float reliabilityProduct, ref float availabilityProduct, ref float maintainabilityProduct, ref float safetyProduct)
    {
        // Calculates modifiers based on the structural components placed in the blueprint
        foreach (var componentInBlueprint in BlueprintManager.instance.structuralComponentsInBlueprint)
        {
            ComponentData component = componentInBlueprint;
            StructuralComponent structuralComponent = (StructuralComponent)component;

            reliabilityProduct *= structuralComponent.reliabilityModifier;
            availabilityProduct *= structuralComponent.availabilityModifier;
            maintainabilityProduct *= structuralComponent.maintainabilityModifier;
            safetyProduct *= structuralComponent.safetyModifier;

        }

        // Calculates modifiers based on component adjacency
        List<AdjacencyModifier> modifiers = BlueprintManager.instance.GetModifiers();
        foreach (var modifier in modifiers)
        {
            GetCompoundedValues(ref reliabilityProduct, ref availabilityProduct, ref maintainabilityProduct, ref safetyProduct, modifier);
        }
    }
    private void GetCompoundedValues(ref float reliability, ref float availability, ref float maintainability, ref float safety, AdjacencyModifier modifier)
    {
        switch (modifier.compoundingType)
        {
            case CompoundingType.Linear:
                reliability *= modifier.reliabilityModifier;
                availability *= modifier.availabilityModifier;
                maintainability *= modifier.maintainabilityModifier;
                safety *= modifier.safetyModifier;
                break;
            case CompoundingType.Diminishing:
                reliability *= (1f - Mathf.Pow(0.8f, modifier.reliabilityModifier));
                break;
            case CompoundingType.Exponential:
                reliability *= Mathf.Pow(1.2f, modifier.reliabilityModifier - 1);
                break;
            case CompoundingType.Logarithmic:
                reliability *= Mathf.Log(modifier.reliabilityModifier + 1);
                break;
        }
    }
    public void RoundFloatsToOneDecimal(ref float reliability, ref float availability, ref float maintainability, ref float safety)
    {
        float roundedReliability = (float)Math.Round(reliability, 1);
        float roundedAvailability = (float)Math.Round(availability, 1);
        float roundedMaintainability = (float)Math.Round(maintainability, 1);
        float roundedSafety = (float)Math.Round(safety, 1);

        reliability = roundedReliability;
        availability = roundedAvailability;
        maintainability = roundedMaintainability;
        safety = roundedSafety;
    }
    #endregion

    #region Resets
    private void ResetFlags(ref ProductData product)
    {
        product.hasRegularComponents = false;
        product.hasSpecialComponents = false;
    }
    private void ResetRequirements()
    {
        electronicComponentsInProduct = 0;
        heatOutput = 0;
        coolingOutput = 0;
        operationalCPUSlots = 0;
        requiredPowerWattage = 0;
        powerInProduct = 0;
    }
    #endregion

    #region SFX
    public void PlayButtonSFX()
    {

        AudioManager.instance.PlayAudioClip(AudioManager.instance.buttonPress1, transform, 1f);

    }
    #endregion

    #region Animator
    public void PlayTimeline()
    {
        // Plays the timeline animation for product assembly
        director.Play();
    }
    #endregion

}