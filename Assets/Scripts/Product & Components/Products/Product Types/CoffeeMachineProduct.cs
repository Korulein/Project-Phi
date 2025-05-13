using UnityEngine;
[CreateAssetMenu(fileName = "Coffee Machine Product", menuName = "Blueprint/Product/Coffee Machine")]
public class CoffeeMachineProduct : ProductData
{
    [Header("Extras")]
    public bool hasModularity;
    public bool hasMultiBeverageMode;
    public bool hasGoodCoffee;
}
