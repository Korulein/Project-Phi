using UnityEngine;
[CreateAssetMenu(fileName = "Coffee Machine Product", menuName = "Blueprint/Product/Coffee Machine")]
public class CoffeeMachineProduct : ProductData
{
    [Header("Mandatory Characteristics")]
    public bool hasPressurisedCoffeeExtractor;
    public bool hasHeatingElement;
    public bool hasAutoShutoff;
    public bool hasSealant;

    [Header("Extras")]
    public bool hasModularity;
    public bool hasMultiBeverageMode;
    public bool hasGoodCoffee;
}
