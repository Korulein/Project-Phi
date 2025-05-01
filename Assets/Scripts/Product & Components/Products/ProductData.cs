using System.Collections.Generic;
using UnityEngine;
public class ProductData : ScriptableObject
{
    [Header("Product Information")]
    public string productName;
    public Sprite productSprite;
    [TextArea] public string productDescription;
    public int productID;
    public List<ComponentData> requiredComponents;
    public List<ComponentData> componentsUsed;

    [Header("RAMS ratings")]
    public float reliabilityRating;
    public float availabilityRating;
    public float safetyRating;
    public float maintainabilityRating;

}
